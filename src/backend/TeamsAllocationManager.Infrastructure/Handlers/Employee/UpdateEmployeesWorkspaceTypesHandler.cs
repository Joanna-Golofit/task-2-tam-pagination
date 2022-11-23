using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TeamsAllocationManager.Contracts.Base.Commands;
using TeamsAllocationManager.Contracts.Employee.Commands;
using TeamsAllocationManager.Database.Repositories.Interfaces;
using TeamsAllocationManager.Domain.Models;
using TeamsAllocationManager.Dtos.Employee;
using TeamsAllocationManager.Dtos.Enums;
using TeamsAllocationManager.Infrastructure.Exceptions;

namespace TeamsAllocationManager.Infrastructure.Handlers.Employee;

public class UpdateEmployeesWorkspaceTypesHandler : IAsyncCommandHandler<UpdateEmployeesWorkspaceTypesCommand, bool>
{
	private readonly IEmployeesRepository _employeesRepository;
	private readonly IEmployeeWorkingTypeHistoryRepository _employeeWorkingTypeHistoryRepository;
	private readonly IDesksRepository _desksRepository;
	private readonly IMapper _mapper;

	public UpdateEmployeesWorkspaceTypesHandler(IEmployeesRepository employeesRepository,
		IEmployeeWorkingTypeHistoryRepository employeeWorkingTypeHistoryRepository,
		IDesksRepository desksRepository,
		IMapper mapper)
	{
		_employeesRepository = employeesRepository;
		_employeeWorkingTypeHistoryRepository = employeeWorkingTypeHistoryRepository;
		_desksRepository = desksRepository;
		_mapper = mapper;
	}

	public async Task<bool> HandleAsync(UpdateEmployeesWorkspaceTypesCommand command, CancellationToken cancellationToken = default)
	{
		var dtos = command.Dtos;

		ValidateWorkspaceTypes(dtos);

		var ids = dtos.Select(dto => dto.EmployeeId).ToList();

		var employees = await _employeesRepository.GetEmployees(ids);

		if (!employees.Any())
		{
			return false;
		}

		var employeesDeskIds = employees.SelectMany(e => e.EmployeeDeskReservations.Where(edr => edr.IsSchedule).Select(edr => edr.DeskId));
		var employeesDesks = await _desksRepository.GetDesks(employeesDeskIds);

		var employeeWorkingTypeHistoryEntityList = new List<EmployeeWorkingTypeHistoryEntity>();

		foreach (var dto in dtos)
		{
			EmployeeEntity employee = employees.Single(e => e.Id == dto.EmployeeId);

			employee.WorkspaceType = _mapper.Map<Domain.Enums.WorkspaceType>(dto.WorkspaceType);

			var employeeWorkingTypeHistoryList = await _employeeWorkingTypeHistoryRepository.GetEmployeeWorkingTypeHistoryForEmployee(dto.EmployeeId);

			if (employeeWorkingTypeHistoryList.Any())
			{
				EmployeeWorkingTypeHistoryEntity? employeeWorkingTypeHistoryElement = employeeWorkingTypeHistoryList
					.OrderByDescending(ewth => ewth.Created)
					.FirstOrDefault();

				employeeWorkingTypeHistoryElement!.To = DateTime.Now;
			}

			employeeWorkingTypeHistoryEntityList.Add(new EmployeeWorkingTypeHistoryEntity
			{
				EmployeeId = dto.EmployeeId,
				WorkspaceType = _mapper.Map<Domain.Enums.WorkspaceType>(dto.WorkspaceType),
				From = DateTime.Now
			});

			if (dto.WorkspaceType == Dtos.Enums.WorkspaceType.Remote)
			{
				var employeeDesks = employeesDesks.Where(d => d.DeskReservations.Any(dr => dr.EmployeeId == employee.Id)).ToList();

				employeeDesks.ForEach(ed => ed.ReleaseDesk(employee.Id));
			}
		}

		await _employeeWorkingTypeHistoryRepository.AddRangeAsync(employeeWorkingTypeHistoryEntityList);

		await _desksRepository.UpdateRangeAsync(employeesDesks);

		return true;
	}

	private static void ValidateWorkspaceTypes(IEnumerable<UpdateEmployeeWorkspaceTypeDto> dtos)
	{
		var workspaceTypes = dtos.Select(d => d.WorkspaceType).ToList();

		var allWorkspaceTypesInEnum =
			workspaceTypes.All(workspaceType => Enum.IsDefined(typeof(WorkspaceType), workspaceType!));

		if (!allWorkspaceTypesInEnum)
		{
			throw new InvalidArgumentException(ExceptionMessage.GetMessage(ExceptionMessage.InvalidArgument_InvalidWorkspaceType))
			{
				TranslationKey = ExceptionMessage.InvalidArgument_InvalidWorkspaceType
			};
		}
	}
}
