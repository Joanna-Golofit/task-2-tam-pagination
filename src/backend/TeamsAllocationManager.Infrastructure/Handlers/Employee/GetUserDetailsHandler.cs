using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using TeamsAllocationManager.Contracts.Base.Queries;
using TeamsAllocationManager.Contracts.Employee.Queries;
using TeamsAllocationManager.Database.Repositories.Interfaces;
using TeamsAllocationManager.Domain.Models;
using TeamsAllocationManager.Dtos.Employee;
using TeamsAllocationManager.Infrastructure.Exceptions;

namespace TeamsAllocationManager.Infrastructure.Handlers.Employee;

public class GetUserDetailsHandler : IAsyncQueryHandler<GetUserDetailsQuery, UserDetailsDto>
{
	private readonly IEmployeesRepository _employeesRepository;
	private readonly IMapper _mapper;

	public GetUserDetailsHandler(IEmployeesRepository employeesRepository, IMapper mapper)
	{
		_employeesRepository = employeesRepository;
		_mapper = mapper;
	}

	public async Task<UserDetailsDto> HandleAsync(GetUserDetailsQuery query, CancellationToken cancellationToken = default)
	{
		EmployeeEntity? user = await _employeesRepository.GetEmployeeWithDetails(query.Id);

		if (user == null)
		{
			throw new EntityNotFoundException<EmployeeEntity>(query.Id);
		}
			
		return _mapper.Map<UserDetailsDto>(user);
	}
}
