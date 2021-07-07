using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Architecture
{
    public static class MiddlewareConfiguration
    {
        public static IServiceCollection AddMiddlewareConfigurations(this IServiceCollection services)
        {
            services.AddTransient<GlobalExceptionHandlerMiddleware>();
            
            return services;
        }

        public static IApplicationBuilder UseMiddlewareConfigurations(this IApplicationBuilder app)
        {
            //app.UseMiddleware<RequestResponseLoggingMiddleware>();
            app.UseMiddleware<GlobalExceptionHandlerMiddleware>();
            app.UseMiddleware<JwtMiddleware>();
            //app.UseMiddleware<RedisMiddleware>();
            //app.UseMiddleware<TransactionMiddleware>();

            return app;
        }
    }
}
