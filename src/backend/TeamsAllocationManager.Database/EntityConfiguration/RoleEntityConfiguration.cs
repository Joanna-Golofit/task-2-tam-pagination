using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TeamsAllocationManager.Domain.Models;

namespace TeamsAllocationManager.Database.EntityConfiguration;

public class RoleEntityConfiguration : IEntityTypeConfiguration<RoleEntity>
{
	public void Configure(EntityTypeBuilder<RoleEntity> builder)
	{
		builder.Property(p => p.Name)
			.HasMaxLength(50)
			.IsRequired();
		builder.HasIndex(p => p.Name)
			.IsUnique();
	}
}
