using Architecture;
using HotChocolate;
using HotChocolate.AspNetCore;
using HotChocolate.AspNetCore.Playground;
using HotChocolate.Execution.Configuration;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Text;

namespace GraphQL
{
    public static class GraphQLExtension
    {
        public static IServiceCollection AddGraphQLConfiguration(this IServiceCollection services)
        {
            //services.AddDataLoaderRegistry();
            //services.AddGraphQLServer(
            //    SchemaBuilder.New()
            //    .AddQueryType<TaskQuery>()
            //    .Create(),
            //    new QueryExecutionOptions { ForceSerialExecution = true });
            return services;
        }

        public static IApplicationBuilder UseGraphQLConfiguration(this IApplicationBuilder app, IWebHostEnvironment env)
        {
            //app.UseGraphQL("/api/graphql");

            if (env.IsDevelopment())
            {
                //app.UsePlayground(new PlaygroundOptions
                //{
                //    QueryPath = "/api/graphql",
                //    Path = "/playground",
                //});
            }

            //app.Map("/api/graphql", appBuilder =>
            //{
            //    appBuilder.UseMiddleware<GraphQLMiddleware>();
            //});

            //app.Map("/playground", appBuilder =>
            //{
            //    appBuilder.UseMiddleware<GraphQLMiddleware>();
            //});

            return app;
        }
    }
}
