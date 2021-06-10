using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace Architecture
{
    public static class DbContextConfigurationExtension
    {
        public static IServiceCollection AddDbContextConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            var serviceProvider = services
                .AddEntityFrameworkSqlServer()
                //.AddEntityFrameworkInMemoryDatabase()
                .BuildServiceProvider();

            services.AddDbContextPool<ApplicationDataContext>(option =>
            {
                //option.UseInMemoryDatabase(configuration.GetConnectionString("InMemory"));
                option.UseNpgsql(configuration.GetConnectionString("Postgres"), build =>
                {
                    build.EnableRetryOnFailure(5, TimeSpan.FromSeconds(10), null);
                    build.MigrationsAssembly("RestApi");
                });
                option.EnableDetailedErrors();
                option.UseInternalServiceProvider(serviceProvider);
            }, 20);

            //services.AddScoped<DbContext, ApplicationDataContext>();

            return services;
        }
    }
}
