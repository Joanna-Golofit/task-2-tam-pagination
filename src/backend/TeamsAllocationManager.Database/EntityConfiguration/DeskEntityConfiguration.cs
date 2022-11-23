using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TeamsAllocationManager.Domain.Models;

namespace TeamsAllocationManager.Database.EntityConfiguration;

public class DeskEntityConfiguration : IEntityTypeConfiguration<DeskEntity>
{
	public void Configure(EntityTypeBuilder<DeskEntity> builder)
	{
		builder.HasIndex(desk => new { desk.RoomId, desk.Number }).IsUnique();
	}
}
