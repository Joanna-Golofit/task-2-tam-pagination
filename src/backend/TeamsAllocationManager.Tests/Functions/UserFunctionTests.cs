using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Primitives;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq.Expressions;
using System.Threading;
using TeamsAllocationManager.Api.Functions;
using TeamsAllocationManager.Contracts.Base;
using TeamsAllocationManager.Contracts.LoggedUser.Queries;
using TeamsAllocationManager.Domain.Models;
using TeamsAllocationManager.Dtos.Employee;

namespace TeamsAllocationManager.Tests.Functions;

[TestFixture]
public class UserFunctionTests
{
	private readonly ILogger _mockedLogger;
	private readonly Mock<IDispatcher> _dispatcherMock;

	public UserFunctionTests()
	{
		_mockedLogger = new Mock<ILogger>().Object;
		_dispatcherMock = new Mock<IDispatcher>();
	}

	[Test]
	public void ShouldCallGetIsUserAdminQuery()
		=> VerifyFunctionExecutionAsync(c
			=> c.DispatchAsync<GetIsUserAdminQuery, bool>(It.IsAny<GetIsUserAdminQuery>(), default), "GET", "GetIsUserAdmin");

	[Test]
	public void ShouldCallGetLoggedUserDataQuery()
	{
		_dispatcherMock.Setup(d => d.DispatchAsync<GetUserRoleQuery, IEnumerable<string>>(It.IsAny<GetUserRoleQuery>(), It.IsAny<CancellationToken>()))
			            .ReturnsAsync(new[] { RoleEntity.Admin });

		VerifyFunctionExecutionAsync(c
				=> c.DispatchAsync<GetLoggedUserDataQuery, LoggedUserDataDto>(It.IsAny<GetLoggedUserDataQuery>(), default), "GET", "GetLoggedUserData",
			new QueryCollection(new Dictionary<string, StringValues>()
			{
				{ "loggedUserEmail", "test@test.com" }
			})
		);
	}

	private void VerifyFunctionExecutionAsync(Expression<Action<IDispatcher>> expression, string verb, string path = "", QueryCollection? query = null, MemoryStream? body = null)
	{
		// given
		var function = new UserFunction(_dispatcherMock.Object);
		var reqMock = new Mock<HttpRequest>();
		reqMock.Setup(r => r.Method).Returns(verb);
		reqMock.Setup(r => r.Query).Returns(query ?? new QueryCollection());
		reqMock.Setup(r => r.Body).Returns(body ?? new MemoryStream());

		// when
		function.RunAsync(reqMock.Object, path, _mockedLogger).Wait();

		// then
		_dispatcherMock.Verify(expression, Times.Once);
	}
}
