using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TeamsAllocationManager.Infrastructure.Extensions;

public static class ObjectExtensions
{
	public static bool IsValid(this object obj, out ICollection<ValidationResult> validationResults)
	{
		validationResults = new List<ValidationResult>();
		return Validator.TryValidateObject(obj, new ValidationContext(obj, null, null), validationResults, true);
	}
}
