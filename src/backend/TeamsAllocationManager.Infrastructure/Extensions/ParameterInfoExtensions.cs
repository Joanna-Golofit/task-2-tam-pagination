using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;

namespace TeamsAllocationManager.Infrastructure.Extensions;

public static class ParameterInfoExtensions
{
	public static bool IsNullable(this ParameterInfo parameter)
	{
		if (Nullable.GetUnderlyingType(parameter.ParameterType) != null)
		{
			return true;
		}

		CustomAttributeData? nullable = parameter
						.CustomAttributes
						.FirstOrDefault(x => x.AttributeType.FullName == "System.Runtime.CompilerServices.NullableAttribute");

		if (nullable != null && nullable.ConstructorArguments.Count == 1)
		{
			CustomAttributeTypedArgument attributeArgument = nullable.ConstructorArguments[0];
			if (attributeArgument.ArgumentType == typeof(byte[]))
			{
				CustomAttributeTypedArgument? arg =
					(attributeArgument.Value as ReadOnlyCollection<CustomAttributeTypedArgument>)
						?.FirstOrDefault();

				if (arg?.ArgumentType == typeof(byte))
				{
					return (byte)arg?.Value! == 2;
				}
			}
			else if (attributeArgument.ArgumentType == typeof(byte))
			{
				return (byte)attributeArgument.Value! == 2;
			}
		}

		return false;
	}
}
