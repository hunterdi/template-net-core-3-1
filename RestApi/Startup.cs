using Architecture;
using Autofac;
using Business;
using Business.Anotations;
using FluentValidation.AspNetCore;
using GraphQL;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Cors;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using Repositories;
using Seed;
using Serilog;
using Serilog.Extensions.Logging;
using Services;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;

namespace RestApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            //services.Configure<RequestLocalizationOptions>(options =>
            //{
            //    options.DefaultRequestCulture = new RequestCulture("pt-BR");
            //    options.SupportedCultures = new List<CultureInfo> { new CultureInfo("pt-BR") };
            //});

            services.AddSingleton<ILoggerFactory>(x => new SerilogLoggerFactory(null, false));
            services.Configure<EmailSettings>(Configuration.GetSection("EmailSettings"));
            services.Configure<JWTSettings>(Configuration.GetSection("JWTSettings"));
            services.Configure<ConnectionStrings>(Configuration.GetSection("ConnectionStrings"));

            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.SuppressModelStateInvalidFilter = true;
            });

            services.AddCorsConfiguration();

            services.AddControllers(options =>
            {
                options.Filters.Add(typeof(ValidateModelStateFilter));
            })
            .AddNewtonsoftJson(options =>
            {
                options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                options.SerializerSettings.NullValueHandling = NullValueHandling.Include;
                options.SerializerSettings.Formatting = Formatting.Indented;
                options.SerializerSettings.Converters.Add(new StringEnumConverter(new CamelCaseNamingStrategy()));
                options.SerializerSettings.Converters.Add(new MemoryStreamJsonConverter());
            })
            .AddFluentValidation(v => v.RegisterValidatorsFromAssemblyContaining<IBaseValidator>())
            .AddControllersAsServices()
            .SetCompatibilityVersion(CompatibilityVersion.Version_3_0);

            services.Configure<FormOptions>(options =>
            {
                options.MemoryBufferThreshold = Int32.MaxValue;
                options.ValueLengthLimit = Int32.MaxValue;
                options.MultipartBodyLengthLimit = Int32.MaxValue;
            });

            services.AddMiddlewareConfigurations();
            services.AddDbContextConfiguration(Configuration);
            services.AddMapperConfiguration();
            //services.AddRedisConfiguration(Configuration);
            services.AddSwaggerConfiguration();
            //services.AddHealthChecksConfiguration();
            //services.AddGraphQLConfiguration();
            //services.AddApplicationInsightsTelemetry(Configuration);
        }

        public void ConfigureContainer(ContainerBuilder builder)
        {
            builder.RegisterModule(new RepositoryModule());
            builder.RegisterModule(new ServicesModule());
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.All
            });

            app.Use(async (context, _next) =>
            {
                context.Request.EnableBuffering();
                await _next.Invoke();
            });

            app.UseMiddlewareConfigurations();
            app.UseSerilogRequestLogging();
            app.UseSwaggerConfiguration();

            app.UseStaticFiles();
            app.UseStaticFiles(new StaticFileOptions()
            {
                FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), @"StaticFiles")),
                RequestPath = new PathString("/StaticFiles")
            });
            app.UseWebSockets();
            app.UseRouting();
            app.UseCorsConfiguration();
            //app.UseHttpsRedirection();

            app.UseAuthorization();

            //app.UseGraphQLConfiguration(env);
            //app.UseHealthChecksConfiguration();

            app.UseEndpoints(endpoints =>
            {
                //endpoints.MapHealthChecksUI();
                endpoints.MapControllers();
            });
        }
    }
}