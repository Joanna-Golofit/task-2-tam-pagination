using System;
using System.Collections.Generic;
using TeamsAllocationManager.Contracts.Base.Queries;
using TeamsAllocationManager.Dtos.Room;

namespace TeamsAllocationManager.Contracts.Room.Queries;

public class GetEmployeesForRoomDetailsQuery : IQuery<IEnumerable<EmployeeForRoomDetailsDto>>
{
	public Guid ProjectId { get; set; }
	public Guid RoomId { get; set; }

	public GetEmployeesForRoomDetailsQuery(Guid projectId, Guid roomId)
	{
		ProjectId = projectId;
		RoomId = roomId;
	}
}
