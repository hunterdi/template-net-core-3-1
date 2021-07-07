using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;

namespace Architecture
{
	public class GlobalExceptionHandlerMiddleware : IMiddleware
	{
		private readonly ILogger<GlobalExceptionHandlerMiddleware> _logger;

		public GlobalExceptionHandlerMiddleware(ILogger<GlobalExceptionHandlerMiddleware> logger)
		{
			this._logger = logger;
		}

		public async Task InvokeAsync(HttpContext context, RequestDelegate next)
		{
			try
			{
				await next.Invoke(context);
			}
			catch (Exception ex)
			{
				this._logger.LogError(JsonConvert.SerializeObject(ex));
				await HandleExceptionAsync(context, ex);
			}
		}

		private static Task HandleExceptionAsync(HttpContext context, Exception exception)
		{
			context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
			context.Response.ContentType = MediaTypeNames.Application.Json;

			var problemDetails = new ValidationProblemDetails
			{
				Status = (int)HttpStatusCode.InternalServerError,
				Title = "EXCEPTION_ERROR",
				Instance = Guid.NewGuid().ToString(),
				Detail = exception.Message,
				ValidationErrors = null,
				InnerException = exception.InnerException != null ? exception.InnerException.Message : null,
				StackTrace = exception.InnerException != null && !string.IsNullOrWhiteSpace(exception.InnerException.StackTrace) ? exception.InnerException.StackTrace : exception.StackTrace
			};

			switch (exception)
			{
				case ApplicationException e:
				case ValidationException e1:
				case FluentValidation.ValidationException e2:
					context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
					problemDetails.Status = (int)HttpStatusCode.BadRequest;
					problemDetails.Title = "VALIDATION_ERROR";
					problemDetails.Detail = "SEE VALIDATIONERRORS FOR DETAILS";
					problemDetails.ValidationErrors = JsonConvert.DeserializeObject<ICollection<ValidationError>>(exception.Message);
					break;
				case KeyNotFoundException e:
					context.Response.StatusCode = (int)HttpStatusCode.NotFound;
					problemDetails.Status = (int)HttpStatusCode.NotFound;
					break;
				case UnauthorizedAccessException e:
					context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
					problemDetails.Status = (int)HttpStatusCode.Unauthorized;
					break;
				case TokenExperidedException e:
					context.Response.StatusCode = (int)HttpStatusCode.NetworkAuthenticationRequired;
					problemDetails.Status = (int)HttpStatusCode.NetworkAuthenticationRequired;
					break;
				default:
					context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
					problemDetails.Status = (int)HttpStatusCode.InternalServerError;
					break;
			}

			return context.Response.WriteAsync(JsonConvert.SerializeObject(problemDetails));
		}
	}
}
