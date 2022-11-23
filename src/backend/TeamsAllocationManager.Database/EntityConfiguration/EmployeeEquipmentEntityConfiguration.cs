using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TeamsAllocationManager.Domain.Models;

namespace TeamsAllocationManager.Database.EntityConfiguration;

public class EmployeeEquipmentEntityConfiguration : IEntityTypeConfiguration<EmployeeEquipmentEntity>
{
	public void Configure(EntityTypeBuilder<EmployeeEquipmentEntity> builder)
	{
		builder.HasOne(ee => ee.Equipment)
			    .WithMany(e => e.EmployeeEquipmentReservations)
			    .HasForeignKey(ee => ee.EquipmentId);
		builder.HasOne(ee => ee.Employee)
			    .WithMany(e => e.EmployeeEquipment)
			    .HasForeignKey(ee => ee.EmployeeId);

		builder.HasIndex(ee => new {ee.EmployeeId, ee.EquipmentId})
			    .IsUnique();
	}
}
