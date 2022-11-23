using AutoMapper;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using TeamsAllocationManager.Database;
using TeamsAllocationManager.Domain.Enums;
using TeamsAllocationManager.Domain.Models;
using TeamsAllocationManager.Dtos.Building;
using TeamsAllocationManager.Dtos.Desk;
using TeamsAllocationManager.Dtos.Employee;
using TeamsAllocationManager.Dtos.Floor;
using TeamsAllocationManager.Dtos.Project;
using TeamsAllocationManager.Dtos.Room;
using TeamsAllocationManager.Mapper.Profiles;

namespace TeamsAllocationManager.Tests.Mapper;

[TestFixture]
public class MapperTests
{
	private readonly ApplicationDbContext _context;
	private readonly IMapper _mapper;

	public MapperTests()
	{
		DbContextOptions<ApplicationDbContext> options = new DbContextOptionsBuilder<ApplicationDbContext>()
			.UseInMemoryDatabase(databaseName: GetType().Name)
			.Options;
		_context = new ApplicationDbContext(options);
		_mapper = new MapperConfiguration(mc => mc.AddProfile(new EntityDtoProfile())).CreateMapper();
	}

	[SetUp]
	public void SetupBeforeEachTest()
	{
		_context.ClearDatabase();
	}

	[Test]
	//given
	public void Map_RoomEntity_RoomDto_EqualValues()
	{
		var entity1 = new RoomEntity
		{
			Id = Guid.NewGuid(),
			Floor = new FloorEntity
			{
				Building = new BuildingEntity(),
				FloorNumber = 5
			},
			Area = 51,
			Name = "Area",
			Desks = new List<DeskEntity>
			{
				new DeskEntity
				{
					Id = Guid.NewGuid(),
					Number = 102,
				}
			}
		};

		//when
		RoomDto? dtoMapped = _mapper.Map<RoomDto>(entity1);

		//then
		Assert.AreEqual(entity1.Id, dtoMapped.Id);
		Assert.IsNotNull(dtoMapped.Building);
		Assert.AreEqual(entity1.Area, dtoMapped.Area);
		Assert.AreEqual(entity1.Name, dtoMapped.Name);
		Assert.AreEqual(entity1.Floor.FloorNumber, dtoMapped.Floor);
		Assert.AreEqual(entity1.Desks.Count(), dtoMapped.Capacity);
	}

	[Test]
	public void Map_RoomEntity_RoomForProjectsDto_EqualValues()
	{
		//given
		var entity1 = new RoomEntity
		{
			Id = Guid.NewGuid(),
			Floor = new FloorEntity
			{
				FloorNumber = 5
			},
			Area = 51,
			Name = "Area"
		};

		//when
		RoomForProjectDto? dtoMapped = _mapper.Map<RoomForProjectDto>(entity1);

		//then
		Assert.AreEqual(entity1.Id, dtoMapped.Id);
		Assert.AreEqual(entity1.Floor.FloorNumber, dtoMapped.Floor);
		Assert.AreEqual(entity1.Area, dtoMapped.Area);
		Assert.AreEqual(entity1.Name, dtoMapped.Name);
	}

	[Test]
	public void Map_RoomEntity_RoomDetailsDto_EqualValues()
	{
		//given
		var entity1 = new RoomEntity
		{
			Id = Guid.NewGuid(),
			Desks = new List<DeskEntity>
			{
				new DeskEntity
				{
					Id = Guid.NewGuid(),
					Number = 102,
				}
			},
			Floor = new FloorEntity
			{
				Id = Guid.NewGuid(),
				FloorNumber = 5,
				Building = new BuildingEntity { Name = "F4" }
			},
			Area = 51,
			Name = "Area"
		};

		//when
		RoomDetailsDto dtoMapped = _mapper.Map<RoomDetailsDto>(entity1);

		//then
		Assert.AreEqual(entity1.Id, dtoMapped.Id);
		Assert.AreEqual(entity1.Floor.Building.Name, dtoMapped.Building.Name);
		Assert.AreEqual(entity1.Area, dtoMapped.Area);
		Assert.AreEqual(entity1.Name, dtoMapped.Name);
		Assert.AreEqual(entity1.Floor.FloorNumber, dtoMapped.Floor);
		Assert.AreEqual(entity1.Desks.Count(), dtoMapped.Capacity);
		Assert.AreEqual(default(decimal), dtoMapped.AreaMinLevelPerPerson);
		Assert.IsNull(dtoMapped.SasTokenForRoomPlans);
		Assert.IsNull(dtoMapped.ProjectsInRoom);
		Assert.IsNotNull(dtoMapped.DesksInRoom);
	}

	[Test]
	public void Map_FloorEntity_FloorDto_EqualValues()
	{
		//given
		var entity1 = new FloorEntity
		{
			Id = Guid.NewGuid(),
			FloorNumber = 5,
		};

		//when
		FloorDto? dtoMapped = _mapper.Map<FloorDto>(entity1);

		//then
		Assert.AreEqual(entity1.Id, dtoMapped.Id);
		Assert.AreEqual(entity1.FloorNumber, dtoMapped.Floor);
	}

