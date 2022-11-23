using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using System;
using System.Linq;
using TeamsAllocationManager.Api;

namespace TeamsAllocationManager.Tests.Helpers;

[TestFixture]
public class HandlersHelperTests
{
	private readonly ServiceCollection _services;

	// given
	private interface ITest<TQuery, TResult> { }
	private interface ITest<TQuery> { }
	private interface ITestQuery<TType> { }
	private class TestQuery : ITestQuery<bool> { }
	private class Test : ITest<TestQuery, bool> { }
	private class Test2 : ITest<TestQuery> { }

	public HandlersHelperTests()
	{
		_services = new ServiceCollection();
	}

	[Test]
	public void Add_NewHandler_AddsHandler()
	{
		// when
		HandlersHelper.AddHandlers(_services, new Type[] { typeof(ITest<,>), typeof(ITest<>) });

		// then
		bool result = _services.Any(x => x.ServiceType == typeof(ITest<TestQuery, bool>));
		bool result2 = _services.Any(x => x.ServiceType == typeof(ITest<TestQuery>));

		Assert.IsTrue(result);
		Assert.IsTrue(result2);
	}
}
