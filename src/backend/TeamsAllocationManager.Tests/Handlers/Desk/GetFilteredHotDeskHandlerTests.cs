using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using Shouldly;
using TeamsAllocationManager.Contracts.Desks.Queries;
using TeamsAllocationManager.Database;
using TeamsAllocationManager.Database.Repositories;
using TeamsAllocationManager.Database.Repositories.Interfaces;
using TeamsAllocationManager.Domain.Models;
using TeamsAllocationManager.Dtos.Common;
using TeamsAllocationManager.Dtos.Desk;
using TeamsAllocationManager.Dtos.Enums;
using TeamsAllocationManager.Infrastructure.Exceptions;
using TeamsAllocationManager.Infrastructure.Handlers.Desk;
using TeamsAllocationManager.Mapper.Profiles;
using TeamsAllocationManager.Tests.Helpers;

namespace TeamsAllocationManager.Tests.Handlers.Desk
{
	[TestFixture]
	public class GetFilteredHotDeskHandlerTests
	{
		private readonly ApplicationDbContext _context;
		private readonly IRoomRepository _roomRepository;
		private readonly IBuildingsRepository _buildingsRepository;
		private readonly IMapper _mapper;

		public GetFilteredHotDeskHandlerTests()
		{
			DbContextOptions<ApplicationDbContext> options = new DbContextOptionsBuilder<ApplicationDbContext>()
			                                                 .UseInMemoryDatabase(databaseName: GetType().Name)
			                                                 .Options;
			_context = new ApplicationDbContext(options);
			_roomRepository = new RoomRepository(_context);
			_mapper = new MapperConfiguration(mc => mc.AddProfile(new EntityDtoProfile())).CreateMapper();
			_buildingsRepository = new BuildingsRepository(_context);
		}

		[SetUp]
		public void SetupBeforeEachTest()
		{
			_context.ClearDatabase();

			var employee1 = new EmployeeEntity
			{
				Id = Guid.NewGuid(),
				Name = "Katarzyna",
				Surname = "Nowak",
				Email = "knowak@fp.pl",
				ExternalId = 2,
				UserLogin = "knowak"
			};

			var employee2 = new EmployeeEntity
			{
				Id = Guid.NewGuid(),
				Name = "Andrzej",
				Surname = "Sztacheta",
				Email = "asztacheta@fp.pl",
				ExternalId = 3,
				UserLogin = "asztacheta"
			};

			_context.Employees.AddRange(employee1);

			var floor1 = new FloorEntity { Building = new BuildingEntity { Name = "F1" }, FloorNumber = 0 };

			var room1 = new RoomEntity { Area = 22.5m, Name = "001", Floor = floor1 };

			_context.Desks.AddRange(
				DeskHelpers.CreateHotDeskWithReservation(room1, 1, employee1),
				DeskHelpers.CreateHotDeskWithReservation(room1, 2, employee1),
				DeskHelpers.CreateHotDeskWithReservation(room1, 2, employee2),
				new DeskEntity { Room = room1, Number = 3 }
			);

			var room2 = new RoomEntity { Area = 22.5m, Name = "002", Floor = floor1 };

			_context.Desks.AddRange(
				DeskHelpers.CreateHotDeskWithReservation(room2, 1, employee1),
				new DeskEntity { Room = room2, Number = 3 }
			);

			var floor2 = new FloorEntity { Building = new BuildingEntity { Name = "F2" }, FloorNumber = 0 };

			var room3 = new RoomEntity { Area = 22.5m, Name = "001", Floor = floor2 };

			_context.Desks.AddRange(DeskHelpers.CreateHotDeskWithReservation(room3, 1, employee1),
				new DeskEntity { Room = room3, Number = 2, IsEnabled = true, IsHotDesk = true });

			_context.SaveChanges();
		}


		[Test]
		public void ShouldThrowsReservationDateRangeException_WithEmptyFilter()
		{
			//given
			var pagedOptions = new GetHotDesksQueryDto();
			var query = new GetFilteredHotDesksQuery();

			var queryHandler = new GetFilteredHotDesksHandler(_roomRepository, _buildingsRepository, _mapper);

			// when
			var exception = Assert.ThrowsAsync<HotDeskGetReservationException>(async () => await queryHandler.HandleAsync(query));

			// then
			exception.ShouldNotBeNull();
			exception.Code.ShouldBe(9);
			exception.Status.ShouldBe("get_desk_reservation_failed");
			exception.Message.ShouldBe(ExceptionMessage.GetMessage(ExceptionMessage.HotDesks_StartDateOrEndDateIsEmpty));
		}

		[Test]
		public async Task ShouldReturnResult_WithDateRangeFilter()
		{
			//given
			var pagedOptions = new GetHotDesksQueryDto()
			{
				Filters = new GetHotDesksQueryDto()
				{
					ReservationDateRangeFilter = new ReservationDateRangeFilterDto()
					{
						StartDate = DateTime.Today.ToString(),
						EndDate = DateTime.Today.AddDays(7).ToString()
					}
				}
			};

			var query = new GetFilteredHotDesksQuery(pagedOptions);

			var queryHandler = new GetFilteredHotDesksHandler(_roomRepository, _buildingsRepository, _mapper);

			// when
			var result = await queryHandler.HandleAsync(query);

			// then
			Assert.AreEqual(_context.Rooms.Count(), result.Rooms.Payload!.Count());
			Assert.AreEqual(6, result.Rooms.Payload!.Sum(r => r.HotDesksCount));
			Assert.AreEqual(1, result.Rooms.Payload!.Sum(r => r.FreeHotDeskCount));
		}

