using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using Shouldly;
using TeamsAllocationManager.Contracts.Floor.Queries;
using TeamsAllocationManager.Database;
using TeamsAllocationManager.Database.Repositories;
using TeamsAllocationManager.Domain.Models;
using TeamsAllocationManager.Dtos.Common;
using TeamsAllocationManager.Dtos.Floor;
using TeamsAllocationManager.Infrastructure.Exceptions;
using TeamsAllocationManager.Infrastructure.Handlers.Floor;
using TeamsAllocationManager.Mapper.Profiles;
using TeamsAllocationManager.Tests.Helpers;

namespace TeamsAllocationManager.Tests.Handlers.Floor
{
	[TestFixture]
	public class GetFilteredFloorsCommandTests
	{
		private readonly ApplicationDbContext _context;
		private readonly IMapper _mapper;
		private readonly FloorsRepository _floorsRepository;
		private readonly BuildingsRepository _buildingsRepository;

		public GetFilteredFloorsCommandTests()
		{
			DbContextOptions<ApplicationDbContext> options = new DbContextOptionsBuilder<ApplicationDbContext>()
			                                                 .UseInMemoryDatabase(databaseName: GetType().Name)
			                                                 .Options;
			_context = new ApplicationDbContext(options);
			_mapper = new MapperConfiguration(mc => mc.AddProfile(new EntityDtoProfile())).CreateMapper();
			_floorsRepository = new FloorsRepository(_context);
			_buildingsRepository = new BuildingsRepository(_context);
		}

		[SetUp]
		public void SetupBeforeEachTest()
		{
			_context.ClearDatabase();

			var teamLeader1 = new EmployeeEntity { Name = "Jan", Surname = "Kowalski", Email = "jkowalski@fp.pl", ExternalId = 1, UserLogin = "jkowalski" };
			var teamLeader2 = new EmployeeEntity { Name = "Mariusz", Surname = "Szczepański", Email = "mszczepanski@fp.pl", ExternalId = 2, UserLogin = "mszczepanski" };
			var employee1 = new EmployeeEntity { Name = "Wojtek", Surname = "Golonka", Email = "wgolonka@fp.pl", ExternalId = 3, UserLogin = "wgolonka" };
			var employee2 = new EmployeeEntity { Name = "Anna", Surname = "Maria", Email = "amaria@fp.pl", ExternalId = 4, UserLogin = "amaria" };


			var building1 = new BuildingEntity { Name = "F1" };
			var floor1 = new FloorEntity { Building = building1, FloorNumber = 0 };
			var room1 = new RoomEntity { Area = 22.5m, Name = "001", Floor = floor1 };
			_context.Desks.AddRange(new DeskEntity { Room = room1, Number = 1 },
				DeskHelpers.CreateDeskWithReservation(room1, 2, teamLeader1),
				DeskHelpers.CreateDeskWithReservation(room1, 3, employee1),
				DeskHelpers.CreateDeskWithReservation(room1, 4, employee2),
				new DeskEntity { Room = room1, Number = 5 }); ;

			var floor2 = new FloorEntity { Building = building1, FloorNumber = 1 };
			var room2 = new RoomEntity { Area = 12.8m, Name = "002", Floor = floor2 };
			_context.Desks.AddRange(new DeskEntity { Room = room2, Number = 1 },
				DeskHelpers.CreateDeskWithReservation(room2, 2, teamLeader2),
				new DeskEntity { Room = room2, Number = 3 },
				new DeskEntity { Room = room2, Number = 4 },
				new DeskEntity { Room = room2, Number = 5 });

			var building2 = new BuildingEntity { Name = "F2" };
			var floor3 = new FloorEntity { Building = building2, FloorNumber = 2 };
			var room3 = new RoomEntity { Area = 37.2m, Name = "001", Floor = floor3 };
			_context.Desks.AddRange(new DeskEntity { Room = room3, Number = 1 },
				DeskHelpers.CreateDeskWithReservation(room3, 2, employee1),
				DeskHelpers.CreateDeskWithReservation(room3, 3, employee2),
				new DeskEntity { Room = room3, Number = 4 },
				new DeskEntity { Room = room3, Number = 5 });

			var floor4 = new FloorEntity { Building = building2, FloorNumber = 1 };
			var room4 = new RoomEntity { Area = 44.3m, Name = "002", Floor = floor4 };
			_context.Desks.AddRange(new DeskEntity { Room = room4, Number = 1 },
				new DeskEntity { Room = room4, Number = 2 },
				new DeskEntity { Room = room4, Number = 3 },
				new DeskEntity { Room = room4, Number = 4 },
				new DeskEntity { Room = room4, Number = 5 });

			var room5 = new RoomEntity { Area = 34.9m, Name = "003", Floor = floor4 };
			_context.Desks.AddRange(new DeskEntity { Room = room5, Number = 1 },
				new DeskEntity { Room = room5, Number = 2 },
				new DeskEntity { Room = room5, Number = 3 },
				new DeskEntity { Room = room5, Number = 4 },
				new DeskEntity { Room = room5, Number = 5 });

			_context.SaveChanges();
		}


