using System;
using TeamsAllocationManager.Contracts.Base.Queries;
using TeamsAllocationManager.Dtos.Employee;

namespace TeamsAllocationManager.Contracts.Employee.Queries;

public class GetUserDetailsQuery : IQuery<UserDetailsDto>
{
	public Guid Id { get; }

	public GetUserDetailsQuery(Guid id)
	{
		Id = id;
	}
}
