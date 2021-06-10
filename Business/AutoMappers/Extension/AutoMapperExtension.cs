using AutoMapper;
using AutoMapper.Extensions.ExpressionMapping;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace Business
{
    public static class AutoMapperExtension
    {
        public static IServiceCollection AddMapperConfiguration(this IServiceCollection services)
        {
            var configMapper = new MapperConfiguration(configuration =>
            {
                configuration.AddProfile<AuthenticationMapper>();
                configuration.AddProfile<TagMapper>();
                configuration.AddProfile<TaskMapper>();
                configuration.AddProfile<TaskListMapper>();
                configuration.AddProfile<TagTaskMapper>();
            });

            services.AddSingleton(configMapper.CreateMapper());

            return services;
        }

        public static IMappingExpression<TSource, TDestination> Ignore<TSource, TDestination>(this IMappingExpression<TSource, 
            TDestination> map, Expression<Func<TDestination, object>> selector)
        {
            map.ForMember(selector, config => config.Ignore());
            return map;
        }
    }
}
