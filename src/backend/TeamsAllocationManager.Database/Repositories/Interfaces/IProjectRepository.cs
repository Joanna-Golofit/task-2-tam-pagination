using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TeamsAllocationManager.Database.Repositories.GenericRepository;
using TeamsAllocationManager.Domain.Models;

namespace TeamsAllocationManager.Database.Repositories.Interfaces;

public interface IProjectRepository : IAsyncRepository<ProjectEntity>
{
	Task<ProjectEntity?> GetProject(Guid companyId);
	Task<ProjectEntity?> GetExternalProject(Guid companyId);
	Task<ProjectEntity?> GetExternalProjectWithDesks(Guid companyId);
	Task<IEnumerable<ProjectEntity>> GetProjectsForTeamLeaders();
	Task<IEnumerable<ProjectEntity>> GetProjectsWithEmployeesAssigned();
	Task<ProjectEntity?> GetProjectsWithDetails(Guid projectId);
}