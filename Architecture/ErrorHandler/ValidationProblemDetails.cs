using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Architecture
{
	public class ValidationProblemDetails : ProblemDetails
	{
		public override bool Successful => (string.IsNullOrWhiteSpace(Detail) && (ValidationErrors?.Count == 0));

		[JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "validationErrors")]
		public ICollection<ValidationError> ValidationErrors { get; set; }
	}
}
