using NUnit.Framework;
using Shouldly;
using System.Linq;
using System.Reflection;
using TeamsAllocationManager.Infrastructure.Extensions;

namespace TeamsAllocationManager.Tests.Extensions;

[TestFixture]
public class ParameterInfoExtensionsTests
{
	[Test]
	public void ShouldReturnTrueForNullableParam()
	{
		ParameterInfo[] parameters = this.GetType().GetMethod(nameof(TestMethod), (BindingFlags)int.MaxValue)!.GetParameters();

		parameters.Single(p => p.Name == "nullableParam").IsNullable().ShouldBe(true);
		parameters.Single(p => p.Name == "notNullable").IsNullable().ShouldBe(false);

	}

	private void TestMethod(object? nullableParam, object notNullable)
	{
	}
}
