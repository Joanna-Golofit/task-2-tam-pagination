using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Moq;
using Newtonsoft.Json;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq.Expressions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TeamsAllocationManager.Api.Functions;
using TeamsAllocationManager.Contracts.Base;
using TeamsAllocationManager.Contracts.Desks.Commands;
using TeamsAllocationManager.Contracts.EntityQueries;
using TeamsAllocationManager.Contracts.LoggedUser.Queries;
using TeamsAllocationManager.Contracts.Room.Queries;
using TeamsAllocationManager.Domain.Models;
using TeamsAllocationManager.Dtos.Desk;
using TeamsAllocationManager.Dtos.Room;

namespace TeamsAllocationManager.Tests.Functions;

[TestFixture]
public class RoomFunctionTests
{
	private readonly ILogger _mockedLogger;
	private readonly Mock<IDispatcher> _dispatcherMock;
	private readonly Mock<IEnumerable<IEntityQuery>> _entityQueriesMock;

	public RoomFunctionTests()
	{
		_mockedLogger = new Mock<ILogger>().Object;
		_dispatcherMock = new Mock<IDispatcher>();
		_entityQueriesMock = new Mock<IEnumerable<IEntityQuery>>();
	}

	[Test]
	public async Task ShouldCallGetRoomQuery() =>
		await VerifyFunctionExecutionAsync(c => c.DispatchAsync<GetRoomDetailsQuery, RoomDetailsDto>(It.IsAny<GetRoomDetailsQuery>(), default), "GET", $"{ Guid.Parse("ceecfa8d-d91f-4b37-8833-846ecd889200")}");

	[Test]
	public async Task ShouldCallGetAllRoomsQuery() =>
		await VerifyFunctionExecutionAsync(c => c.DispatchAsync<GetAllRoomsQuery, RoomsDto>(It.IsAny<GetAllRoomsQuery>(), default), "GET");

	[Test]
	public async Task ShouldCallRemoveTeamFromRoomCommand() =>
		await VerifyFunctionExecutionAsync(c => c.DispatchAsync<RemoveTeamFromRoomCommand>(It.IsAny<RemoveTeamFromRoomCommand>(), default), "PUT", "ceecfa8d-d91f-4b37-8833-846ecd889200/RemoveTeamFromRoom/ceecfa8d-d91f-4b37-8833-846ecd889200");

	[Test]
	public async Task ShouldCallDeleteDesksCommand() =>
		await VerifyFunctionExecutionAsync(c => c.DispatchAsync<DeleteDesksFromRoomCommand, bool>(It.IsAny<DeleteDesksFromRoomCommand>(), default), "DELETE",
		"ceecfa8d-d91f-4b37-8833-846ecd889200/DeleteDesks",
		body: new MemoryStream(Encoding.UTF8.GetBytes("['ceecfa8d-d91f-4b37-8833-846ecd889200']")));

	[Test]
	public async Task ShouldCallAllocateDesksCommand()
	{
		var dto = new AllocateDesksDto
		{
			DeskIds = new List<Guid> { Guid.Parse("ceecfa8d-d91f-4b37-8833-846ecd889200") },
			RoomId = Guid.Parse("ceecfa8d-d91f-4b37-8833-846ecd889200")
		};

		await VerifyFunctionExecutionAsync(c => c.DispatchAsync<AllocateDesksCommand, bool>(It.IsAny<AllocateDesksCommand>(), default), "PUT",
			"AllocateDesks",
			body: new MemoryStream(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(dto))));
	}

	[Test]
	public async Task ShouldCallAddDesksCommand()
	{
		var dto = new AddDesksDto
		{
			FirstDeskNumber = 1,
			NumberOfDesks = 1,
			RoomId = Guid.Parse("ceecfa8d-d91f-4b37-8833-846ecd889200")
		};
		await VerifyFunctionExecutionAsync(c => c.DispatchAsync<AddDesksCommand, bool>(It.IsAny<AddDesksCommand>(), default), "POST",
			"AddDesks",
			body: new MemoryStream(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(dto))));
	}

	[Test]
	public async Task ShouldCallToggleDesksIsEnabled()
	{
		var dto = new ToggleDeskIsEnabledDto
		{
			DesksIds = new List<Guid> { Guid.Parse("ceecfa8d-d91f-4b37-8833-846ecd889200") }
		};

		await VerifyFunctionExecutionAsync(c => c.DispatchAsync<ToggleDeskIsEnabledCommand>(It.IsAny<ToggleDeskIsEnabledCommand>(), default), "PUT",
			"ToggleDesksIsEnabled",
			body: new MemoryStream(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(dto))));
	}

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
		var function = new RoomFunction(_dispatcherMock.Object, _entityQueriesMock.Object);
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
