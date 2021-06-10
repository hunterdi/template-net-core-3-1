using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Architecture
{
	public static class ErrorHandlerConfigurationExtension
	{
		//public static IApplicationBuilder UseErrorHandler(this IApplicationBuilder app)
		//{
		//	app.UseMiddleware<GlobalExceptionHandlerMiddleware>();

		//	return app;
		//}

		public static IServiceCollection AddErrorHandler(this IServiceCollection services)
		{
			services.AddTransient<GlobalExceptionHandlerMiddleware>();

			return services;
		}
	}
}
