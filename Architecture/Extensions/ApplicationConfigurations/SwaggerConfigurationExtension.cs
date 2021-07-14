using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.OpenApi.Models;

namespace Architecture
{
    public static class SwaggerConfigurationExtension
    {
		public static IServiceCollection AddSwaggerConfiguration(this IServiceCollection services)
		{
			services.AddSwaggerGenNewtonsoftSupport();
			services.AddSwaggerGen(c =>
			{
				c.SwaggerDoc("v1", 
					new OpenApiInfo 
					{ 
						Title = "Rest API", 
						Version = "v1",
						Description = "Exemplo de API REST criada com o ASP.NET Core 3.1",
						Contact = new OpenApiContact
                        {
							Email = "jurandiraraujo85@gmail.com",
							Name = "Jurandir Araújo",
							Url = new Uri("https://github.com/hunterdi")
                        }
					});
			});

			return services;
		}

		public static IApplicationBuilder UseSwaggerConfiguration(this IApplicationBuilder app)
		{
			app.UseSwagger();
			app.UseSwaggerUI(c =>
			{
				c.RoutePrefix = string.Empty;
				c.SwaggerEndpoint("/swagger/v1/swagger.json", "Rest API v1");
			});

			return app;
		}
	}
}
