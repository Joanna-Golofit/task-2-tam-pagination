using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using TeamsAllocationManager.Api.Functions;
using TeamsAllocationManager.Contracts.Base;
using TeamsAllocationManager.Contracts.Floor.Queries;
using TeamsAllocationManager.Contracts.LoggedUser.Queries;
using TeamsAllocationManager.Domain.Models;
using TeamsAllocationManager.Dtos.Floor;

namespace TeamsAllocationManager.Tests.Functions;

[TestFixture]
public class BuildingFunctionTests
{
	private readonly ILogger _mockedLogger;
	private readonly Mock<IDispatcher> _dispatcherMock;

	public BuildingFunctionTests()
	{
		_mockedLogger = new Mock<ILogger>().Object;
		_dispatcherMock = new Mock<IDispatcher>();
	}

	[Test]
	public async Task ShouldCallGetAllFloorsCommand()
	{
		// given
		var function = new FloorFunction(_dispatcherMock.Object);
		var reqMock = new Mock<HttpRequest>();
		reqMock.Setup(r => r.Method).Returns("GET");
		reqMock.Setup(r => r.Query).Returns(new QueryCollection());
		reqMock.Setup(r => r.Body).Returns(new MemoryStream());
		_dispatcherMock.Setup(d => d.DispatchAsync<GetUserRoleQuery, IEnumerable<string>>(It.IsAny<GetUserRoleQuery>(), It.IsAny<CancellationToken>()))
			            .ReturnsAsync(new[] { RoleEntity.Admin });

		// when
		await function.RunAsync(reqMock.Object, string.Empty, _mockedLogger);

		// then
		_dispatcherMock.Verify(c => c.DispatchAsync<GetAllFloorsQuery, FloorsDto>(It.IsAny<GetAllFloorsQuery>(), default), Times.Once);
	}
}
