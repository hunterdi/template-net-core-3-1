using Business;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using Microsoft.EntityFrameworkCore;
using LinqKit;
using System.Reflection;

namespace Architecture
{
    //https://stackoverflow.com/questions/60515202/parameterized-query-from-an-expression-tree-in-entity-framework-core
    public static class QuerybleExtension
    {
        private sealed class HoldPropertyValue<T>
        {
            public T v;
        }

        public static IQueryable<T> WhereEquals<T, TValue>(this IQueryable<T> query, string propertyName, TValue propertyValue)
        {
            // p
            var pe = Expression.Parameter(typeof(T));

            var property = Expression.PropertyOrField(pe, propertyName);
            var holdpv = new HoldPropertyValue<TValue> { v = propertyValue };
            //var value = Expression.Constant(propertyValue);
            var value = Expression.PropertyOrField(Expression.Constant(holdpv), "v");

            var predicateBody = Expression.Equal(
                property,
                value
            );
            var wf = Expression.Lambda<Func<T, bool>>(predicateBody, new ParameterExpression[] { pe });

            //var v = (int)propertyValue;
            //Expression<Func<Accounts,bool>> wf = (Accounts a) => a.Actid == v;

            var whereCallExpression = Expression.Call(
                typeof(Queryable),
                "Where",
                new[] { typeof(T) },
                query.Expression,
                wf
            );

            return query.Provider.CreateQuery<T>(whereCallExpression);
        }

        public static IQueryable<T> WhereEquals2<T>(this IQueryable<T> query, string propertyName, object propertyValue)
        {
            // p
            var pe = Expression.Parameter(typeof(T), "p");
            // p.propertyName
            var property = Expression.PropertyOrField(pe, propertyName);
            var holdpv = new HoldPropertyValue<object> { v = propertyValue };
            // Convert.ChangeType(holdpv.v, property.Type)
            var value = Expression.Convert(Expression.PropertyOrField(Expression.Constant(holdpv), "v"), property.Type);

            // p.propertyName == Convert.ChangeType(holdpv.v, property.Type)
            var predicateBody = Expression.Equal(
                property,
                value
            );
            // p => p.propertyName == Convert.ChangeType(holdpv.v, property.Type)
            var wf = Expression.Lambda<Func<T, bool>>(predicateBody, new ParameterExpression[] { pe });

            // Queryable.Where(p => p.propertyName == Convert.ChangeType(holdpv.v, property.Type))
            var whereCallExpression = Expression.Call(
                typeof(Queryable),
                "Where",
                new[] { typeof(T) },
                query.Expression,
                wf
            );

            // query.Where(p => p.propertyName == Convert.ChangeType(holdpv.v, property.Type))
            return query.Provider.CreateQuery<T>(whereCallExpression);
        }

        //public static IQueryable<T> LoadRelated<T>(this IQueryable<T> originalQuery)
        //{
        //    Func<IQueryable<T>, IQueryable<T>> includeFunc = f => f;
        //    foreach (var prop in typeof(T).GetProperties()
        //        .Where(p => Attribute.IsDefined(p, typeof(BaseDomain))))
        //    {
        //        Func<IQueryable<T>, IQueryable<T>> chainedIncludeFunc = f => f.Include<T, T>(prop.Name);
        //        includeFunc = Compose(includeFunc, chainedIncludeFunc);
        //    }
        //    return includeFunc(originalQuery);
        //}

        //public static IQueryable<T> LoadRelated<T>(this IQueryable<T> originalQuery, ICollection<string> includes)
        //{
        //    if (includes == null || !includes.Any()) return originalQuery;

        //    Func<IQueryable<T>, IQueryable<T>> includeFunc = f => f;
        //    foreach (var include in includes)
        //    {
        //        IQueryable<T> ChainedFunc(IQueryable<T> f) => f.Include<T, T>(include);
        //        includeFunc = Compose(includeFunc, ChainedFunc);
        //    }

        //    return includeFunc(originalQuery);
        //}

        private static Func<T, T> Compose<T>(Func<T, T> innerFunc, Func<T, T> outerFunc)
        {
            return arg => outerFunc(innerFunc(arg));
        }

        //https://pt.stackoverflow.com/questions/199351/query-gen%C3%A9rica-com-predicatebuilder-e-linqkit
        //public static Expression<Func<TPredicate, bool>> And<TPredicate>(this ExpressionStarter<TPredicate> predicate, 
        //    Func<TPredicate, string> property, StringFilterDefinition filter, bool ignoreNull = true)
        //{
        //    if (InvalidStringFilter(filter, ignoreNull))
        //    {
        //        return predicate;
        //    }

        //    // este é o And do LinqKit
        //    return predicate.And(BuildPredicate(property, filter));
        //}

