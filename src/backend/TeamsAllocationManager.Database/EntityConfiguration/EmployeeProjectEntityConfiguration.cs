using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TeamsAllocationManager.Domain.Models;

namespace TeamsAllocationManager.Database.EntityConfiguration;

public class EmployeeProjectEntityConfiguration : IEntityTypeConfiguration<EmployeeProjectEntity>
{
	public void Configure(EntityTypeBuilder<EmployeeProjectEntity> builder)
	{
		builder.HasOne(ep => ep.Employee)
			.WithMany(e => e.Projects)
			.HasForeignKey(ep => ep.EmployeeId);
		builder.HasOne(ep => ep.Project)
			.WithMany(p => p.Employees)
			.HasForeignKey(ep => ep.ProjectId);

		builder.HasIndex(ep => new { ep.EmployeeId, ep.ProjectId })
			.IsUnique();
	}
}
