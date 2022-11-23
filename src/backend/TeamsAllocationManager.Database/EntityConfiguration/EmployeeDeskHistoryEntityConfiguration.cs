using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TeamsAllocationManager.Domain.Models;

namespace TeamsAllocationManager.Database.EntityConfiguration;

public class EmployeeDeskHistoryEntityConfiguration : IEntityTypeConfiguration<EmployeeDeskHistoryEntity>
{
	public void Configure(EntityTypeBuilder<EmployeeDeskHistoryEntity> builder)
	{
		builder.HasOne(edh => edh.Desk)
			.WithMany(d => d.EmployeeDeskHistory)
			.HasForeignKey(edh => edh.DeskId)
			.OnDelete(DeleteBehavior.Cascade);
	}
}
