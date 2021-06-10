﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;

namespace Architecture
{
	public static class HttpExtensions
	{
		private static readonly RouteData EmptyRouteData = new RouteData();
		private static readonly ActionDescriptor EmptyActionDescriptor = new ActionDescriptor();
		private static readonly JsonSerializer Serializer = new JsonSerializer
		{
			NullValueHandling = NullValueHandling.Ignore
		};

		public static bool IsAjaxRequest(this HttpRequest request)
		{
			if (request == null)
				throw new ArgumentNullException("request");
			if (request.Headers != null)
				return request.Headers["X-Requested-With"] == "XMLHttpRequest";
			return false;
		}

		public static void WriteJson<T>(this HttpResponse response, T obj, string contentType = null)
		{
			response.ContentType = contentType ?? MediaTypeNames.Application.Json;
			using (var writer = new HttpResponseStreamWriter(response.Body, Encoding.UTF8))
			{
				using (var jsonWriter = new JsonTextWriter(writer))
				{
					jsonWriter.CloseOutput = false;
					jsonWriter.AutoCompleteOnClose = false;

					Serializer.Serialize(jsonWriter, obj);
				}
			}
		}

		public static Task WriteJsonAsync<T>(this HttpResponse response, T obj)
		{
			response.ContentType = MediaTypeNames.Application.Json;
			return response.WriteAsync(JsonConvert.SerializeObject(obj));
		}

		public static Task WriteJsonAsync<TModel>(this HttpContext context, TModel model)
		{
			return context.ExecuteResultAsync(new JsonResult(model));
		}

		public static async Task<T> ReadFromJsonAsync<T>(this HttpContext httpContext)
		{
			using (var streamReader = new StreamReader(httpContext.Request.Body))
			using (var jsonTextReader = new JsonTextReader(streamReader))
			{
				var obj = Serializer.Deserialize<T>(jsonTextReader);

				var results = new List<ValidationResult>();
				if (Validator.TryValidateObject(obj, new ValidationContext(obj), results))
				{
					return obj;
				}

				httpContext.Response.StatusCode = 400;
				await httpContext.Response.WriteJsonAsync(results);

				return default(T);
			}
		}

		public static Task WriteModelAsync<TModel>(this HttpContext context, TModel model)
		{
			var result = new ObjectResult(model)
			{
				DeclaredType = typeof(TModel)
			};

			var executor = context.RequestServices.GetRequiredService<ObjectResultExecutor>();
			var routerData = context.GetRouteData() ?? EmptyRouteData;
			var actionContext = new ActionContext(context, routerData, EmptyActionDescriptor);

			return executor.ExecuteAsync(actionContext, result);
		}

		public static Task ExecuteResultAsync<TResult>(this HttpContext context, TResult result)
			where TResult : IActionResult
		{
			if (context == null) throw new ArgumentNullException(nameof(context));
			if (result == null) throw new ArgumentNullException(nameof(result));

			var executor = context.RequestServices.GetRequiredService<IActionResultExecutor<TResult>>();

			var routeData = context.GetRouteData() ?? EmptyRouteData;
			var actionContext = new ActionContext(context, routeData, EmptyActionDescriptor);

			return executor.ExecuteAsync(actionContext, result);
		}

		public static (IOutputFormatter SelectedFormatter, OutputFormatterWriteContext FormatterContext) SelectFormatter<TModel>(this HttpContext context, TModel model)
		{
			if (context == null) throw new ArgumentNullException(nameof(context));
			if (model == null) throw new ArgumentNullException(nameof(model));

			var selector = context.RequestServices.GetRequiredService<OutputFormatterSelector>();
			var writerFactory = context.RequestServices.GetRequiredService<IHttpResponseStreamWriterFactory>();
			var formatterContext = new OutputFormatterWriteContext(context, writerFactory.CreateWriter, typeof(TModel), model);

			var selectedFormatter = selector.SelectFormatter(formatterContext, Array.Empty<IOutputFormatter>(), new MediaTypeCollection());
			return (selectedFormatter, formatterContext);
		}

		public static Guid AccountID(this HttpContext context, JWTSettings jwtSettings)
        {
			var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
			var key = Encoding.ASCII.GetBytes(jwtSettings.Secret);

			var tokenHandler = new JwtSecurityTokenHandler();
			SecurityToken validatedToken = null;

			try
			{
				tokenHandler.ValidateToken(token, new TokenValidationParameters
				{
					ValidateIssuerSigningKey = true,
					IssuerSigningKey = new SymmetricSecurityKey(key),
					ValidateIssuer = false,
					ValidateAudience = false,
					ClockSkew = DateTime.Now.AddHours(11).TimeOfDay
				}, out validatedToken);
			}
			catch (Exception e)
			{
				throw new TokenExperidedException(string.Format("Token expired {0}!", e.Message));
			}

			var jwtToken = (JwtSecurityToken)validatedToken;
			return Guid.Parse(jwtToken.Claims.First(x => x.Type == "id").Value);
		}
	}
}
