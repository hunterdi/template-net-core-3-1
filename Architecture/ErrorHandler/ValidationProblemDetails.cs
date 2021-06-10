using System;
using System.Collections.Generic;
using System.Text;

namespace Architecture
{
	public class ValidationProblemDetails : ProblemDetails
	{
		public override bool Successful => (string.IsNullOrWhiteSpace(Detail) && (ValidationErrors?.Count == 0));
		public ICollection<ValidationError> ValidationErrors { get; set; }
	}
}
