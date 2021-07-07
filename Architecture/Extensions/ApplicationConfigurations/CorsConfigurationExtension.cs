using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Architecture
{
    public static class CorsConfigurationExtension
    {
        public static IServiceCollection AddCorsConfiguration(this IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddDefaultPolicy(builderPolicy =>
                {
                    builderPolicy
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials()
                    .SetIsOriginAllowed(origin => true) // allow any origin
                    .SetPreflightMaxAge(TimeSpan.FromSeconds(5000));
                });

                options.AddPolicy("RestApi", builderPolicy =>
                {
                    builderPolicy
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials()
                    .SetIsOriginAllowed(origin => true) // allow any origin
                    .SetPreflightMaxAge(TimeSpan.FromSeconds(5000));
                });
            });

            return services;
        }


        public static IApplicationBuilder UseCorsConfiguration(this IApplicationBuilder app)
        {
            app.UseCors(builder => builder
                .AllowAnyMethod()
                .AllowAnyHeader()
                .AllowCredentials()
                .SetIsOriginAllowed(origin => true) // allow any origin
                .SetPreflightMaxAge(TimeSpan.FromSeconds(5000)));

            app.UseCors("RestApi");

            return app;
        }
    }
}
