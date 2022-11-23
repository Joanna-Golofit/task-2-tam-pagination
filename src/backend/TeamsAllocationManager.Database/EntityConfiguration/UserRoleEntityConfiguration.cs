using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TeamsAllocationManager.Domain.Models;

namespace TeamsAllocationManager.Database.EntityConfiguration;

public class UserRoleEntityConfiguration : IEntityTypeConfiguration<UserRoleEntity>
{
	public void Configure(EntityTypeBuilder<UserRoleEntity> builder)
	{
		builder.HasOne(ur => ur.Employee)
			.WithMany(e => e.UserRoles)
			.HasForeignKey(ur => ur.EmployeeId);
		builder.HasOne(ur => ur.Role)
			.WithMany()
			.HasForeignKey(ur => ur.RoleId);
		builder.HasIndex(ur => new { ur.RoleId, ur.EmployeeId })
			.IsUnique();
	}
}
