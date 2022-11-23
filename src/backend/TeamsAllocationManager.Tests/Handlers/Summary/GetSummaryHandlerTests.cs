using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TeamsAllocationManager.Contracts.Summary.Queries;
using TeamsAllocationManager.Database;
using TeamsAllocationManager.Domain.Enums;
using TeamsAllocationManager.Domain.Models;
using TeamsAllocationManager.Dtos.Summary;
using TeamsAllocationManager.Infrastructure.Handlers.Summary;
using TeamsAllocationManager.Tests.Helpers;

namespace TeamsAllocationManager.Tests.Handlers.Summary;

[TestFixture]
public class GetSummaryHandlerTests
{
	private readonly ApplicationDbContext _context;
	public GetSummaryHandlerTests()
	{
		_context = TestsHelpers.CreateDbContextInMemory(this);
	}

	[SetUp]
	public void SetupBeforeEachTest()
	{
		_context.ClearDatabase();
	}

	[Test]
	public async Task ExecuteAsync_ExampleDatabase_ReturnsProperCounts()
	{
		// given
		var teamLeader1 = new EmployeeEntity { Name = "Jan", Surname = "Kowalski", Email = "jkowalski@fp.pl", ExternalId = 1, UserLogin = "jkowalski", WorkspaceType = WorkspaceType.Office };
		var employee1 = new EmployeeEntity { Name = "Katarzyna", Surname = "Nowak", Email = "knowak@fp.pl", ExternalId = 2, UserLogin = "knowak", WorkspaceType = WorkspaceType.Office };
		var employee2 = new EmployeeEntity { Name = "Krystian", Surname = "Wiadro", Email = "kwiadro@fp.pl", ExternalId = 3, UserLogin = "kwiadro", WorkspaceType = WorkspaceType.Hybrid };
		var employee3 = new EmployeeEntity { Name = "Alina", Surname = "Wêgrzyk", Email = "awegrzyk@fp.pl", ExternalId = 4, UserLogin = "awegrzyk", WorkspaceType = WorkspaceType.Remote };
		var employee4 = new EmployeeEntity { Name = "Ryszard", Surname = "Ochucki", Email = "rochucki@fp.pl", ExternalId = 5, UserLogin = "rochucki", WorkspaceType = WorkspaceType.Remote };
		var employee5 = new EmployeeEntity { Name = "Adam", Surname = "Ma³ysz", Email = "amalysz@fp.pl", ExternalId = 6, UserLogin = "amalysz", WorkspaceType = WorkspaceType.Office };
		var employee6 = new EmployeeEntity { Name = "Krystyna", Surname = "Kowalik", Email = "kkowalik@fp.pl", ExternalId = 7, UserLogin = "kkowalik", WorkspaceType = WorkspaceType.Office };
		
		var employee7 = new EmployeeEntity { Name = "Katarzyna", Surname = "Nowak4", Email = "knowak4@fp.pl", ExternalId = 8, UserLogin = "knowak4", WorkspaceType = WorkspaceType.Office, IsContractor = true };
		var employee8 = new EmployeeEntity { Name = "Krystian", Surname = "Wiadro4", Email = "kwiadro4@fp.pl", ExternalId = 9, UserLogin = "kwiadro4", WorkspaceType = WorkspaceType.Hybrid, IsContractor = true };
		var employee9 = new EmployeeEntity { Name = "Alina", Surname = "Wêgrzyk4", Email = "awegrzyk4@fp.pl", ExternalId = 10, UserLogin = "awegrzyk4", WorkspaceType = WorkspaceType.Remote, IsContractor = true };
		var employee10 = new EmployeeEntity { Name = "Ryszard", Surname = "Ochucki4", Email = "rochucki4@fp.pl", ExternalId = 11, UserLogin = "rochucki4", WorkspaceType = WorkspaceType.Remote, IsContractor = true };
		var employee11 = new EmployeeEntity { Name = "Adam", Surname = "Ma³ysz4", Email = "amalysz4@fp.pl", ExternalId = 12, UserLogin = "amalysz4", WorkspaceType = WorkspaceType.Office, IsContractor = true };
		var employee12 = new EmployeeEntity { Name = "Adam2", Surname = "Ma³ysz24", Email = "amalysz24@fp.pl", ExternalId = 13, UserLogin = "amalysz24", WorkspaceType = null, IsContractor = true };

		var project1 = new ProjectEntity
		{
			Name = "Project 1", EndDate = DateTime.Now.AddDays(100), ExternalId = 1,
			Employees = new List<EmployeeProjectEntity> {
				new EmployeeProjectEntity { Employee = teamLeader1, IsTeamLeaderProjectRole = true },
				new EmployeeProjectEntity { Employee = employee1 },
				new EmployeeProjectEntity { Employee = employee2 },
				new EmployeeProjectEntity { Employee = employee5 },
				new EmployeeProjectEntity { Employee = employee6 },
				new EmployeeProjectEntity { Employee = employee7 },
				new EmployeeProjectEntity { Employee = employee8 }
			}
		};
		_context.Projects.Add(project1);

		var project2 = new ProjectEntity
		{
			Name = "Project 2", EndDate = DateTime.Now.AddDays(100), ExternalId = 2,
			Employees = new List<EmployeeProjectEntity> {
				new EmployeeProjectEntity { Employee = teamLeader1, IsTeamLeaderProjectRole = true },
				new EmployeeProjectEntity { Employee = employee3, IsTeamLeaderProjectRole = true },
				new EmployeeProjectEntity { Employee = employee4, IsTeamLeaderProjectRole = true }
			}
		};
		_context.Projects.Add(project2);

		var project3 = new ProjectEntity
		{
			Name = "Project 3", EndDate = DateTime.Now.AddDays(100), ExternalId = 3,
			Employees = new List<EmployeeProjectEntity> {
				new EmployeeProjectEntity { Employee = teamLeader1, IsTeamLeaderProjectRole = true },
				new EmployeeProjectEntity { Employee = employee7 },
				new EmployeeProjectEntity { Employee = employee8 },
				new EmployeeProjectEntity { Employee = employee9 },
				new EmployeeProjectEntity { Employee = employee10 },
				new EmployeeProjectEntity { Employee = employee11 },
				new EmployeeProjectEntity { Employee = employee12 }
			}
		};
		_context.Projects.Add(project3);

		var building1 = new BuildingEntity { Name = "F1" };
		var floor1 = new FloorEntity { Building = building1, FloorNumber = 0 };
		var room1 = new RoomEntity { Area = 22.5m, Name = "001", Floor = floor1 };
		var floor2 = new FloorEntity { Building = building1, FloorNumber = 0 };
		var room2 = new RoomEntity { Area = 26.5m, Name = "002", Floor = floor2 };
		var floor3 = new FloorEntity { Building = building1, FloorNumber = 0 };
		var room3 = new RoomEntity { Area = 12.5m, Name = "003", Floor = floor3 };
		_context.Desks.AddRange(
			new DeskEntity { Room = room1, Number = 1 },
			new DeskEntity { Room = room1, Number = 2 },
			new DeskEntity { Room = room2, Number = 3 },
			new DeskEntity { Room = room1, Number = 4 },
			new DeskEntity { Room = room2, Number = 5 },
			new DeskEntity { Room = room1, Number = 6 },
			new DeskEntity { Room = room3, Number = 7 },
			new DeskEntity { Room = room3, Number = 8 },
			new DeskEntity { Room = room3, Number = 10 },
			new DeskEntity { Room = room3, Number = 11 },
			new DeskEntity { Room = room3, Number = 12 }
			);

		_context.SaveChanges();

		var query = new GetSummaryQuery();
		var handler = new GetSummaryHandler(_context);

		// when
		SummaryDto result = (await handler.HandleAsync(query))!;

		// then
		var employees = await _context.Employees.Include(e => e.EmployeeDeskReservations).ThenInclude(dr => dr.Desk).ToListAsync();
		var projects = await _context.Projects.ToListAsync();
			
		Assert.AreEqual(Dtos.Enums.ErrorCodes.NoError, result.ErrorCode);
		Assert.AreEqual(projects.Count(), result.ProjectsCount);

		// Desks 
		var desks = await _context.Desks.ToListAsync();
		Assert.AreEqual(desks.Count(), result.DesksCount);
		Assert.AreEqual(desks.Count(d => d.IsHotDesk == true), result.HotDesksCount);

		// All employees
		Assert.AreEqual(employees.Count(), result.AllEmployeesCount);
		Assert.AreEqual(employees.Count(e => e.WorkspaceType == WorkspaceType.Office), result.AllOfficeEmployeesCount);
		Assert.AreEqual(employees.Count(e => e.WorkspaceType == WorkspaceType.Office && e.EmployeeDeskReservations.Any(dr => dr.IsSchedule)), result.AllAssignedOfficeEmployeesCount);
		Assert.AreEqual(employees.Count(e => e.WorkspaceType == WorkspaceType.Office && !e.EmployeeDeskReservations.Any(dr => dr.IsSchedule)), result.AllUnassignedOfficeEmployeesCount);
		Assert.AreEqual(employees.Count(e => e.WorkspaceType == WorkspaceType.Hybrid), result.AllHybridEmployeesCount);
		Assert.AreEqual(employees.Count(e => e.WorkspaceType == WorkspaceType.Hybrid && e.EmployeeDeskReservations.Any(dr => dr.IsSchedule)), result.AllAssignedHybridEmployeesCount);
		Assert.AreEqual(employees.Count(e => e.WorkspaceType == WorkspaceType.Hybrid && !e.EmployeeDeskReservations.Any(dr => dr.IsSchedule)), result.AllUnassignedHybridEmployeesCount);
		Assert.AreEqual(employees.Count(e => e.WorkspaceType == WorkspaceType.Remote), result.AllRemoteEmployeesCount);
		Assert.AreEqual(employees.Count(e => e.WorkspaceType != WorkspaceType.Remote && e.WorkspaceType != null && e.EmployeeDeskReservations.Where(dr => dr.IsSchedule).Count() != 0), result.AllAssignedToDesksCount);
		Assert.AreEqual(employees.Count(e => e.WorkspaceType != WorkspaceType.Remote && e.WorkspaceType != null && e.EmployeeDeskReservations.Where(dr => dr.IsSchedule).Count() == 0), result.AllUnassignedToDesksCount);
		Assert.AreEqual(employees.Count(e => e.WorkspaceType == null), result.AllNotSetEmployeesCount);

		// FP
		Assert.AreEqual(employees.Count(e => e.IsContractor == false), result.FpEmployeesCount);
		Assert.AreEqual(employees.Count(e => e.WorkspaceType == WorkspaceType.Office && e.IsContractor == false), result.FpOfficeEmployeesCount);
		Assert.AreEqual(employees.Count(e => e.WorkspaceType == WorkspaceType.Office && e.IsContractor == false && e.EmployeeDeskReservations.Any(dr => dr.IsSchedule)), result.FpAssignedOfficeEmployeesCount);
		Assert.AreEqual(employees.Count(e => e.WorkspaceType == WorkspaceType.Office && e.IsContractor == false && !e.EmployeeDeskReservations.Any(dr => dr.IsSchedule)), result.FpUnassignedOfficeEmployeesCount);
		Assert.AreEqual(employees.Count(e => e.WorkspaceType == WorkspaceType.Hybrid && e.IsContractor == false), result.FpHybridEmployeesCount);
		Assert.AreEqual(employees.Count(e => e.WorkspaceType == WorkspaceType.Hybrid && e.IsContractor == false && e.EmployeeDeskReservations.Any(dr => dr.IsSchedule)), result.FpAssignedHybridEmployeesCount);
		Assert.AreEqual(employees.Count(e => e.WorkspaceType == WorkspaceType.Hybrid && e.IsContractor == false && !e.EmployeeDeskReservations.Any(dr => dr.IsSchedule)), result.FpUnassignedHybridEmployeesCount);
		Assert.AreEqual(employees.Count(e => e.WorkspaceType == WorkspaceType.Remote && e.IsContractor == false), result.FpRemoteEmployeesCount);
		Assert.AreEqual(employees.Count(e => e.WorkspaceType != WorkspaceType.Remote && e.WorkspaceType != null && e.EmployeeDeskReservations.Where(dr => dr.IsSchedule).Count() != 0 && e.IsContractor == false), result.FpAssignedToDesksCount);
		Assert.AreEqual(employees.Count(e => e.WorkspaceType != WorkspaceType.Remote && e.WorkspaceType != null && e.EmployeeDeskReservations.Where(dr => dr.IsSchedule).Count() == 0 && e.IsContractor == false), result.FpUnassignedToDesksCount);
		Assert.AreEqual(employees.Count(e => e.WorkspaceType == null && e.IsContractor == false), result.FpNotSetEmployeesCount);

		// Contractor
		Assert.AreEqual(employees.Count(e => e.IsContractor), result.ContractorEmployeesCount);
		Assert.AreEqual(employees.Count(e => e.WorkspaceType == WorkspaceType.Office && e.IsContractor), result.ContractorOfficeEmployeesCount);
		Assert.AreEqual(employees.Count(e => e.WorkspaceType == WorkspaceType.Office && e.IsContractor && e.EmployeeDeskReservations.Any(dr => dr.IsSchedule)), result.ContractorAssignedOfficeEmployeesCount);
		Assert.AreEqual(employees.Count(e => e.WorkspaceType == WorkspaceType.Office && e.IsContractor && !e.EmployeeDeskReservations.Any(dr => dr.IsSchedule)), result.ContractorUnassignedOfficeEmployeesCount);
		Assert.AreEqual(employees.Count(e => e.WorkspaceType == WorkspaceType.Hybrid && e.IsContractor), result.ContractorHybridEmployeesCount);
		Assert.AreEqual(employees.Count(e => e.WorkspaceType == WorkspaceType.Hybrid && e.IsContractor && e.EmployeeDeskReservations.Any(dr => dr.IsSchedule)), result.ContractorAssignedHybridEmployeesCount);
		Assert.AreEqual(employees.Count(e => e.WorkspaceType == WorkspaceType.Hybrid && e.IsContractor && !e.EmployeeDeskReservations.Any(dr => dr.IsSchedule)), result.ContractorUnassignedHybridEmployeesCount);
		Assert.AreEqual(employees.Count(e => e.WorkspaceType == WorkspaceType.Remote && e.IsContractor), result.ContractorRemoteEmployeesCount);
		Assert.AreEqual(employees.Count(e => e.WorkspaceType != WorkspaceType.Remote && e.WorkspaceType != null && e.EmployeeDeskReservations.Where(dr => dr.IsSchedule).Count() != 0 && e.IsContractor), result.ContractorAssignedToDesksCount);
		Assert.AreEqual(employees.Count(e => e.WorkspaceType != WorkspaceType.Remote && e.WorkspaceType != null && e.EmployeeDeskReservations.Where(dr => dr.IsSchedule).Count() == 0 && e.IsContractor), result.ContractorUnassignedToDesksCount);
		Assert.AreEqual(employees.Count(e => e.WorkspaceType == null && e.IsContractor), result.ContractorNotSetEmployeesCount);
	}
}