	[Test]
	public void Map_BuildingEntity_BuildingDto_EqualValues()
	{
		//given
		var entity1 = new BuildingEntity
		{
			Id = Guid.NewGuid(),
			Name = "F4",
		};

		//when
		BuildingDto? dtoMapped = _mapper.Map<BuildingDto>(entity1);
		Assert.AreEqual(entity1.Id, dtoMapped.Id);
		Assert.AreEqual(entity1.Name, dtoMapped.Name);
	}

	[Test]
	public void Map_DeskEntity_DeskForProjectDetailsDto_EqualValues()
	{
		//given
		var entity1 = new DeskEntity
		{
			Id = Guid.NewGuid(),
			Number = 102,
		};

		//when
		DeskForProjectDetailsDto dtoMapped = _mapper.Map<DeskForProjectDetailsDto>(entity1);
		Assert.AreEqual(entity1.Id, dtoMapped.Id);
		Assert.AreEqual(entity1.Number, dtoMapped.Number);
	}

	[Test]
	public void Map_DeskEntity_DeskForRoomDetailsDto_EqualValues()
	{
		//given
		var entity1 = new DeskEntity
		{
			Id = Guid.NewGuid(),
			Number = 69,
		};

		//when
		DeskForRoomDetailsDto dtoMapped = _mapper.Map<DeskForRoomDetailsDto>(entity1);

		//then
		Assert.AreEqual(entity1.Id, dtoMapped.Id);
		Assert.AreEqual(entity1.Number, dtoMapped.Number);
	}

	[Test]
	public void Map_ProjectEntity_ProjectDto_EqualValues()
	{
		//given
		var project1 = new ProjectEntity { Id = Guid.NewGuid(), Name = "Project1", ExternalId = 0 };
		var project2 = new ProjectEntity { Id = Guid.NewGuid(), Name = "Project2", ExternalId = 1 };

		//when
		ProjectDto? dtoMapped1 = _mapper.Map<ProjectDto>(project1);
		ProjectDto? dtoMapped2 = _mapper.Map<ProjectDto>(project2);

		//then
		Assert.AreEqual(project1.Id, dtoMapped1.Id);
		Assert.AreEqual(project2.Id, dtoMapped2.Id);
	}

	[Test]
	public void Map_ProjectEntity_ProjectDetailsDto_EqualValues()
	{
		//given
		//Employees
		var employee2 = new EmployeeEntity { Name = "Adam", Surname = "Nowak", UserLogin = "anowak", WorkspaceType = WorkspaceType.Remote, ExternalId = 1 };
		var employeeTeamLeader1 = new EmployeeEntity { Name = "Zofia", Surname = "Na³kowska", UserLogin = "znalkowska", WorkspaceType = WorkspaceType.Hybrid, ExternalId = 2 };
		var employeeTeamLeader2 = new EmployeeEntity { Name = "Gra¿yna", Surname = "Wielkopolska", UserLogin = "gwielkopolska", WorkspaceType = WorkspaceType.Office, ExternalId = 3 };

		//Projects
		var project2 = new ProjectEntity { Id = Guid.NewGuid(), Name = "Project2", ExternalId = 1 };

		//EmployeeProjects
		var employeeProject2 = new EmployeeProjectEntity { Employee = employee2, Project = project2 };
		var employeeProject4 = new EmployeeProjectEntity { Employee = employeeTeamLeader2, Project = project2, IsTeamLeaderProjectRole = true };
		var employeeProject5 = new EmployeeProjectEntity { Employee = employeeTeamLeader1, Project = project2, IsTeamLeaderProjectRole = true };
		project2.Employees = new List<EmployeeProjectEntity> { employeeProject2, employeeProject4, employeeProject5 };

		//when
		ProjectDetailsDto dtoMapped = _mapper.Map<ProjectDetailsDto>(project2);
		var employees = dtoMapped.Employees.ToList();

		//then
		Assert.AreEqual(project2.Id, dtoMapped.Id);
		Assert.AreEqual(employee2.Name, employees[0].Name);
		Assert.AreEqual(employee2.Surname, employees[0].Surname);
		Assert.AreEqual(employeeTeamLeader2.Name, employees[1].Name);
		Assert.AreEqual(employeeTeamLeader2.Surname, employees[1].Surname);
		Assert.AreEqual(employeeTeamLeader1.Name, employees[2].Name);
		Assert.AreEqual(employeeTeamLeader1.Surname, employees[2].Surname);

		Assert.AreEqual(Dtos.Enums.WorkspaceType.Remote, employees[0].Workmode);
		Assert.AreEqual(Dtos.Enums.WorkspaceType.Office, employees[1].Workmode);
		Assert.AreEqual(Dtos.Enums.WorkspaceType.PartiallyRemote, employees[2].Workmode);
	}

