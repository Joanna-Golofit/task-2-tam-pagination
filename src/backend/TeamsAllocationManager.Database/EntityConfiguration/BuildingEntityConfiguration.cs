using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TeamsAllocationManager.Domain.Models;

namespace TeamsAllocationManager.Database.EntityConfiguration;

public class BuildingEntityConfiguration : IEntityTypeConfiguration<BuildingEntity>
{
	public void Configure(EntityTypeBuilder<BuildingEntity> builder)
	{
		builder.Property(e => e.Name).HasMaxLength(50);
	}
}
