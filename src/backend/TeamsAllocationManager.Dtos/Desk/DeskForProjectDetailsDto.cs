using System;
using System.Collections.Generic;
using TeamsAllocationManager.Dtos.Room;

namespace TeamsAllocationManager.Dtos.Desk;

public class DeskForProjectDetailsDto
{
	public Guid Id { get; set; }
	public int Number { get; set; }
	public bool IsHotDesk { get; set; }
	public bool IsEnabled { get; set; }
	public IList<DeskHistoryDto> DeskHistory { get; set; } = new List<DeskHistoryDto>();
	public IEnumerable<DeskReservationDto>? Reservations { get; set; }
}
