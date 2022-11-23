using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TeamsAllocationManager.Domain.Models;

namespace TeamsAllocationManager.Database.EntityConfiguration;

public class ProjectEntityConfiguration : IEntityTypeConfiguration<ProjectEntity>
{
	public void Configure(EntityTypeBuilder<ProjectEntity> builder)
	{
		builder.Property(p => p.Name).HasMaxLength(255);

		builder.Ignore(p => p.TeamLeaders);
	}
}
