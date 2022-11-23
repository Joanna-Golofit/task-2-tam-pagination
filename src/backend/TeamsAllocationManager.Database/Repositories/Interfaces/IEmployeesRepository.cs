using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TeamsAllocationManager.Database.Repositories.GenericRepository;
using TeamsAllocationManager.Domain.Models;

namespace TeamsAllocationManager.Database.Repositories.Interfaces;

public interface IEmployeesRepository: IAsyncRepository<EmployeeEntity>
{
	Task<EmployeeEntity?> GetEmployee(string email);
	Task<EmployeeEntity?> GetEmployee(Guid id);
	Task<EmployeeEntity?> GetEmployeeWithReservations(string email);
	Task<EmployeeEntity?> GetEmployeeWithReservations(Guid id);
	Task<IEnumerable<EmployeeEntity>> GetEmployees(IEnumerable<Guid> employeesIds);
	Task<EmployeeEntity?> GetExternalEmployee(Guid employeeId);
	Task<IEnumerable<EmployeeEntity>> GetEmployeesForProject(Guid projectId);
	Task<IEnumerable<EmployeeEntity>> GetEmployeesForSearcher();
	Task<IEnumerable<EmployeeEntity>> GetTeamLeaders();
	Task<EmployeeEntity?> GetEmployeeWithDetails(Guid employeeId);
	Task<bool> IsUserAdmin(string userName);
	Task<EmployeeEntity?> GetLoggedEmployee(string? name, string? email);
	Task<IEnumerable<string>> GetUserRoleNames(string userName);
}