		[Test]
		public async Task ShouldReturnAllFloors_WithFilterEmpty()
		{
			// given
			var query = new GetFilteredFloorsQuery(new FloorsQueryDto>());

			var handler = new GetFilteredFloorHandler(_floorsRepository, _buildingsRepository, _mapper);

			// when
			var result = await handler.HandleAsync(query);

			// then 
			Assert.AreEqual(4, result.Floors.Count);
			Assert.AreEqual(4, result.Floors.Payload!.Count());
			Assert.AreEqual(2, result.MaxFloor);
		}

		[Test]
		public async Task ShouldReturnResults_WithBuildingAndFloorNumberFilter()
		{
			// given
			var query = new GetFilteredFloorsQuery(new FloorsQueryDto>()
			{
				Filters = new FloorsQueryDto()
				{
					BuildingName = "F1",
					FloorNumber = 0,
				}
			});

			var handler = new GetFilteredFloorHandler(_floorsRepository, _buildingsRepository, _mapper);

			// when
			var result = await handler.HandleAsync(query);

			// then 
			Assert.AreEqual(1, result.Floors.Count);
			Assert.AreEqual(1, result.Floors.Payload!.Count());
			Assert.AreEqual(0, result.MaxFloor);

			var floorDto = result.Floors.Payload!.First();
			Assert.IsNotNull(floorDto);
			Assert.AreEqual("F1", floorDto.Building.Name);
			Assert.AreEqual(0, floorDto.Floor);
		}

		[Test]
		public async Task ShouldReturnResults_WithPagination()
		{
			// given
			var query = new GetFilteredFloorsQuery(new FloorsQueryDto>()
			{
				PageSize = 2,
				PageNumber = 0
			});

			var handler = new GetFilteredFloorHandler(_floorsRepository, _buildingsRepository, _mapper);

			// when
			var result = await handler.HandleAsync(query);

			// then 
			Assert.AreEqual(4, result.Floors.Count);
			Assert.AreEqual(2, result.Floors.Payload!.Count());
		}

		[Test]
		public async Task ShouldReturnResults_WithCapacityAndOccupiedDesksFilter()
		{
			// given
			var query = new GetFilteredFloorsQuery(new FloorsQueryDto>()
			{
				Filters = new FloorsQueryDto()
				{
					CapacityRange = new CapacityRangeFilterDto()
					{
						Min = 0,
						Max = 5
					},
					OccupiedDesksRange = new OccupiedDesksRangeFilterDto()
					{
						Min = 0
					}
				}
			});

			var handler = new GetFilteredFloorHandler(_floorsRepository, _buildingsRepository, _mapper);

			// when
			var result = await handler.HandleAsync(query);

			// then 
			Assert.AreEqual(3, result.Floors.Count);
			Assert.AreEqual(3, result.Floors.Payload!.Count());
		}

		[Test]
		public void ShouldThrowException_WithCapacityRangeFilterSetIncorrectly()
		{
			// given
			var query = new GetFilteredFloorsQuery(new FloorsQueryDto>()
			{
				Filters = new FloorsQueryDto()
				{
					CapacityRange = new CapacityRangeFilterDto()
					{
						Min = 5,
						Max = 0
					}
				}
			});

			var handler = new GetFilteredFloorHandler(_floorsRepository, _buildingsRepository, _mapper);

			// when
			var exception = Assert.ThrowsAsync<InvalidArgumentException>(async () => await handler.HandleAsync(query));

			// then 
			exception.ShouldNotBeNull();
			exception.Code.ShouldBe(4);
			exception.Status.ShouldBe("invalid_argument_exception");
			exception.Message.ShouldBe(ExceptionMessage.GetMessage(ExceptionMessage.InvalidArgument_IncorrectDeskCapacityRangeValues));
		}

		[Test]
		public void ShouldThrowException_WithOccupiedDeskRangeFilterSetIncorrectly()
		{
			// given
			var query = new GetFilteredFloorsQuery(new FloorsQueryDto>()
			{
				Filters = new FloorsQueryDto()
				{
					OccupiedDesksRange = new OccupiedDesksRangeFilterDto()
					{
						Min = 5,
						Max = 0
					}
				}
			});

			var handler = new GetFilteredFloorHandler(_floorsRepository, _buildingsRepository, _mapper);

			// when
			var exception = Assert.ThrowsAsync<InvalidArgumentException>(async () => await handler.HandleAsync(query));

			// then 
			exception.ShouldNotBeNull();
			exception.Code.ShouldBe(4);
			exception.Status.ShouldBe("invalid_argument_exception");
			exception.Message.ShouldBe(ExceptionMessage.GetMessage(ExceptionMessage.InvalidArgument_OccupiedDeskRangeValues));
		}
	}
}
