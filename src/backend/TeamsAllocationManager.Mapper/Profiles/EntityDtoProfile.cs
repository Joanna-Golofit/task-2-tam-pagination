using System;
using System.Collections.Generic;
using AutoMapper;
using System.Linq;
using TeamsAllocationManager.Domain.Enums;
using TeamsAllocationManager.Domain.Models;
using TeamsAllocationManager.Dtos.Building;
using TeamsAllocationManager.Dtos.Desk;
using TeamsAllocationManager.Dtos.Employee;
using TeamsAllocationManager.Dtos.Equipment;
using TeamsAllocationManager.Dtos.Floor;
using TeamsAllocationManager.Dtos.Project;
using TeamsAllocationManager.Dtos.Room;

namespace TeamsAllocationManager.Mapper.Profiles;

public class EntityDtoProfile : Profile
{
	public EntityDtoProfile()
	{
		CreateMap<RoomEntity, RoomDto>()
			.ForMember(dest => dest.Floor, opt => opt.MapFrom(src => src.Floor.FloorNumber))
			.ForMember(dest => dest.Building, opt => opt.MapFrom(src => src.Floor.Building))
			.ForMember(dest => dest.Capacity, opt => opt.MapFrom(src => src.Desks.Count))
			.ForMember(dest => dest.HotDesksCount, opt => opt.MapFrom(src => src.Desks.Count(d => d.IsHotDesk)))
			.ForMember(dest => dest.OccupiedDesksCount, opt => opt.MapFrom(src => GetOccupiedDeskCount(src)))
			.ForMember(dest => dest.DisabledDesksCount, opt => opt.MapFrom(src => src.Desks.Count(d => !d.IsEnabled)));

		CreateMap<RoomEntity, HotDeskRoomDto>()
			.ForMember(dest => dest.Floor, opt => opt.MapFrom(src => src.Floor.FloorNumber))
			.ForMember(dest => dest.Building, opt => opt.MapFrom(src => src.Floor.Building))
			.ForMember(dest => dest.HotDesksCount, opt => opt.MapFrom(src => src.Desks.Count(d => d.IsHotDesk)));

		CreateMap<RoomEntity, RoomForProjectDto>()
			.IncludeBase<RoomEntity, RoomDto>()
			.ForMember(dest => dest.DesksInRoom, opt => opt.MapFrom(src => src.Desks))
			.ForMember(dest => dest.Capacity, opt => opt.MapFrom(src => src.Desks.Count))
			.ForMember(dest => dest.Building, opt => opt.MapFrom(src => src.Floor.Building))
			.ForMember(dest => dest.Floor, opt => opt.MapFrom(src => src.Floor.FloorNumber));

		CreateMap<RoomEntity, RoomDetailsDto>()
			.IncludeBase<RoomEntity, RoomDto>()
			.ForMember(dest => dest.DesksInRoom, opt => opt.MapFrom(src => src.Desks))
			.ForMember(dest => dest.Floor, opt => opt.MapFrom(src => src.Floor.FloorNumber));

		CreateMap<FloorEntity, FloorDto>()
			.ForMember(dest => dest.Floor, opt => opt.MapFrom(src => src.FloorNumber));

		CreateMap<BuildingEntity, BuildingDto>();

		CreateMap<DeskEntity, DeskForProjectDetailsDto>()
			.ForMember(dest => dest.DeskHistory, opt => opt.MapFrom(src => src.EmployeeDeskHistory))
			.ForMember(dest => dest.Reservations, opt => opt.MapFrom(src => src.DeskReservations));

		CreateMap<DeskEntity, DeskForRoomDetailsDto>()
			.ForMember(dest => dest.Reservations, opt => opt.MapFrom(src => src.DeskReservations))
			.ForMember(dest => dest.DeskHistory, opt => opt.MapFrom(src => src.EmployeeDeskHistory));

		CreateMap<DeskEntity, RoomDeskDto>()
			.ForMember(dest => dest.DeskId, opt => opt.MapFrom(src => src.Id))
			.ForMember(dest => dest.RoomName, opt => opt.MapFrom(src => src.Room.Name))
			.ForMember(dest => dest.DeskNumber, opt => opt.MapFrom(src => src.Number));

		CreateMap<DeskEntity, DeskLocationDto>()
			.ForMember(dest => dest.LocationName, opt => opt.MapFrom(src => $"{src.Room.Floor.Building.Name} {src.Room.Name}"))
			.ForMember(dest => dest.DeskNumber, opt => opt.MapFrom(src => src.Number));

		CreateMap<DeskEntity, UserLocationDto>()
			.ForMember(dest => dest.DeskNumber, opt => opt.MapFrom(src => src.Number))
			.ForMember(dest => dest.RoomId, opt => opt.MapFrom(src => src.RoomId))
			.ForMember(dest => dest.RoomName, opt => opt.MapFrom(src => src.Room.Name))
			.ForMember(dest => dest.FloorNumber, opt => opt.MapFrom(src => src.Room.Floor.FloorNumber))
			.ForMember(dest => dest.BuildingName, opt => opt.MapFrom(src => src.Room.Floor.Building.Name));

		CreateMap<DeskReservationEntity, DeskReservationDto>()
			.ForMember(dest => dest.ReservationDate, opt => opt.MapFrom(src => src.ReservationStart.Date));

		CreateMap<DeskReservationEntity, ReservationInfoDto>();

		CreateMap<DeskReservationEntity, ScheduledDeskReservationInfoDto>();

		CreateMap<DeskReservationEntity, HotDeskReservationInfoDto>()
			.ForMember(dest => dest.EmployeeName, opt => opt.MapFrom(src => $"{src.Employee.Name} {src.Employee.Surname}"));

		CreateMap<EmployeeDeskHistoryEntity, DeskHistoryDto>()
			.ForMember(dest => dest.DeskId, opt => opt.MapFrom(src => src.DeskId))
			.ForMember(dest => dest.EmployeeId, opt => opt.MapFrom(src => src.EmployeeId))
			.ForMember(dest => dest.EmployeeName, opt => opt.MapFrom(src => src.Employee!.Name))
			.ForMember(dest => dest.EmployeeSurname, opt => opt.MapFrom(src => src.Employee!.Surname))
			.ForMember(dest => dest.From, opt => opt.MapFrom(src => src.From))
			.ForMember(dest => dest.To, opt => opt.MapFrom(src => src.To));

		CreateMap<ProjectEntity, ProjectDto>()
			.IncludeAllDerived();

		CreateMap<ProjectEntity, ProjectDetailsDto>()
			.ForMember(dest => dest.Employees, opt => opt.MapFrom(src => src.Employees))
			.ForMember(dest => dest.AssignedPeopleCount, opt => opt.MapFrom(src => src.Employees.Count(e => e.Employee.WorkspaceType != null && e.Employee.WorkspaceType != WorkspaceType.Remote && e.Employee.EmployeeDeskReservations.Any(edr => edr.IsSchedule))))
			.ForMember(dest => dest.TeamLeaders, opt => opt.MapFrom(src => src.TeamLeaders))
			.ForMember(dest => dest.PeopleCount, opt => opt.MapFrom(src => src.Employees.Count));

		CreateMap<ProjectEntity, ProjectForRoomDetailsDto>()
			.ForMember(p => p.TeamLeaders, opt => opt.MapFrom(src => src.TeamLeaders));

		CreateMap<EmployeeProjectEntity, EmployeeForProjectDetailsDto>()
			.ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Employee.Name))
			.ForMember(dest => dest.Surname, opt => opt.MapFrom(src => src.Employee.Surname))
			.ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.EmployeeId))
			.ForMember(dest => dest.ProjectsNames, opt => opt.MapFrom(src => src.Employee.Projects.Select(p => p.Project.Name).ToList()))
			.ForMember(dest => dest.RoomDeskDtos, opt => opt.MapFrom(src => src.Employee.EmployeeDeskReservations.Where(edr => edr.IsSchedule).Select(edr => edr.Desk)))
			.ForMember(dest => dest.Workmode, opt => opt.MapFrom(src => src.Employee.WorkspaceType));

		// TODO: Tests
		CreateMap<EmployeeProjectEntity, ProjectForRoomDetailsDto>()
			.ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Project.Id))
			.ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Project.Name))
			.ForMember(dest => dest.TeamLeaders, opt => opt.MapFrom(src => src.Project.TeamLeaders));

		CreateMap<EmployeeProjectEntity, UserProjectDto>()
			.ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Project.Id))
			.ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Project.Name))
			.ForMember(dest => dest.TeamLeadersNames, opt => opt.MapFrom(src => src.Project.TeamLeaders
				.OrderBy(tl => tl.Surname)
				.Select(tl => $"{tl.Name} {tl.Surname}")));

		CreateMap<EmployeeEntity, EmployeeForRoomDetailsDto>()
			.ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
			.ForMember(dest => dest.RoomDeskDtos, opt => opt.MapFrom(src => src.EmployeeDeskReservations.Where(edr => edr.IsSchedule).Select(edr => edr.Desk)))
			.ForMember(dest => dest.ProjectsNames, opt => opt.MapFrom(src => src.Projects.Select(p => p.Project.Name).ToList()));

		CreateMap<EmployeeEntity, EmployeeForProjectDetailsDto>()
			.ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
			.ForMember(dest => dest.Surname, opt => opt.MapFrom(src => src.Surname))
			.ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
			.ForMember(dest => dest.RoomDeskDtos, opt => opt.MapFrom(src => src.EmployeeDeskReservations.Where(edr => edr.IsSchedule).Select(edr => edr.Desk)))
			.ForMember(dest => dest.Workmode, opt => opt.MapFrom(src => src.WorkspaceType));

		CreateMap<EmployeeEntity, TeamLeaderDto>()
			.ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
			.ForMember(dest => dest.Surname, opt => opt.MapFrom(src => src.Surname));

		CreateMap<EmployeeEntity, EmployeeBasicInfoDto>()
			.ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
			.ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
			.ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
			.ForMember(dest => dest.Surname, opt => opt.MapFrom(src => src.Surname));

		CreateMap<EmployeeEntity, LoggedUserDataDto>()
			.ForMember(dest => dest.RoleNames, opt => opt.MapFrom(src => src.UserRoles.Select(ur => ur.Role.Name)));

		CreateMap<EmployeeEntity, UserDetailsDto>()
			.ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Projects.FirstOrDefault() != null && !string.IsNullOrEmpty(src.Projects.First().Project.Email) ? src.Projects.First().Project.Email : src.Email))
			.ForMember(dest => dest.EmployeeType, opt => opt.MapFrom(src => src.UserRoles.Any(ur => ur.Role.Name == RoleEntity.TeamLeader) ? 1 : 0))
			.ForMember(dest => dest.EmployeeName, opt => opt.MapFrom(src => src.Name))
			.ForMember(dest => dest.EmployeeSurname, opt => opt.MapFrom(src => src.Surname))
			.ForMember(dest => dest.Locations, opt => opt.MapFrom(src => src.EmployeeDeskReservations.Where(edr => edr.IsSchedule).Select(edr => edr.Desk)))
			.ForMember(dest => dest.Projects, opt => opt.MapFrom(src => src.Projects.OrderBy(p => p.Project.Name)))
			.ForMember(dest => dest.ReservationInfo, opt => opt.MapFrom(src => src.EmployeeDeskReservations.Where(edr => !edr.IsSchedule)))
			.ForMember(dest => dest.LedProjectsReservationInfo, opt => opt.MapFrom(src => src.LedProjects.SelectMany(p => p.Employees.SelectMany(e => e.Employee.EmployeeDeskReservations.Where(edr => edr.CreatedById == src.Id && !edr.IsSchedule)))));

		CreateMap<UpdateEmployeeWorkspaceTypeDto, EmployeeEntity>()
			.ForMember(dest => dest.WorkspaceType, opt => opt.MapFrom(src => src.WorkspaceType)).ReverseMap();

		CreateMap<Dtos.Enums.WorkspaceType, WorkspaceType>().ReverseMap();

		CreateMap<EmployeeEquipmentHistoryEntity, EquipmentReservationHistoryDto>()
			.ForMember(dest => dest.EmployeeName, opt => opt.MapFrom(src => src.Employee!.Name))
			.ForMember(dest => dest.EmployeeSurname, opt => opt.MapFrom(src => src.Employee!.Surname));

		CreateMap<EquipmentEntity, EquipmentDto>()
			.ForMember(dest => dest.AssignedPeopleCount, opt => opt.MapFrom(src => src.EmployeeEquipmentReservations.Count));
		CreateMap<EmployeeEquipmentEntity, ReservationEquipmentDto>();

		CreateMap<EquipmentEntity, EquipmentDetailDto>()
			.ForMember(dest => dest.Employees, opt => opt.MapFrom(src => src.EmployeeEquipmentReservations))
			.ForMember(dest => dest.ReservationsHistory, opt => opt.MapFrom(src => src.EmployeeEquipmentHistory))
			.ForMember(dest => dest.AssignedPeopleCount, opt => opt.MapFrom(src => src.EmployeeEquipmentReservations.Count));

		CreateMap<EmployeeEquipmentEntity, EmployeeForEquipmentDetailsDto>()
			.ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Employee.Name))
			.ForMember(dest => dest.Surname, opt => opt.MapFrom(src => src.Employee.Surname))
			.ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.EmployeeId));

		CreateMap<EmployeeEquipmentEntity, EmployeeEquipmentDetailDto>()
		 .ForMember(dest => dest.EquipmentName, opt => opt.MapFrom(src => src.Equipment.Name));
	}

	private int GetOccupiedDeskCount(RoomEntity room)
	{
		return room.Desks.Count(d => d.DeskReservations.Any(dr => dr.IsSchedule
		                                                          && dr.ScheduledWeekdays.Contains(DayOfWeek.Monday)
		                                                          && dr.ScheduledWeekdays.Contains(DayOfWeek.Tuesday)
		                                                          && dr.ScheduledWeekdays.Contains(DayOfWeek.Wednesday)
		                                                          && dr.ScheduledWeekdays.Contains(DayOfWeek.Thursday)
		                                                          && dr.ScheduledWeekdays.Contains(DayOfWeek.Friday)));
	}
}
