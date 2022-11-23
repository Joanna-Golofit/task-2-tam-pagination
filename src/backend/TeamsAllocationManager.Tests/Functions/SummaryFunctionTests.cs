using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using TeamsAllocationManager.Api.Functions;
using TeamsAllocationManager.Contracts.Base;
using TeamsAllocationManager.Contracts.LoggedUser.Queries;
using TeamsAllocationManager.Contracts.Summary.Queries;
using TeamsAllocationManager.Domain.Models;
using TeamsAllocationManager.Dtos.Summary;

namespace TeamsAllocationManager.Tests.Functions;

[TestFixture]
public class SummaryFunctionTests
{
	private readonly ILogger _mockedLogger;
	private readonly Mock<IDispatcher> _dispatcherMock;

	public SummaryFunctionTests()
	{
		_mockedLogger = new Mock<ILogger>().Object;
		_dispatcherMock = new Mock<IDispatcher>();
	}

	[Test]
	public async Task ShouldCallGetSummary() =>
		await VerifyFunctionExecutionAsync(c => c.DispatchAsync<GetSummaryQuery, SummaryDto>(It.IsAny<GetSummaryQuery>(), default), "GET");

	/// <summary>
	/// In case of request body that holds reference type (dto class), moq verify feature (times.once) does not detect execution
	/// of command because dto created based on JSON deserialization (this way FunctionBase binds request body content with function input parameter)
	/// is of different instance - what means to moq a different value for input parameter of mocked method (being tested).
	/// (In general Moq does not detect method execution if value of input param of method used in expression object differs
	/// from input value of method of mocked object being tested).
	/// </summary>
	/// 
	private async Task VerifyFunctionExecutionAsync(Expression<Action<IDispatcher>> expression, string verb, string path = "", QueryCollection? query = null, MemoryStream? body = null)
	{
		// given
		var function = new SummaryFunction(_dispatcherMock.Object);
		var reqMock = new Mock<HttpRequest>();
		reqMock.Setup(r => r.Method).Returns(verb);
		reqMock.Setup(r => r.Query).Returns(query ?? new QueryCollection());
		reqMock.Setup(r => r.Body).Returns(body ?? new MemoryStream());
		_dispatcherMock.Setup(d => d.DispatchAsync<GetUserRoleQuery, IEnumerable<string>>(It.IsAny<GetUserRoleQuery>(), It.IsAny<CancellationToken>()))
			            .ReturnsAsync(new[] { RoleEntity.Admin });

		// when
		await function.RunAsync(reqMock.Object, path, _mockedLogger);

		// then
		_dispatcherMock.Verify(expression, Times.Once);
	}
}
