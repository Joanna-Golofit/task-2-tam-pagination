using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Moq;
using TeamsAllocationManager.Contracts.Desks.Commands;
using TeamsAllocationManager.Database;
using TeamsAllocationManager.Database.Repositories;
using TeamsAllocationManager.Database.Repositories.Interfaces;
using TeamsAllocationManager.Domain.Models;
using TeamsAllocationManager.Infrastructure.Exceptions;
using TeamsAllocationManager.Infrastructure.Handlers.Desk;
using TeamsAllocationManager.Infrastructure.Services.EmailFormatters;
using TeamsAllocationManager.Infrastructure.Services.Interfaces;
using TeamsAllocationManager.Mapper.Profiles;

namespace TeamsAllocationManager.Tests.Handlers.Desk;

[TestFixture]
public class SetHotDeskHandlerTests
{
	private readonly ApplicationDbContext _context;
	private readonly IMapper _mapper;
	private readonly Mock<IDesksRepository> _desksRepository = new Mock<IDesksRepository>();
	private readonly Mock<IMailComposer<HotDeskRemovedMailFormatter>> _mailComposer = new Mock<IMailComposer<HotDeskRemovedMailFormatter>>();
	private readonly Mock<IMailSenderService> _mailSenderService = new Mock<IMailSenderService>();

	public SetHotDeskHandlerTests()
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
		_context.SaveChanges();

		_desksRepository
			.Setup(r => r.GetDeskWithLocationAndHistoryAndReservation(It.IsAny<Guid>()))
			.Returns<Guid>(input => Task.FromResult(_context.Desks.SingleOrDefault(d => input == d.Id)));
	}

	[Test]
	public async Task ExecuteAsync_DeskIsNotHotDesk_SetsHotDesk()
	{
		// given
		DeskEntity desk = new DeskEntity
		{
			Id = Guid.NewGuid(),
			IsHotDesk = false,
			IsEnabled = true,
			Room = new RoomEntity
			{
				Id = Guid.NewGuid(),
				Name = "Room"
			}
		};

		_context.Desks.Add(desk);
		await _context.SaveChangesAsync();

		var command = new SetHotDeskCommand(desk.Id, desk.RoomId, true);
		var handler = new SetHotDeskHandler(_desksRepository.Object, _mailComposer.Object, _mailSenderService.Object, _mapper);

		// when
		Assert.DoesNotThrowAsync(async () => await handler.HandleAsync(command));

		// then
		DeskEntity resultDesk = await _context.Desks.Where(d => d.Id == desk.Id).SingleAsync();
		Assert.AreEqual(true, resultDesk.IsHotDesk);
	}

	[Test]
	public async Task ExecuteAsync_DeskIsHotDesk_UnsetsHotDesk()
	{
		// given
		DeskEntity desk = new DeskEntity
		{
			Id = Guid.NewGuid(),
			IsHotDesk = true,
			IsEnabled = true,
			Room = new RoomEntity
			{
				Id = Guid.NewGuid(),
				Name = "Room"
			}
		};

		_context.Desks.Add(desk);
		await _context.SaveChangesAsync();

		var command = new SetHotDeskCommand(desk.Id, desk.RoomId, false);
		var handler = new SetHotDeskHandler(_desksRepository.Object, _mailComposer.Object, _mailSenderService.Object, _mapper);

		// when
		Assert.DoesNotThrowAsync(async () => await handler.HandleAsync(command));

		// then
		DeskEntity resultDesk = await _context.Desks.Where(d => d.Id == desk.Id).SingleAsync();
		Assert.AreEqual(false, resultDesk.IsHotDesk);
	}

	[Test]
	public async Task ExecuteAsync_EmployeeOccupiesDesk_RemovesEmployeeAndSetsHotDesk()
	{
		// given
		DeskEntity desk = new DeskEntity
		{
			Id = Guid.NewGuid(),
			IsHotDesk = false,
			IsEnabled = true,
			Room = new RoomEntity
			{
				Id = Guid.NewGuid(),
				Name = "Room"
			}
		};

		_context.Desks.Add(desk);
		await _context.SaveChangesAsync();

		var command = new SetHotDeskCommand(desk.Id, desk.RoomId, true);
		var handler = new SetHotDeskHandler(_desksRepository.Object, _mailComposer.Object, _mailSenderService.Object, _mapper);


		// when
		Assert.DoesNotThrowAsync(async () => await handler.HandleAsync(command));

		// then
		DeskEntity resultDesk = await _context.Desks.Where(d => d.Id == desk.Id).SingleAsync();
		Assert.AreEqual(true, resultDesk.IsHotDesk);
	}

	[Test]
	public void ExecuteAsync_DeskIsNull_ReturnsError()
	{
		// given
		var command = new SetHotDeskCommand(Guid.NewGuid(), Guid.NewGuid(), true);
		var handler = new SetHotDeskHandler(_desksRepository.Object, _mailComposer.Object, _mailSenderService.Object, _mapper);


		// then
		Assert.ThrowsAsync<EntityNotFoundException<DeskEntity>>(async () => await handler.HandleAsync(command));
	}
}
