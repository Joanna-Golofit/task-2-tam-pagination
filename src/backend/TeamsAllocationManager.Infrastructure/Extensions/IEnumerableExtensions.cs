using System;
using System.Collections.Generic;
using System.Linq;

namespace TeamsAllocationManager.Infrastructure.Extensions;

public static class IEnumerableExtensions
{
	public static TType SingleByType<TModel, TType>(this IEnumerable<TModel> @this)
		where TType : class, TModel
		=> @this.Single(x => x is TType) as TType ??
				throw new InvalidOperationException("Value not found.");
}
