using System.Collections.Generic;
using TeamsAllocationManager.Contracts.Base.Queries;
using TeamsAllocationManager.Dtos.Employee;

namespace TeamsAllocationManager.Contracts.Employee.Queries;

public class GetTeamLeadersQuery : IQuery<IEnumerable<TeamLeaderDto>>
{
}
