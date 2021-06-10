using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NpgsqlTypes;
using Serilog;
using Serilog.Sinks.PostgreSQL;
using Serilog.Sinks.Elasticsearch;
using System.Reflection;
using Serilog.Exceptions;
using Serilog.Events;

namespace Architecture
{
    public static class LoggerConfigurationExtension
    {
        private static Dictionary<string, ColumnWriterBase> _columnWriters = new Dictionary<string, ColumnWriterBase>
        {
            { "message", new RenderedMessageColumnWriter(NpgsqlDbType.Text) },
            { "message_template", new MessageTemplateColumnWriter(NpgsqlDbType.Text) },
            { "level", new LevelColumnWriter(true, NpgsqlDbType.Varchar) },
            { "raise_date", new TimestampColumnWriter(NpgsqlDbType.Timestamp) },
            { "exception", new ExceptionColumnWriter(NpgsqlDbType.Text) },
            { "properties", new LogEventSerializedColumnWriter(NpgsqlDbType.Jsonb) },
            { "props_test", new PropertiesColumnWriter(NpgsqlDbType.Jsonb) },
            { "machine_name", new SinglePropertyColumnWriter("MachineName", PropertyWriteMethod.ToString, NpgsqlDbType.Text, "l") }
        };

        public static IHostBuilder AddSerialogConfiguration(this IHostBuilder hostBuilder)
        {
            hostBuilder.ConfigureLogging((hostingContext, logging) =>
            {
                logging.ClearProviders();
                logging.AddSerilog(dispose: true);
            })
            .UseSerilog();

            return hostBuilder;
        }

        private static ElasticsearchSinkOptions ConfigureElasticSink(IConfigurationRoot configuration, string environment)
        {
            return new ElasticsearchSinkOptions(new Uri(configuration["ElasticConfiguration:Uri"]))
            {
                AutoRegisterTemplate = true,
                IndexFormat = $"{Assembly.GetExecutingAssembly().GetName().Name.ToLower().Replace(".", "-")}-{environment?.ToLower().Replace(".", "-")}-{DateTime.UtcNow:yyyy-MM}"
            };
        }

        public static Serilog.Core.Logger CreateLoggerConfiguration(this LoggerConfiguration loggerConfiguration)
        {
            //.ConfigureAppConfiguration((hostCtx, config) =>
            //{
            //    var _environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            //    var _configuration = new ConfigurationBuilder()
            //        .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            //        .AddJsonFile(
            //            $"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")}.json",
            //            optional: true)
            //        .Build();

            //    Log.Logger = new Serilog.LoggerConfiguration()
            //        .ReadFrom.Configuration(_configuration)
            //        .MinimumLevel.Debug()
            //        .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
            //        .Enrich.FromLogContext()
            //        .Enrich.WithExceptionDetails()
            //        .Enrich.WithMachineName()
            //        .WriteTo.PostgreSQL(_configuration.GetSection("ConnectionString").Value, "Logs", _columnWriters)
            //        .WriteTo.Elasticsearch(ConfigureElasticSink(_configuration, _environment))
            //        .ReadFrom.Configuration(_configuration)
            //        .CreateLogger();
            //})
            return loggerConfiguration.MinimumLevel.Debug()
                .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
                .Enrich.FromLogContext()
                .WriteTo.Debug()
                .WriteTo.Console(outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj} {Properties:j}{NewLine}{Exception}")
                .WriteTo.File(
                    @".\Logs\log.txt",
                    fileSizeLimitBytes: 1_000_000,
                    rollOnFileSizeLimit: true,
                    shared: true,
                    flushToDiskInterval: TimeSpan.FromSeconds(1))
                .CreateLogger();
        }
    }
}
