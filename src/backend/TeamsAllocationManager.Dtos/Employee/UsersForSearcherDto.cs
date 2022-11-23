using System;
using System.Collections.Generic;
using System.Text;

namespace TeamsAllocationManager.Dtos.Employee;

public class UsersForSearcherDto
{
	public List<UserForSearcherDto> Users { get; set; } = new List<UserForSearcherDto>();
}
