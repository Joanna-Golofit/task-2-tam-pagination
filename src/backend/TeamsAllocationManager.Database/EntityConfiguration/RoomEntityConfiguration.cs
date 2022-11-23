using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TeamsAllocationManager.Domain.Models;

namespace TeamsAllocationManager.Database.EntityConfiguration;

public class RoomEntityConfiguration : IEntityTypeConfiguration<RoomEntity>
{
	public void Configure(EntityTypeBuilder<RoomEntity> builder)
	{
		builder.Property(r => r.Name).HasMaxLength(50);
		builder.Property(r => r.Area).HasColumnType("decimal(6,2)");
	}
}
