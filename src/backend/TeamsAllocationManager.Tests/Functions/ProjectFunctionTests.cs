using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
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
using TeamsAllocationManager.Contracts.Project.Queries;
using TeamsAllocationManager.Domain.Models;
using TeamsAllocationManager.Dtos.Common;
using TeamsAllocationManager.Dtos.Project;

namespace TeamsAllocationManager.Tests.Functions;

[TestFixture]
public class ProjectFunctionTests
{
	private readonly ILogger _mockedLogger;
	private readonly Mock<IDispatcher> _dispatcherMock;

	public ProjectFunctionTests()
	{
		_mockedLogger = new Mock<ILogger>().Object;
		_dispatcherMock = new Mock<IDispatcher>();
	}

	[Test]
	public void ShouldCallGetProjectDetailsQuery()
		=> VerifyFunctionExecutionAsync(c
			=> c.DispatchAsync<GetProjectDetailsQuery, ProjectDetailsDto?>(It.IsAny<GetProjectDetailsQuery>(), default), "GET", $"{ Guid.NewGuid()}");

	[Test]
	public void ShouldCallGetAllProjectsQuery()
		=> VerifyFunctionExecutionAsync(c
			=> c.DispatchAsync<GetProjectsQuery, PagedListResultDto<IEnumerable<ProjectDto>>>(It.IsAny<GetProjectsQuery>(), default), "GET");

	[Test]
	public void ShouldCallGetAllProjectsForDropdownQuery()
		=> VerifyFunctionExecutionAsync(c
			=> c.DispatchAsync<GetAllProjectsForDropdownQuery, IEnumerable<ProjectForDropdownDto>>(It.IsAny<GetAllProjectsForDropdownQuery>(), default), "GET", "GetAllProjectsForDropdown");

	private void VerifyFunctionExecutionAsync(Expression<Action<IDispatcher>> expression, string verb, string path = "", QueryCollection? query = null, MemoryStream? body = null)
	{
		// given
		var function = new ProjectFunction(_dispatcherMock.Object);
		var reqMock = new Mock<HttpRequest>();
		reqMock.Setup(r => r.Method).Returns(verb);
		reqMock.Setup(r => r.Query).Returns(query ?? new QueryCollection());
		reqMock.Setup(r => r.Body).Returns(body ?? new MemoryStream());
		_dispatcherMock.Setup(d => d.DispatchAsync<GetUserRoleQuery, IEnumerable<string>>(It.IsAny<GetUserRoleQuery>(), It.IsAny<CancellationToken>()))
			            .ReturnsAsync(new[] { RoleEntity.Admin });

		// when
		function.RunAsync(reqMock.Object, path, _mockedLogger).Wait();

		// then
		_dispatcherMock.Verify(expression, Times.Once);
	}
}
