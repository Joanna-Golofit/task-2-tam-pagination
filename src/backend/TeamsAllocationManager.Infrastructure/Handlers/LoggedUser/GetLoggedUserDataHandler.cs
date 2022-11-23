using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using TeamsAllocationManager.Contracts.Base.Queries;
using TeamsAllocationManager.Contracts.LoggedUser.Queries;
using TeamsAllocationManager.Database.Repositories.Interfaces;
using TeamsAllocationManager.Domain.Models;
using TeamsAllocationManager.Dtos.Employee;
using TeamsAllocationManager.Infrastructure.Exceptions;

namespace TeamsAllocationManager.Infrastructure.Handlers.LoggedUser;

public class GetLoggedUserDataHandler : IAsyncQueryHandler<GetLoggedUserDataQuery, LoggedUserDataDto>
{
	private readonly IEmployeesRepository _employeesRepository;
	private readonly IMapper _mapper;

	public GetLoggedUserDataHandler(
		IEmployeesRepository employeesRepository, 
		IMapper mapper)
	{
		_employeesRepository = employeesRepository;
		_mapper = mapper;
	}

	public async Task<LoggedUserDataDto> HandleAsync(GetLoggedUserDataQuery query, CancellationToken cancellationToken = default)
	{
		if (string.IsNullOrWhiteSpace(query.CurrentUsername)
			|| query.CurrentUsername.Equals(query.LoggedUserEmail, StringComparison.OrdinalIgnoreCase) == false)
		{
			// TODO: temporary commented to not block functionality
			// throw new UnauthorizedAccessException("Requested user does not match the logged in one (or is empty) ");
		}

		var loggedEmployee = await _employeesRepository.GetLoggedEmployee(query.CurrentUsername, query.LoggedUserEmail);

		if (loggedEmployee == null)
		{
			throw new EntityNotFoundException<EmployeeEntity>(nameof(EmployeeEntity.Email), (query.LoggedUserEmail ?? query.CurrentUsername));
		}

		return _mapper.Map<LoggedUserDataDto>(loggedEmployee);
	}
}
