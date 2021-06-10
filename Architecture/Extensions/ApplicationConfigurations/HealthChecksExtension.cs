using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Mime;
using System.Text;
using System.Text.Json;

namespace Architecture
{
    public static class HealthChecksExtension
    {
        public static IServiceCollection AddHealthChecksConfiguration(this IServiceCollection services)
        {
            var serviceProvider = services.BuildServiceProvider();
            var connectionScrings = serviceProvider.GetRequiredService<IOptions<ConnectionStrings>>();

            //URL healthcheck use a IHttpClientFactory that can be configured to disable certificate validation.
            //services.AddHttpClient("https://localhost:5001/healthchecks-data-ui")
            //    .ConfigurePrimaryHttpMessageHandler(() => new HttpClientHandler()
            //    {
            //        ClientCertificateOptions = ClientCertificateOption.Manual,
            //        ServerCertificateCustomValidationCallback = (httpRequestMessage, cert, cetChain, policyErrors) =>
            //        {
            //            return true;
            //        }
            //    });

            //services.AddHealthChecksUI()
            //    .AddPostgreSqlStorage(connectionScrings.Value.Postgres);
                //.AddUrlGroup(new Uri("http://httpbin.org/status/200"));

            services.AddHealthChecks()
                .AddHealthChecksOptionsConfiguration(connectionScrings);

            return services;
        }

        public static IHealthChecksBuilder AddHealthChecksOptionsConfiguration(this IHealthChecksBuilder services, IOptions<ConnectionStrings> options)
        {

            if (!string.IsNullOrWhiteSpace(options.Value.Redis))
            {
                services.AddRedis(options.Value.Redis);
            }

            if (!string.IsNullOrWhiteSpace(options.Value.Postgres))
            {
                services.AddNpgSql(options.Value.Postgres);
            }

            return services;
        }

        public static IApplicationBuilder UseHealthChecksConfiguration(this IApplicationBuilder app)
        {
            // Gera o endpoint que retornará os dados utilizados no dashboard
            app.UseHealthChecks("/status", new HealthCheckOptions()
            {
                Predicate = _ => true,
                ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
            });

            // Ativa o dashboard para a visualização da situação de cada Health Check
            //app.UseHealthChecksUI();

            // Endpoint para retorno de informações sobre Health Checks
            // a serem utilizados pelo Worker Process de Monitoramento
            app.UseHealthChecks("/status-ui",
               new HealthCheckOptions()
               {
                   ResponseWriter = async (context, report) =>
                   {
                       var result = JsonSerializer.Serialize(
                            report.Entries.Select(e => new
                            {
                                healthCheck = e.Key,
                                error = e.Value.Exception?.Message,
                                status = Enum.GetName(typeof(HealthStatus), e.Value.Status)
                            }));
                       context.Response.ContentType = MediaTypeNames.Application.Json;
                       await context.Response.WriteAsync(result);
                   }
               });

            return app;
        }
    }
}
