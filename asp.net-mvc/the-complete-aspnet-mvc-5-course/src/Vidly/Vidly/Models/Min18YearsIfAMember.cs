namespace Vidly.Models
{
	using System;
	using System.ComponentModel.DataAnnotations;

	public class Min18YearsIfAMember : ValidationAttribute
	{
		protected override ValidationResult IsValid(object value, ValidationContext validationContext)
		{
			Customer customer = validationContext.ObjectInstance as Customer;

			if (customer == null)
			{
				return new ValidationResult("Internal error: ObjectInstance cannot cast to Customer type");
			}

			if (customer.MembershipTypeId == MembershipType.Unknown ||
				customer.MembershipTypeId == MembershipType.PayAsYouGo)
			{
				return ValidationResult.Success;
			}

			if (customer.BirthDate == null)
			{
				return new ValidationResult("Birth date is required.");
			}

			int age = DateTime.Today.Year - customer.BirthDate.Value.Year;

			return age >= 18 
				? ValidationResult.Success 
				: new ValidationResult("Customer age soluld be at least 18 years old to go on a membership.");
		}
	}
}