using System;
using System.Linq;
using System.Linq.Expressions;

namespace TeamsAllocationManager.Infrastructure.Extensions;

public static class IQueryableExtensions
{
	public static IQueryable<TModel> Where<TModel>(this IQueryable<TModel> @this, bool isEnabled, Expression<Func<TModel, bool>> predicate)
		=> isEnabled ? @this.Where(predicate) : @this;
}
