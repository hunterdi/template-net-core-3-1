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
		private static string _corsPolicy = "RestApi";

		public static IServiceCollection AddCorsConfiguration(this IServiceCollection services)
		{
			services.AddCors(options =>
			{
				options.AddPolicy(_corsPolicy, builderPolicy =>
				{
					builderPolicy
					.SetIsOriginAllowed(origin => true)
					.AllowAnyMethod()
					.AllowAnyHeader()
					.AllowCredentials();
				});
			});

			return services;
		}


		public static IApplicationBuilder UseCorsConfiguration(this IApplicationBuilder app)
        {
			app.UseCors(_corsPolicy);
			
			return app;
        }
	}
}
