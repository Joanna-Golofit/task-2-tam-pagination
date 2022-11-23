using System.Collections.Generic;
using TeamsAllocationManager.Contracts.Base.Queries;
using TeamsAllocationManager.Dtos.Room;

namespace TeamsAllocationManager.Contracts.Desks.Queries
{
	public class GetFilteredHotDesksQuery : IQuery<PagedHotDeskRoomsDto>
	{
		public GetFilteredHotDesksQuery(){
		}
	}
}