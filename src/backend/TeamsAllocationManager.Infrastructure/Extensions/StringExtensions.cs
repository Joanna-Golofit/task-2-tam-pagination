using System.Diagnostics.CodeAnalysis;
using Newtonsoft.Json;

namespace TeamsAllocationManager.Infrastructure.Extensions;

public static class StringExtensions
{
	[return: MaybeNull]
	public static T TryDeserialize<T>(this string? str)
	{
		if (string.IsNullOrEmpty(str))
		{
			return default;
		}

		return JsonConvert.DeserializeObject<T>(str) ?? default;
	}

	public static T DeserializeOrDefault<T>(this string? str, T defaultValue)
		=> str.TryDeserialize<T>() ?? defaultValue;
}
