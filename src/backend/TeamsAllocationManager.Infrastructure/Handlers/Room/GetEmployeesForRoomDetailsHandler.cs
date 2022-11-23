using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TeamsAllocationManager.Contracts.Base.Queries;
using TeamsAllocationManager.Contracts.Room.Queries;
using TeamsAllocationManager.Database;
using TeamsAllocationManager.Domain.Enums;
using TeamsAllocationManager.Domain.Models;
using TeamsAllocationManager.Dtos.Desk;
using TeamsAllocationManager.Dtos.Room;

namespace TeamsAllocationManager.Infrastructure.Handlers.Room;

public class GetEmployeesForRoomDetailsHandler : IAsyncQueryHandler<GetEmployeesForRoomDetailsQuery, IEnumerable<EmployeeForRoomDetailsDto>>
{
	private readonly ApplicationDbContext _applicationDbContext;
	public GetEmployeesForRoomDetailsHandler(ApplicationDbContext applicationDbContext)
	{
		_applicationDbContext = applicationDbContext;
	}

	public async Task<IEnumerable<EmployeeForRoomDetailsDto>> HandleAsync(GetEmployeesForRoomDetailsQuery query, CancellationToken cancellationToken = default)
	{
		var employees = await _applicationDbContext.Employees.Where(e => e.WorkspaceType != WorkspaceType.Remote && e.WorkspaceType != null)
			.Where(e => e.Projects.Any(ep => ep.ProjectId == query.ProjectId))
			.Select(e => new EmployeeForRoomDetailsDto
			{
				Name = e.Name!,
				Surname = e.Surname!,
				Id = e.Id,
				ProjectsNames = e.Projects.Select(p => p.Project.Name),
			})
			.OrderBy(e => e.Surname)
			.ToListAsync();

		List<DeskEntity>? desksList = await _applicationDbContext.Desks
			.Include(d => d.DeskReservations)
				.ThenInclude(dr => dr.Employee)
			.Include(d => d.Room)
				.ThenInclude(r => r.Desks)
			.Include(d => d.Room)
				.ThenInclude(r => r.Floor)
				.ThenInclude(f => f.Building)
			.Where(d => d.DeskReservations.Any(dr => dr.IsSchedule))
			.AsSplitQuery()
			.ToListAsync();

		foreach (EmployeeForRoomDetailsDto employee in employees)
		{
			var desksIds = desksList.Where(d => d.DeskReservations.Any(dr => dr.EmployeeId == employee.Id))
				.Select(d => new RoomDeskDto { 
					DeskId = d.Id,
					DeskNumber = d.Number,
					RoomId = d.RoomId,
					RoomName = $"{d.Room.Floor.Building.Name} {d.Room.Name}" 
				}).ToList();

			employee.RoomDeskDtos = desksIds;
		}

		return employees;
	}
}
