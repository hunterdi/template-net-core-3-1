using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Architecture
{
	public class ValidateModelStateFilter : ActionFilterAttribute
	{
		private readonly ILogger<ValidateModelStateFilter> _logger;

		public ValidateModelStateFilter(ILogger<ValidateModelStateFilter> logger)
		{
			this._logger = logger;
		}

		public override void OnActionExecuting(ActionExecutingContext context)
		{
			if (!context.ModelState.IsValid)
			{
				var modelStateEntries = context.ModelState.Where(e => e.Value.Errors.Count > 0).ToList();
				var errors = new List<ValidationError>();
				
				if (modelStateEntries.Any())
				{
					errors = context.ModelState.Values.Where(v => v.Errors.Count > 0)
					.SelectMany(v => v.Errors)
					.Select(v => new ValidationError
					{
						Description = v.ErrorMessage
					})
					.ToList();
				}

				this._logger.LogError(JsonConvert.SerializeObject(errors));

				throw new ValidationException(JsonConvert.SerializeObject(errors));
			}

            base.OnActionExecuting(context);
        }

        public override Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            return base.OnActionExecutionAsync(context, next);
        }
    }
}
