using System;

namespace TeamsAllocationManager.Dtos.Employee;

public class EmployeeBasicInfoDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public string Surname { get; set; } = null!;
    public string Email { get; set; } = null!;
}
