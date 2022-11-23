using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Moq;
using Newtonsoft.Json;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TeamsAllocationManager.Api.Functions;
using TeamsAllocationManager.Contracts.Base;
using TeamsAllocationManager.Contracts.Employee.Commands;
using TeamsAllocationManager.Contracts.Employee.Queries;
using TeamsAllocationManager.Contracts.LoggedUser.Queries;
using TeamsAllocationManager.Domain.Models;
using TeamsAllocationManager.Dtos.Employee;
using TeamsAllocationManager.Dtos.Enums;

namespace TeamsAllocationManager.Tests.Functions;

[TestFixture]
public class EmployeeFunctionTests
{
	private readonly ILogger _mockedLogger;
	private readonly Mock<IDispatcher> _dispatcherMock;

	public EmployeeFunctionTests()
	{
		_mockedLogger = new Mock<ILogger>().Object;
		_dispatcherMock = new Mock<IDispatcher>();
	}

	[Test]
	public async Task ShouldCallGetTeamLeadersCommand()
		=> await VerifyFunctionExecutionAsync(c => c.DispatchAsync<GetTeamLeadersQuery, IEnumerable<TeamLeaderDto>>(It.IsAny<GetTeamLeadersQuery>(), default), "GET", "GetTeamLeaders");

	[Test]
	public async Task ShouldCallUpdateEmployeeWorkspaceTypeCommand()
	{
		var dto = new UpdateEmployeeWorkspaceTypeDto
		{
			EmployeeId = Guid.NewGuid(),
			WorkspaceType = WorkspaceType.Office
		};

		var dtos = new List<UpdateEmployeeWorkspaceTypeDto> { dto };

		await VerifyFunctionExecutionAsync(c => c.DispatchAsync<UpdateEmployeesWorkspaceTypesCommand, bool>(It.IsAny<UpdateEmployeesWorkspaceTypesCommand>(), default),
			"PUT",
			"UpdateEmployeesWorkspaceTypes",
			body: new MemoryStream(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(dtos))));
	}

	private async Task VerifyFunctionExecutionAsync(Expression<Action<IDispatcher>> expression, string verb, string path = "", QueryCollection? query = null, MemoryStream? body = null)
	{
		// given
		var function = new EmployeeFunction(_dispatcherMock.Object);
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
