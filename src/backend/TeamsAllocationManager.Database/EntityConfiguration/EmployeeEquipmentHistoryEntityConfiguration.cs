using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TeamsAllocationManager.Domain.Models;

namespace TeamsAllocationManager.Database.EntityConfiguration;

public class EmployeeEquipmentHistoryEntityConfiguration : IEntityTypeConfiguration<EmployeeEquipmentHistoryEntity>
{
	public void Configure(EntityTypeBuilder<EmployeeEquipmentHistoryEntity> builder)
	{
		builder.HasOne(eh => eh.Equipment)
			    .WithMany(e => e.EmployeeEquipmentHistory)
			    .HasForeignKey(eh => eh.EquipmentId)
			    .OnDelete(DeleteBehavior.Cascade);
	}
}
