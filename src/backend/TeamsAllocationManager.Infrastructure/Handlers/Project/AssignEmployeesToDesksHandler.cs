using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TeamsAllocationManager.Contracts.Base.Commands;
using TeamsAllocationManager.Contracts.Project.Commands;
using TeamsAllocationManager.Database;
using TeamsAllocationManager.Database.Repositories.Interfaces;
using TeamsAllocationManager.Domain.Models;
using TeamsAllocationManager.Dtos.Project;
using TeamsAllocationManager.Infrastructure.Exceptions;

namespace TeamsAllocationManager.Infrastructure.Handlers.Project;

// TODO: remove when ReserveDesk command will be ready and in use
public class AssignEmployeesToDesksHandler : IAsyncCommandHandler<AssignEmployeesToDesksCommand>
{
	private readonly ApplicationDbContext _applicationDbContext;
	private readonly IDesksRepository _desksRepository;

	public AssignEmployeesToDesksHandler(
		ApplicationDbContext applicationDbContext,
		IDesksRepository desksRepository)
	{
		_applicationDbContext = applicationDbContext;
		_desksRepository = desksRepository;
	}

	public async Task HandleAsync(AssignEmployeesToDesksCommand command, CancellationToken cancellationToken = default)
	{
		AssignEmployeesToDesksDto dto = command.Dto;

		List<DeskEmployeeDto> desksEmployees = dto.DeskEmployeeDtos.ToList();

		var desks = await _desksRepository.GetDeskForRoom(dto.RoomId);

		var employees = await _applicationDbContext.Employees.Where(e => desksEmployees.Select(de => de.EmployeeId).Contains(e.Id)).ToListAsync();

		var desksToUpdate = new List<DeskEntity>();

		foreach (var deskEmployee in desksEmployees)
		{
			var desk = desks.SingleOrDefault(d => d.Id == deskEmployee.DeskId);
			if (desk == null)
			{
				throw new EntityNotFoundException<DeskEntity>(deskEmployee.DeskId);
			}

			var deskReservation = DeskReservationEntity.NewDeskReservation(
				DateTime.Now,
				new List<DayOfWeek> {
					DayOfWeek.Monday,
					DayOfWeek.Tuesday,
					DayOfWeek.Wednesday,
					DayOfWeek.Thursday,
					DayOfWeek.Friday,
				},
				desk, 
				employees.SingleOrDefault(e => e.Id == deskEmployee.EmployeeId));

			desk.ReserveDesk(deskReservation);

			desksToUpdate.Add(desk);
		}

		await _desksRepository.UpdateRangeAsync(desksToUpdate);
	}
}