	[Test]
	public void Map_ProjectEntity_ProjectForRoomDetailsDto_ReturnsTrue()
	{
		//given
		var project1 = new ProjectEntity
		{
			Id = Guid.NewGuid(),
			Name = "Project1",
			ExternalId = 0,
			Employees = new List<EmployeeProjectEntity>
			{
				new EmployeeProjectEntity
				{
					Employee = new EmployeeEntity
					{
						Id = Guid.NewGuid(),
						Name = "Jan",
						Surname = "Kowalski",
						UserLogin = "jkowalski",
						ExternalId = 0,
						Email = "jkowalski@future-processing.com"
					},
					IsTeamLeaderProjectRole = true
				}
			}
		};

		//when
		ProjectForRoomDetailsDto dtoMapped = _mapper.Map<ProjectForRoomDetailsDto>(project1);
		TeamLeaderDto? teamLeader = dtoMapped.TeamLeaders.First();
		EmployeeProjectEntity? employeeProject = project1.Employees.First();

		//then
		Assert.AreEqual(project1.Id, dtoMapped.Id);
		Assert.AreEqual(project1.Name, dtoMapped.Name);
		Assert.AreEqual(employeeProject.Employee.Id, teamLeader.Id);
		Assert.AreEqual(employeeProject.Employee.Name, teamLeader.Name);
		Assert.AreEqual(employeeProject.Employee.Surname, teamLeader.Surname);
		Assert.AreEqual(employeeProject.Employee.Email, teamLeader.Email);
	}

	[Test]
	public void Map_EmployeeProjectEntity_EmployeeProjectDetailsDto_EqualValues()
	{
		//given
		//Employees
		var employee1 = new EmployeeEntity { Name = "Zofia", Surname = "Na³kowska", UserLogin = "znalkowska", WorkspaceType = WorkspaceType.Hybrid, ExternalId = 2 };

		//Projects
		var project1 = new ProjectEntity { Name = "Project2", ExternalId = 1 };

		//EmployeeProjects
		var employeeProject1 = new EmployeeProjectEntity { Employee = employee1, Project = project1, IsTeamLeaderProjectRole = true };

		//when
		EmployeeForProjectDetailsDto dtoMapped = _mapper.Map<EmployeeForProjectDetailsDto>(employeeProject1);

		//then
		Assert.AreEqual(employee1.Name, dtoMapped.Name);
		Assert.AreEqual(employee1.Surname, dtoMapped.Surname);
		Assert.AreEqual(Dtos.Enums.WorkspaceType.PartiallyRemote, dtoMapped.Workmode);
		Assert.AreEqual(employee1.Id, dtoMapped.Id);
	}

	[Test]
	public void Map_EmployeeEntity_TeamLeaderDto_EqualValues()
	{
		//given
		//Employees
		var employee1 = new EmployeeEntity { Id = Guid.NewGuid(), Name = "Zofia", Surname = "Na³kowska", UserLogin = "znalkowska", WorkspaceType = WorkspaceType.Hybrid, ExternalId = 2 };

		//when
		TeamLeaderDto dtoMapped = _mapper.Map<TeamLeaderDto>(employee1);

		//then
		Assert.AreEqual(employee1.Id, dtoMapped.Id);
		Assert.AreEqual("Zofia", dtoMapped.Name);
		Assert.AreEqual("Na³kowska", dtoMapped.Surname);
	}

	[Test]
	public void Map_UpdateEmployeeWorkspaceTypeDto_EmployeeEntity_ReturnsTrue()
	{
		//given
		var employeeEntity1 = new EmployeeEntity { WorkspaceType = WorkspaceType.Office };
		var dto1 = new UpdateEmployeeWorkspaceTypeDto { WorkspaceType = Dtos.Enums.WorkspaceType.Office };

		//when
		UpdateEmployeeWorkspaceTypeDto dtoMapped = _mapper.Map<UpdateEmployeeWorkspaceTypeDto>(employeeEntity1);
		EmployeeEntity employeeEntityMapped = _mapper.Map<EmployeeEntity>(dto1);

		//then
		Assert.AreEqual(WorkspaceType.Office, employeeEntityMapped.WorkspaceType);
		Assert.AreEqual(Dtos.Enums.WorkspaceType.Office, dtoMapped.WorkspaceType);
	}

	[Test]
	public void Map_DtosWorkspaceType_DomainWorkspaceType_ReturnsTrue()
	{
		//given

		//when
		WorkspaceType mapped1 = _mapper.Map<WorkspaceType>(Dtos.Enums.WorkspaceType.Office);
		Dtos.Enums.WorkspaceType mapped2 = _mapper.Map<Dtos.Enums.WorkspaceType>(WorkspaceType.Office);

		//then
		Assert.AreEqual(WorkspaceType.Office, mapped1);
		Assert.AreEqual(Dtos.Enums.WorkspaceType.Office, mapped2);
	}
}
