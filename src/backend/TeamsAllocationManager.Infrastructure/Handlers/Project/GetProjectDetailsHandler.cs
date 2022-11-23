using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TeamsAllocationManager.Contracts.Base.Queries;
using TeamsAllocationManager.Contracts.Project.Queries;
using TeamsAllocationManager.Database.Repositories.Interfaces;
using TeamsAllocationManager.Domain.Models;
using TeamsAllocationManager.Dtos.Project;
using TeamsAllocationManager.Dtos.Room;
using TeamsAllocationManager.Infrastructure.Exceptions;
using TeamsAllocationManager.Infrastructure.Extensions;

namespace TeamsAllocationManager.Infrastructure.Handlers.Project;

public class GetProjectDetailsHandler : IAsyncQueryHandler<GetProjectDetailsQuery, ProjectDetailsDto>
{
	private readonly IProjectRepository _projectRepository;
	private readonly IRoomRepository _roomRepository;
	private readonly IMapper _mapper;

	public GetProjectDetailsHandler(IProjectRepository projectRepository, IRoomRepository roomRepository, IMapper mapper)
	{
		_projectRepository = projectRepository;
		_roomRepository = roomRepository;
		_mapper = mapper;
	}
		
	public async Task<ProjectDetailsDto> HandleAsync(GetProjectDetailsQuery query, CancellationToken cancellationToken = default)
	{
		ProjectEntity? result = await _projectRepository.GetProjectsWithDetails(query.Id);
			
		if (result == null)
		{
			throw new EntityNotFoundException<ProjectEntity>(query.Id);
		}

		ProjectDetailsDto project = _mapper.Map<ProjectDetailsDto>(result);
			
		if (project.Employees.Any())
		{
			project.Employees = result.IsExternal && project.Employees.All(e => int.TryParse(e.Surname, out var _))
								? project.Employees.OrderBy(e => int.Parse(e.Surname))
								: project.Employees.OrderBy(e => e.Surname);
		}

		if (project.TeamLeaders.Count > 0)
		{
			project.TeamLeaders = result.IsExternal && project.TeamLeaders.All(tl => int.TryParse(tl.Surname, out var _))
									? project.TeamLeaders.OrderBy(e => int.Parse(e.Surname)).ToList()
									: project.TeamLeaders.OrderBy(e => e.Surname).ToList();
		}
			
		var roomIds = result.Employees
			        .SelectMany(ep => ep.Employee.EmployeeDeskReservations
						.Where(edr => edr.IsSchedule)
						.Select(edr => edr.Desk)
						.Select(d => d.Room))
			        .Select(r => r.Id)
			        .Distinct();

		var rooms = await _roomRepository.GetRoomsWithDetails(roomIds);

		foreach (RoomEntity room in rooms)
		{
			room.FilterLastNDaysInDeskHistory(7);
		}

		project.Rooms = _mapper.Map<IEnumerable<RoomForProjectDto>>(rooms).OrderBy(r => r.Name);

		foreach (RoomForProjectDto room in project.Rooms)
		{
			room.DesksInRoom = room.DesksInRoom.OrderBy(d => d.Number);
			room.SasTokenForRoomPlans = Helpers.GenerateSasTokenForRoomPlansContainer();
		}

		return project;
	}
}
