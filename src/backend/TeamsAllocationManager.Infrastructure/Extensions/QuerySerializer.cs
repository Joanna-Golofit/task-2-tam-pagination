using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Reflection;

namespace TeamsAllocationManager.Infrastructure.Extensions;

public static class QuerySerializer
{
	public static object? Deserialize(Type type, IQueryCollection entries, string? prefix = null, string? parameterName = null)
	{
		if (prefix == null && !string.IsNullOrWhiteSpace(parameterName))
		{
			if (type.IsValueType) // Is value type (including primitives)
			{
				return GetValueOfSimpleProperty(entries, prefix, parameterName, type) ?? Activator.CreateInstance(type);
			}
			else if (type == typeof(string)) // Is string
			{
				return GetValueOfSimpleProperty(entries, prefix, parameterName, type);
			}
			else if (typeof(IEnumerable).IsAssignableFrom(type)) // Is collection
			{
				return GetValueOfCollectionProperty(entries, prefix, parameterName, type);
			}
			else // Is complex object
			{
				return Deserialize(type, entries, string.Empty);
			}
		}
		else
		{
			object? obj = Activator.CreateInstance(type);
			PropertyInfo[] properties = type
				.GetProperties()
				.Where(x => x.CanWrite)
				.ToArray();

			foreach (PropertyInfo property in properties)
			{
				Type propertyType = property.PropertyType;
				object? value;

				if (propertyType.IsValueType || propertyType == typeof(string)) // Is value type (including primitives) or string
				{
					value = GetValueOfSimpleProperty(entries, prefix, property.Name, property.PropertyType);
				}
				else if (typeof(IEnumerable).IsAssignableFrom(propertyType)) // Is collection
				{
					value = GetValueOfCollectionProperty(entries, prefix, property.Name, property.PropertyType);
				}
				else // Is complex object
				{
					value = GetValueOfComplexProperty(entries, prefix, property.Name, property.PropertyType);
				}

				if (value != null)
				{
					property.SetValue(obj, value);
				}
			}

			return obj;
		}
	}

	private static object? Parse(string stringValue, Type dataType)
	{
		try
		{
			if (stringValue == null)
			{
				return null;
			}

			TypeConverter converter = TypeDescriptor.GetConverter(dataType);
			object? value = converter.ConvertFromString(null, CultureInfo.InvariantCulture, stringValue);
			return value;
		}
		catch
		{
			return null;
		}
	}

	private static object? GetValueOfSimpleProperty(IQueryCollection entries, string? prefix, string name, Type type)
	{
		KeyValuePair<string, StringValues> parameter = entries
			.LastOrDefault(e => e.Key.Equals($"{prefix}{name}", StringComparison.OrdinalIgnoreCase));
		string stringValue = parameter.Value;

		return Parse(stringValue, type);
	}

	private static IList? GetValueOfCollectionProperty(IQueryCollection entries, string? prefix, string name, Type type)
	{
		KeyValuePair<string, StringValues> parameter = entries
			.LastOrDefault(e => e.Key.Equals($"{prefix}{name}", StringComparison.OrdinalIgnoreCase));
		string stringValue = parameter.Value;

		string[]? items = stringValue?.Split(',');

		Type itemsType = type.GetGenericArguments()[0];
		Type listType = typeof(List<>).MakeGenericType(itemsType);
		var list = (IList)Activator.CreateInstance(listType)!;

		if (items != null && items.Any())
		{
			foreach (string item in items)
			{
				object? parsedValue = Parse(item, itemsType);

				if (parsedValue != null)
				{
					list.Add(parsedValue);
				}
			}
		}
		
		return list;
	}

	private static object? GetValueOfComplexProperty(IQueryCollection entries, string? prefix, string name, Type type)
	{
		KeyValuePair<string, StringValues> parameter = entries
			.LastOrDefault(e => e.Key.StartsWith($"{prefix}{name}", StringComparison.OrdinalIgnoreCase));

		return Deserialize(type, entries, $"{prefix}{name}.");
	}
}
