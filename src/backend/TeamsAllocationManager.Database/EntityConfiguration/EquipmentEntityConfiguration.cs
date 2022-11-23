using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TeamsAllocationManager.Domain.Models;

namespace TeamsAllocationManager.Database.EntityConfiguration;

public class EquipmentEntityConfiguration : IEntityTypeConfiguration<EquipmentEntity>
{
	public void Configure(EntityTypeBuilder<EquipmentEntity> builder)
	{
		builder.Property(e => e.Name).HasMaxLength(100);
		builder.Property(e => e.AdditionalInfo).HasMaxLength(200);
	}
}
