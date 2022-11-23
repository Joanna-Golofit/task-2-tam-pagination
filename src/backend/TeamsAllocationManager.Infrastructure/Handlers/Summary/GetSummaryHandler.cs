using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TeamsAllocationManager.Contracts.Base.Queries;
using TeamsAllocationManager.Contracts.Summary.Queries;
using TeamsAllocationManager.Database;
using TeamsAllocationManager.Domain.Enums;
using TeamsAllocationManager.Dtos.Summary;

namespace TeamsAllocationManager.Infrastructure.Handlers.Summary;

public class GetSummaryHandler : IAsyncQueryHandler<GetSummaryQuery, SummaryDto>
{
	private readonly ApplicationDbContext _applicationDbContext;

	public GetSummaryHandler(ApplicationDbContext applicationDbContext)
	{
		_applicationDbContext = applicationDbContext;
	}

	public async Task<SummaryDto> HandleAsync(GetSummaryQuery query, CancellationToken cancellationToken = default)
	{
		var employees = await _applicationDbContext.Employees
			.Include(e => e.EmployeeDeskReservations)
				.ThenInclude(dr => dr.Desk)
			.AsSplitQuery()
			.AsNoTracking()
			.ToListAsync();

		return new SummaryDto
		{
			ProjectsCount = _applicationDbContext.Projects.Count(),

			DesksCount = _applicationDbContext.Desks.Count(),
			FreeDesksCount = _applicationDbContext.Desks.Count(d => !d.DeskReservations.Any(dr => dr.IsSchedule) && !d.IsHotDesk && d.IsEnabled),
			OccupiedDesksCount = _applicationDbContext.Desks.Count(d => d.DeskReservations.Any(dr => dr.IsSchedule)),
			HotDesksCount = _applicationDbContext.Desks.Count(d => d.IsHotDesk),

			AllEmployeesCount = employees.Count(),
			AllHybridEmployeesCount = employees.Count(e => e.WorkspaceType == WorkspaceType.Hybrid),
			AllAssignedHybridEmployeesCount = employees.Count(e => e.WorkspaceType == WorkspaceType.Hybrid && e.EmployeeDeskReservations.Any(dr => dr.IsSchedule)),
			AllUnassignedHybridEmployeesCount = employees.Count(e => e.WorkspaceType == WorkspaceType.Hybrid && !e.EmployeeDeskReservations.Any(dr => dr.IsSchedule)),
			AllOfficeEmployeesCount = employees.Count(e => e.WorkspaceType == WorkspaceType.Office),
			AllAssignedOfficeEmployeesCount = employees.Count(e => e.WorkspaceType == WorkspaceType.Office && e.EmployeeDeskReservations.Any(dr => dr.IsSchedule)),
			AllUnassignedOfficeEmployeesCount = employees.Count(e => e.WorkspaceType == WorkspaceType.Office && !e.EmployeeDeskReservations.Any(dr => dr.IsSchedule)),
			AllRemoteEmployeesCount = employees.Count(e => e.WorkspaceType == WorkspaceType.Remote),
			AllNotSetEmployeesCount = employees.Count(e => e.WorkspaceType == null),
			AllAssignedToDesksCount = employees.Count(e => e.WorkspaceType != WorkspaceType.Remote
									&& e.WorkspaceType != null && e.EmployeeDeskReservations.Any(dr => dr.IsSchedule)),
			AllUnassignedToDesksCount = employees.Count(e => e.WorkspaceType != WorkspaceType.Remote
										&& e.WorkspaceType != null && !e.EmployeeDeskReservations.Any(dr => dr.IsSchedule)),

			FpEmployeesCount = employees.Count(e => e.IsContractor == false),
			FpHybridEmployeesCount = employees.Count(e => e.WorkspaceType == WorkspaceType.Hybrid && e.IsContractor == false),
			FpAssignedHybridEmployeesCount = employees.Count(e => e.WorkspaceType == WorkspaceType.Hybrid && e.IsContractor == false && e.EmployeeDeskReservations.Any(dr => dr.IsSchedule)),
			FpUnassignedHybridEmployeesCount = employees.Count(e => e.WorkspaceType == WorkspaceType.Hybrid && e.IsContractor == false && !e.EmployeeDeskReservations.Any(dr => dr.IsSchedule)),
			FpOfficeEmployeesCount = employees.Count(e => e.WorkspaceType == WorkspaceType.Office && e.IsContractor == false),
			FpAssignedOfficeEmployeesCount = employees.Count(e => e.WorkspaceType == WorkspaceType.Office && e.IsContractor == false && e.EmployeeDeskReservations.Any(dr => dr.IsSchedule)),
			FpUnassignedOfficeEmployeesCount = employees.Count(e => e.WorkspaceType == WorkspaceType.Office && e.IsContractor == false && !e.EmployeeDeskReservations.Any(dr => dr.IsSchedule)),
			FpRemoteEmployeesCount = employees.Count(e => e.WorkspaceType == WorkspaceType.Remote && e.IsContractor == false),
			FpNotSetEmployeesCount = employees.Count(e => e.WorkspaceType == null && e.IsContractor == false),
			FpAssignedToDesksCount = employees.Count(e => e.WorkspaceType != WorkspaceType.Remote
									&& e.WorkspaceType != null && e.EmployeeDeskReservations.Any(dr => dr.IsSchedule) && e.IsContractor == false),
			FpUnassignedToDesksCount = employees.Count(e => e.WorkspaceType != WorkspaceType.Remote
										&& e.WorkspaceType != null && !e.EmployeeDeskReservations.Any(dr => dr.IsSchedule) && e.IsContractor == false),

			ContractorEmployeesCount = employees.Count(e => e.IsContractor),
			ContractorHybridEmployeesCount = employees.Count(e => e.WorkspaceType == WorkspaceType.Hybrid && e.IsContractor),
			ContractorAssignedHybridEmployeesCount = employees.Count(e => e.WorkspaceType == WorkspaceType.Hybrid && e.IsContractor && e.EmployeeDeskReservations.Any(dr => dr.IsSchedule)),
			ContractorUnassignedHybridEmployeesCount = employees.Count(e => e.WorkspaceType == WorkspaceType.Hybrid && e.IsContractor && !e.EmployeeDeskReservations.Any(dr => dr.IsSchedule)),
			ContractorOfficeEmployeesCount = employees.Count(e => e.WorkspaceType == WorkspaceType.Office && e.IsContractor),
			ContractorAssignedOfficeEmployeesCount = employees.Count(e => e.WorkspaceType == WorkspaceType.Office && e.IsContractor && e.EmployeeDeskReservations.Any(dr => dr.IsSchedule)),
			ContractorUnassignedOfficeEmployeesCount = employees.Count(e => e.WorkspaceType == WorkspaceType.Office && e.IsContractor && !e.EmployeeDeskReservations.Any(dr => dr.IsSchedule)),
			ContractorRemoteEmployeesCount = employees.Count(e => e.WorkspaceType == WorkspaceType.Remote && e.IsContractor),
			ContractorNotSetEmployeesCount = employees.Count(e => e.WorkspaceType == null && e.IsContractor),
			ContractorAssignedToDesksCount = employees.Count(e => e.WorkspaceType != WorkspaceType.Remote
									&& e.WorkspaceType != null && e.EmployeeDeskReservations.Any(dr => dr.IsSchedule) && e.IsContractor),
			ContractorUnassignedToDesksCount = employees.Count(e => e.WorkspaceType != WorkspaceType.Remote
										&& e.WorkspaceType != null && !e.EmployeeDeskReservations.Any(dr => dr.IsSchedule) && e.IsContractor),

			ErrorCode = Dtos.Enums.ErrorCodes.NoError
		};
	}
}
