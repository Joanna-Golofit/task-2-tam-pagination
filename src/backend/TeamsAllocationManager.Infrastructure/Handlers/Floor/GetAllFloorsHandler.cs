using System;
using AutoMapper;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TeamsAllocationManager.Contracts.Base.Queries;
using TeamsAllocationManager.Contracts.Floor.Queries;
using TeamsAllocationManager.Database.Repositories.Interfaces;
using TeamsAllocationManager.Domain.Models;
using TeamsAllocationManager.Dtos.Building;
using TeamsAllocationManager.Dtos.Floor;

namespace TeamsAllocationManager.Infrastructure.Handlers.Floor;

public class GetAllFloorsHandler : IAsyncQueryHandler<GetAllFloorsQuery, FloorsDto>
{
	private readonly IFloorsRepository _floorsRepository;
	private readonly IBuildingsRepository _buildingsRepository;
	private readonly IMapper _mapper;

	public GetAllFloorsHandler(IFloorsRepository floorsRepository, 
		IBuildingsRepository buildingsRepository,
		IMapper mapper)
	{
		_floorsRepository = floorsRepository;
		_buildingsRepository = buildingsRepository;
		_mapper = mapper;
	}

	public async Task<FloorsDto> HandleAsync(GetAllFloorsQuery query, CancellationToken cancellationToken = default)
	{
		var floors = (await _floorsRepository.GetFloors())
			.Select(f =>
			{
				FloorDto floor = _mapper.Map<FloorDto>(f);
				floor.Area = f.Rooms.Sum(r => r.Area);
				floor.Capacity = f.Rooms.Sum(r => r.Desks.Count());
				floor.OccupiedDesks = f.Rooms.Sum(r => r.Desks.Count(d => d.DeskReservations.Any(dr => dr.IsSchedule && dr.ContainsAllWeekDays())));
				floor.RoomCount = f.Rooms.Count();
				return floor;
			})
			.ToList();

		return new FloorsDto
		{
			Buildings = _mapper.Map<IEnumerable<BuildingDto>>(await _buildingsRepository.GetAllAsync()),
			MaxFloor = floors.Any() ? floors.Max(r => r.Floor) : 0,
			Floors = floors
		};
	}
}
