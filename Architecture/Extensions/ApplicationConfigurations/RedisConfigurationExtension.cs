using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IO;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using BC = BCrypt.Net.BCrypt;

namespace Architecture
{
    public static class RedisConfigurationExtension
    {
        public static IServiceCollection AddRedisConfiguration(this IServiceCollection service, IConfiguration configuration)
        {
            //service.AddDistributedRedisCache(options =>
            //{
            //    options.Configuration = configuration.GetConnectionString("Redis");
            //    options.InstanceName = "RestAPi";
                //options.ConfigurationOptions.ConnectRetry = 5;
            //});

            service.AddSingleton<IConnectionMultiplexer>(ConnectionMultiplexer.Connect(configuration.GetConnectionString("Redis")));

            return service;
        }

        //public static IApplicationBuilder UseRedisMiddleware(this IApplicationBuilder app)
        //{
        //    app.UseMiddleware<RedisMiddleware>();

        //    return app;
        //}
    }
}
