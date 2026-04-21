using System;
using System.ComponentModel.DataAnnotations;


namespace Psz.Core.FinanceControl.Models.Accounting.Validation
{
	public class FNCDateValidation: ValidationAttribute
	{
		public string SpanStart { get; set; }
		public string SpanEnd { get; set; }
		protected override ValidationResult IsValid(object value, ValidationContext validationContext)
		{
			if(value is DateTime)
			{
				var data = (DateTime)value;
				if(data >= DateTime.Parse(SpanStart) && data <= DateTime.Parse(SpanEnd))
				{
					return ValidationResult.Success;
				}
			}
			return new ValidationResult(ErrorMessage ?? "Invalid Dates, too wide time period !");
		}
	}
}