        //private static Expression<Func<TPredicate, bool>> BuildPredicate<TPredicate>(Func<TPredicate, string> property,
        //    StringFilterDefinition filter)
        //{
        //    if (filter.Filter == StringFilterComparators.Equal)
        //    {
        //        return x => property.Invoke(x) == filter.Value;
        //    }

        //    if (filter.Filter == StringFilterComparators.BeginsWith)
        //    {
        //        return x => property.Invoke(x).StartsWith(filter.Value);
        //    }

        //    if (filter.Filter == StringFilterComparators.EndsWith)
        //    {
        //        return x => property.Invoke(x).EndsWith(filter.Value);
        //    }

        //    return x => property.Invoke(x).Contains(filter.Value);
        //}

        //private static bool InvalidStringFilter(StringFilterDefinition filter,
        //    bool ignoreNullValue = true)
        //{
        //    if (filter?.Filter == null)
        //    {
        //        return true;
        //    }

        //    return ignoreNullValue && string.IsNullOrEmpty(filter.Value);
        //}

        //private static Expression<Func<TPredicate, bool>> BuildPredicate<TPredicate>(Expression<Func<TPredicate, string>> property,
        //    StringFilterDefinition filter)
        //{
        //    if (filter.Filter == StringFilterComparators.Equal)
        //    {
        //        return x => property.Invoke(x) == filter.Value;
        //    }

        //    if (filter.Filter == StringFilterComparators.BeginsWith)
        //    {
        //        return x => property.Invoke(x).StartsWith(filter.Value);
        //    }

        //    if (filter.Filter == StringFilterComparators.EndsWith)
        //    {
        //        return x => property.Invoke(x).EndsWith(filter.Value);
        //    }

        //    return x => property.Invoke(x).Contains(filter.Value);
        //}

        //https://michael-mckenna.com/sorting-iqueryables-using-strings-and-reflection/
        //items = items.OrderBy("SomeProperty");
        //items = items.OrderBy(s => s.SomeProperty);
        public static IQueryable<T> OrderBy<T>(this IQueryable<T> source, string property, bool asc = true) where T : class
        {
            //STEP 1: Verify the property is valid
            var searchProperty = typeof(T).GetProperty(property);

            if (searchProperty == null)
                throw new ArgumentException("property");

            if (!searchProperty.PropertyType.IsValueType && !searchProperty.PropertyType.IsPrimitive && !searchProperty.PropertyType.Namespace.StartsWith("System") && !searchProperty.PropertyType.IsEnum)
                throw new ArgumentException("property");

            if (searchProperty.GetMethod == null || !searchProperty.GetMethod.IsPublic)
                throw new ArgumentException("property");

            //STEP 2: Create the OrderBy property selector
            var parameter = Expression.Parameter(typeof(T), "o");
            var selectorExpr = Expression.Lambda(Expression.Property(parameter, property), parameter);

            //STEP 3: Update the IQueryable expression to include OrderBy
            Expression queryExpr = source.Expression;
            queryExpr = Expression.Call(
                typeof(Queryable),
                asc ? "OrderBy" : "OrderByDescending",
                new Type[] { source.ElementType, searchProperty.PropertyType },
                queryExpr,
                selectorExpr);

            return source.Provider.CreateQuery<T>(queryExpr);
        }

        //https://pt.stackoverflow.com/questions/125421/where-din%c3%a2mico-linq-to-sql?rq=1
        public static IQueryable<TSource> WhereIf<TSource>(this IQueryable<TSource> source, bool condition, Expression<Func<TSource, bool>> predicate)
        {
            if (!condition) return source;
            return source.Where(predicate);
        }

        #region https://stackoverflow.com/questions/49601748/dynamic-linq-where-in
        //context.Records.In(x => x.ID, 1, 2, 3)
        public static IQueryable<TSource> In<TSource, TMember>(this IQueryable<TSource> source, Expression<Func<TSource, TMember>> identifier, params TMember[] values)
        {
            var parameter = Expression.Parameter(typeof(TSource), "m");
            var inExpression = GetExpression(parameter, identifier, values);
            var theExpression = Expression.Lambda<Func<TSource, bool>>(inExpression, parameter);
            return source.Where(theExpression);
        }

        static Expression GetExpression<TSource, TMember>(ParameterExpression parameter, Expression<Func<TSource, TMember>> identifier, IEnumerable<TMember> values)
        {
            var memberName = (identifier.Body as MemberExpression).Member.Name;
            var member = Expression.Property(parameter, memberName);
            var valuesConstant = Expression.Constant(values.ToList());
            MethodInfo method = typeof(List<TMember>).GetMethod("Contains");
            Expression call = Expression.Call(valuesConstant, method, member);
            return call;
        }
        #endregion
    }

}