		[Test]
		public void ShouldThrowException_WithIncorrectDateRangeFilter()
		{
			//given
			var pagedOptions = new GetHotDesksQueryDto>()
			{
				Filters = new GetHotDesksQueryDto()
				{
					ReservationDateRangeFilter = new ReservationDateRangeFilterDto()
					{
						StartDate = DateTime.Today.AddDays(7).ToString(),
						EndDate = DateTime.Today.ToString()
					}
				}
			};

			var query = new GetFilteredHotDesksQuery(pagedOptions);

			var queryHandler = new GetFilteredHotDesksHandler(_roomRepository, _buildingsRepository, _mapper);

			// when
			// when
			var exception = Assert.ThrowsAsync<HotDeskGetReservationException>(async () => await queryHandler.HandleAsync(query));

			// then
			// then
			exception.ShouldNotBeNull();
			exception.Code.ShouldBe(9);
			exception.Status.ShouldBe("get_desk_reservation_failed");
			exception.Message.ShouldBe(ExceptionMessage.GetMessage(ExceptionMessage.HotDesks_StartDateGreaterThanEndDate));
		}

		[Test]
		public async Task ShouldReturnResult_WithBuildingNameAndRoomName()
		{
			//given
			var pagedOptions = new GetHotDesksQueryDto>()
			{
				Filters = new GetHotDesksQueryDto()
				{
					ReservationDateRangeFilter = new ReservationDateRangeFilterDto()
					{
						StartDate = DateTime.Today.ToString(),
						EndDate = DateTime.Today.AddDays(7).ToString()
					},
					BuildingName = "F2",
					RoomName = "001"
				}
			};

			var query = new GetFilteredHotDesksQuery(pagedOptions);

			var queryHandler = new GetFilteredHotDesksHandler(_roomRepository, _buildingsRepository, _mapper);

			// when
			var result = await queryHandler.HandleAsync(query);

			// then
			Assert.AreEqual(1, result.Rooms.Payload!.Count());
			Assert.AreEqual(2, result.Rooms.Payload!.Sum(r => r.HotDesksCount));
		}

		[Test]
		public void ShouldThrowException_WithIncorrectFreeDesksRange()
		{
			//given
			var pagedOptions = new GetHotDesksQueryDto>()
			{
				Filters = new GetHotDesksQueryDto()
				{
					ReservationDateRangeFilter = new ReservationDateRangeFilterDto()
					{
						StartDate = DateTime.Today.ToString(),
						EndDate = DateTime.Today.AddDays(7).ToString()
					},
					FreeDesksRange = new FreeDesksRangeFilterDto()
					{
						Min = 2,
						Max = 1
					}
				}
			};

			var query = new GetFilteredHotDesksQuery(pagedOptions);

			var queryHandler = new GetFilteredHotDesksHandler(_roomRepository, _buildingsRepository, _mapper);

			// when
			var exception = Assert.ThrowsAsync<HotDeskGetReservationException>(async () => await queryHandler.HandleAsync(query));

			// then
			// then
			exception.ShouldNotBeNull();
			exception.Code.ShouldBe(9);
			exception.Status.ShouldBe("get_desk_reservation_failed");
			exception.Message.ShouldBe(ExceptionMessage.GetMessage(ExceptionMessage.InvalidArgument_IncorrectFreeDeskRangeValues));
		}

		[Test]
		public async Task ShouldReturnResult_WithFreeDesksRangeFilter()
		{
			//given
			var pagedOptions = new GetHotDesksQueryDto>()
			{
				Filters = new GetHotDesksQueryDto()
				{
					ReservationDateRangeFilter = new ReservationDateRangeFilterDto()
					{
						StartDate = DateTime.Today.ToString(),
						EndDate = DateTime.Today.AddDays(7).ToString()
					},
					FreeDesksRange = new FreeDesksRangeFilterDto()
					{
						Min = 1,
						Max = 1
					}
				}
			};

			var query = new GetFilteredHotDesksQuery(pagedOptions);

			var queryHandler = new GetFilteredHotDesksHandler(_roomRepository, _buildingsRepository, _mapper);

			// when
			var result = await queryHandler.HandleAsync(query);

			// then
			Assert.AreEqual(1, result.Rooms.Payload!.Count());
			Assert.AreEqual(1, result.Rooms.Payload!.Sum(r => r.FreeHotDeskCount));
		}

		[Test]
		public async Task ShouldReturnResult_WithPagination()
		{
			//given
			var pagedOptions = new GetHotDesksQueryDto>()
			{
				PageSize = 2,
				PageNumber = 0,
				Filters = new GetHotDesksQueryDto()
				{
					ReservationDateRangeFilter = new ReservationDateRangeFilterDto()
					{
						StartDate = DateTime.Today.ToString(),
						EndDate = DateTime.Today.AddDays(7).ToString()
					},
				}
			};

			var query = new GetFilteredHotDesksQuery(pagedOptions);

			var queryHandler = new GetFilteredHotDesksHandler(_roomRepository, _buildingsRepository, _mapper);

			// when
			var result = await queryHandler.HandleAsync(query);

			// then
			Assert.AreEqual(2, result.Rooms.Payload!.Count());
			Assert.AreEqual(3, result.Rooms.Count);
		}
	}
}
