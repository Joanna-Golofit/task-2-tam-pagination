using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TeamsAllocationManager.Domain.Models;

namespace TeamsAllocationManager.Database.EntityConfiguration;

public class FloorEntityConfiguration : IEntityTypeConfiguration<FloorEntity>
{
	public void Configure(EntityTypeBuilder<FloorEntity> builder)
	{
	}
}
