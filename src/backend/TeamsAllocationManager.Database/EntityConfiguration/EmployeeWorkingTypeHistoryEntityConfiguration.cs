using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TeamsAllocationManager.Domain.Models;

namespace TeamsAllocationManager.Database.EntityConfiguration;

public class EmployeeWorkingTypeHistoryEntityConfiguration : IEntityTypeConfiguration<EmployeeWorkingTypeHistoryEntity>
{
	public void Configure(EntityTypeBuilder<EmployeeWorkingTypeHistoryEntity> builder)
	{
		builder.HasOne(ewth => ewth.Employee)
			.WithMany(e => e.EmployeeWorkingTypeHistory)
			.HasForeignKey(ewth => ewth.EmployeeId)
			.OnDelete(DeleteBehavior.Cascade);
		builder.Property(e => e.WorkspaceType)
			.HasConversion<int?>();
	}
}
