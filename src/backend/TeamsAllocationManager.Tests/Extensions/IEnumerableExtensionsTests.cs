using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TeamsAllocationManager.Contracts.EntityQueries;
using TeamsAllocationManager.Domain.Models;
using TeamsAllocationManager.Dtos.Room;
using TeamsAllocationManager.Infrastructure.Extensions;

namespace TeamsAllocationManager.Tests.Extensions;

[TestFixture]
public class IEnumerableExtensionsTests
{
	[Test]
	public void ShouldReturnProperType()
	{
		// given
		IEntityQuery[] queries =
		{
			new FirstQuery(),
			new FirstQuery(),
			new SecondQuery()
		};

		// when
		IRoomEntityQuery result = queries.SingleByType<IEntityQuery, IRoomEntityQuery>();

		// then
		Assert.AreEqual(queries[2], result);
	}

	[Test]
	public void ShouldThrowExceptionWhenMultipleInterfaceImplementation()
	{
		// given
		IEntityQuery[] commands =
		{
			new FirstQuery(),
			new SecondQuery(),
			new SecondQuery()
		};

		// when & then
		Assert.Throws<InvalidOperationException>(() => commands.SingleByType<IEntityQuery, IRoomEntityQuery>());
	}

	[Test]
	public void ShouldThrowExceptionWhenItemNotFound()
	{
		// given
		IEntityQuery[] commands =
		{
			new FirstQuery()
		};

		// when & then
		Assert.Throws<InvalidOperationException>(() => commands.SingleByType<IEntityQuery, IRoomEntityQuery>());
	}

	private class FirstQuery : IEntityQuery
	{
	}

	private class SecondQuery : IRoomEntityQuery
	{
		public Task<List<RoomEntity>> GetAllRoomsByUserRoleAsync(string username)
		{
			throw new NotImplementedException();
		}

		public Task<RoomDetailsDto> GetRoomAsync(Guid id)
		{
			throw new NotImplementedException();
		}

		public Task<RoomsDto> GetAllRoomsAsync()
		{
			throw new NotImplementedException();
		}

		public Task<PagedRoomsDto> GetFilteredRoomAsync(RoomsQueryFilterDto roomsQueryFilterDto)
		{
			throw new NotImplementedException();
		}
	}
}
