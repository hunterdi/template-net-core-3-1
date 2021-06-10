using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using YamlDotNet.Serialization;

namespace Architecture
{
    public static class ObjectExtension
    {
        private static ICollection<Type> _types = new List<Type>
        {
            typeof(string),
            typeof(int),
            typeof(decimal),
            typeof(float),
            typeof(double),
            typeof(bool),
            typeof(Guid),
            typeof(DateTime)
        };

        public static IDictionary<string, object> GetProperties(this object obj)
        {
            if (obj == null) return new Dictionary<string, object>();

            var result = new Dictionary<string, object>();

            var type = obj.GetType();
            var properties = type.GetProperties();

            if (properties.Length > 0)
            {
                result = properties.ToDictionary(e => e.Name, e => e.GetValue(obj, null));
            }

            return result;
        }

        public static IDictionary<string, object> GetPropertiesNotNull(this object obj)
        {
            if (obj == null) return new Dictionary<string, object>();

            var result = new Dictionary<string, object>();

            var type = obj.GetType();
            var properties = type.GetProperties();

            if (properties.Length > 0)
            {
                foreach (var property in properties)
                {
                    if (_types.Contains(property.PropertyType))
                    {
                        if (property.PropertyType.Equals(typeof(Guid)))
                        {
                            var value = type.GetProperty(property.Name).GetValue(obj, null);
                            if (value != null && (Guid)value != Guid.Empty)
                            {
                                result.Add(property.Name, value);
                            }
                        }
                        else if (property.PropertyType.Equals(typeof(string)))
                        {
                            var value = type.GetProperty(property.Name).GetValue(obj, null);
                            if (value != null && !string.IsNullOrWhiteSpace(value.ToString()))
                            {
                                result.Add(property.Name, value);
                            }
                        }
                        else
                        {
                            var value = type.GetProperty(property.Name).GetValue(obj, null);
                            if (value != null)
                            {
                                result.Add(property.Name, value);
                            }
                        }
                    }
                }
            }

            return result;
        }

        public static bool DateTimeIsValid(this DateTime propertie)
        {
            return propertie != DateTime.MinValue;
        }

        public static Expression<Func<T, bool>> BuildWhereExpression<T>(string nameValueQuery) where T : class
        {
            Expression<Func<T, bool>> predicate = null;
            PropertyInfo prop = null;
            var fieldName = nameValueQuery.Split("=")[0];
            var fieldValue = nameValueQuery.Split("=")[1];
            var properties = typeof(T).GetProperties();

            foreach (var property in properties)
            {
                if (property.Name.ToLower() == fieldName.ToLower())
                {
                    prop = property;
                }
            }

            if (prop != null)
            {
                var isNullable = prop.PropertyType.IsNullableType();
                var parameter = Expression.Parameter(typeof(T), "x");
                var member = Expression.Property(parameter, fieldName);

                if (isNullable)
                {
                    var filter1 = Expression.Constant(Convert.ChangeType(fieldValue, member.Type.GetGenericArguments()[0]));
                    Expression typeFilter = Expression.Convert(filter1, member.Type);
                    var body = Expression.Equal(member, typeFilter);
                    predicate = Expression.Lambda<Func<T, bool>>(body, parameter);
                }
                else
                {
                    if (prop.PropertyType == typeof(string) /*&& likeOerator.ToLower() == "like"*/)
                    {
                        var parameterExp = Expression.Parameter(typeof(T), "type");
                        var propertyExp = Expression.Property(parameterExp, prop);
                        MethodInfo method = typeof(string).GetMethod("Contains", new[] { typeof(string) });
                        var someValue = Expression.Constant(fieldValue, typeof(string));
                        var containsMethodExp = Expression.Call(propertyExp, method, someValue);
                        predicate = Expression.Lambda<Func<T, bool>>(containsMethodExp, parameterExp);
                    }
                    else
                    {
                        var constant = Expression.Constant(Convert.ChangeType(fieldValue, prop.PropertyType));
                        var body = Expression.Equal(member, constant);
                        predicate = Expression.Lambda<Func<T, bool>>(body, parameter);
                    }
                }
            }
            return predicate;
        }

