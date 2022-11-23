using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TeamsAllocationManager.Domain.Models;

namespace TeamsAllocationManager.Database.EntityConfiguration;

public class EmployeeEntityConfiguration : IEntityTypeConfiguration<EmployeeEntity>
{
	public void Configure(EntityTypeBuilder<EmployeeEntity> builder)
	{
		builder.Property(e => e.Email).HasMaxLength(255);
		builder.Property(e => e.UserLogin).HasMaxLength(255);
		builder.Property(e => e.Name).HasMaxLength(255);
		builder.Property(e => e.Surname).HasMaxLength(255);
		builder.Property(e => e.WorkspaceType)
			.HasConversion<int?>();

		builder.HasIndex(e => e.Email).IsUnique();
		builder.HasIndex(e => e.UserLogin).IsUnique();

		builder.Ignore(e => e.LedProjects);
	}
}
