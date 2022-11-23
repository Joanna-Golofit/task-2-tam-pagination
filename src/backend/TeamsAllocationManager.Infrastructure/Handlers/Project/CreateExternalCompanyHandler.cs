using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using TeamsAllocationManager.Contracts.Base.Commands;
using TeamsAllocationManager.Contracts.Project.Commands;
using TeamsAllocationManager.Database;
using TeamsAllocationManager.Database.Repositories.Interfaces;
using TeamsAllocationManager.Domain.Models;
using TeamsAllocationManager.Infrastructure.Exceptions;

namespace TeamsAllocationManager.Infrastructure.Handlers.Project;

public class CreateExternalCompanyHandler : IAsyncCommandHandler<CreateExternalCompanyCommand, Guid>
{
	private readonly ApplicationDbContext _applicationDbContext;
	private readonly IProjectRepository _projectRepository;

	public CreateExternalCompanyHandler(ApplicationDbContext applicationDbContext, IProjectRepository projectRepository)
	{
		_applicationDbContext = applicationDbContext;
		_projectRepository = projectRepository;
	}

	public async Task<Guid> HandleAsync(CreateExternalCompanyCommand command, CancellationToken cancellationToken = default)
	{
		if (_applicationDbContext.Projects.Any(p => p.Name == command.Dto.Name))
		{
			throw new ExternalCompanyEntityDuplicateException(nameof(command.Dto.Name));
		}

		if (_applicationDbContext.Projects.Any(p => p.Email == command.Dto.Email))
		{
			throw new ExternalCompanyEntityDuplicateException(nameof(command.Dto.Email));
		}

		var newCompany = new ProjectEntity
		{
			Name = command.Dto.Name,
			Email = command.Dto.Email,
			IsExternal = true,
			Employees = new List<EmployeeProjectEntity>()
		};

		for (int i = 1; i <= command.Dto.InitialEmployeeCount; i++)
		{
			newCompany.Employees.Add(EmployeeProjectEntity.AddNewExternalEmployeeToProject(newCompany, i, false));
		}

		await _projectRepository.AddAsync(newCompany);

		return newCompany.Id;
	}
}