        public static bool IsNullableType(this Type type)
        {
            return type.IsGenericType && (type.GetGenericTypeDefinition().Equals(typeof(Nullable<>)));
        }

        public static Dictionary<string, object> GetPropertyValues(this object obj)
        {
            return obj.GetType().GetPublicPropertiesWithSetters()
                .ToDictionary(pi => pi.Name, pi => pi.GetValue(obj, null));
        }

        public static IEnumerable<PropertyInfo> GetPublicPropertiesWithSetters(this Type type)
        {
            return type.GetProperties().Where(pi => pi.CanWrite);
        }

        public static bool IsNonStringEnumerable(this PropertyInfo pi)
        {
            return pi != null && pi.PropertyType.IsNonStringEnumerable();
        }

        public static bool IsNonStringEnumerable(this object instance)
        {
            return instance != null && instance.GetType().IsNonStringEnumerable();
        }

        public static bool IsNonStringEnumerable(this Type type)
        {
            if (type == null || type == typeof(string))
                return false;
            return typeof(IEnumerable).IsAssignableFrom(type) || typeof(IEnumerable<>).IsAssignableFrom(type)
                || typeof(ICollection).IsAssignableFrom(type) || typeof(ICollection<>).IsAssignableFrom(type)
                || type.IsSubclassOf(typeof(IEnumerable)) || type.IsSubclassOf(typeof(ICollection))
                || type.IsSubclassOf(typeof(IEnumerable<>)) || type.IsSubclassOf(typeof(ICollection<>))
                || (type.IsClass && type.GetInterfaces().Contains(typeof(IEnumerable)))
                || (type.IsClass && type.GetInterfaces().Contains(typeof(IEnumerable<>)))
                || (type.IsClass && type.GetInterfaces().Contains(typeof(ICollection)))
                || (type.IsClass && type.GetInterfaces().Contains(typeof(ICollection<>)));
        }

        #region https://coderethinked.com/building-dynamic-linq-queries-using-expression-trees-and-func/
        public static Func<T, bool> GetDynamicQueryWithExpresionTrees<T>(string propertyName, string val) where T : class
        {
            var param = Expression.Parameter(typeof(T), "x");
            #region Convert to specific data type
            MemberExpression member = Expression.Property(param, propertyName);
            UnaryExpression valueExpression = GetValueExpression(propertyName, val, param);
            #endregion
            Expression body = Expression.Equal(member, valueExpression);
            var final = Expression.Lambda<Func<T, bool>>(body: body, parameters: param);
            return final.Compile();
        }

        public static UnaryExpression GetValueExpression(string propertyName, string val, ParameterExpression param)
        {
            var member = Expression.Property(param, propertyName);
            var propertyType = ((PropertyInfo)member.Member).PropertyType;
            var converter = TypeDescriptor.GetConverter(propertyType);
            if (!converter.CanConvertFrom(typeof(string)))
            {
                throw new NotSupportedException();
            }
            var propertyValue = converter.ConvertFromInvariantString(val);
            var constant = Expression.Constant(propertyValue);
            return Expression.Convert(constant, propertyType);
        }
        #endregion

        public static List<Expression<Func<T, object>>> GetPropertiesExpressions<T>(this object obj) where T : class
        {
            List<Expression<Func<T, object>>> expressions = new List<Expression<Func<T, object>>>();
            var properties = typeof(T).GetProperties();

            foreach (var property in properties)
            {
                ConstantExpression instance = Expression.Constant((T)Activator.CreateInstance(typeof(T)));
                MemberExpression propertyExpression = Expression.Property(instance, property);

                Expression<Func<T, object>> exp = u => propertyExpression;

                expressions.Add(exp);
            }

            return expressions;
        }
    }
}
