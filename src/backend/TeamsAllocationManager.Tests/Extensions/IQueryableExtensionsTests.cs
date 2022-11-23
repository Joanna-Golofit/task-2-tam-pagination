using NUnit.Framework;
using Shouldly;
using System.Collections.Generic;
using System.Linq;
using TeamsAllocationManager.Infrastructure.Extensions;

namespace TeamsAllocationManager.Tests.Extensions;

[TestFixture]
public class IQueryableExtensionsTests
{
	[Test]
	public void Where_IsEnabled_FiltersResults()
	{
		// given
		var data = new List<int> { 1, 2, 4, 5, 7, 8 };
		IQueryable<int> queryable = data.AsQueryable();

		// when
		var result = queryable.Where(true, x => x > 5).ToList();

		// then
		result.Count.ShouldBe(2);
		result.ShouldContain(7);
		result.ShouldContain(8);
	}

	[Test]
	public void Where_IsDisabled_ReturnNotFilteredResults()
	{
		// given
		var data = new List<int> { 1, 2, 4, 5, 7, 8 };
		IQueryable<int> queryable = data.AsQueryable();

		// when
		var result = queryable.Where(false, x => x > 5).ToList();

		// then
		result.Count.ShouldBe(6);
		result.ShouldBe(data);
	}
}
