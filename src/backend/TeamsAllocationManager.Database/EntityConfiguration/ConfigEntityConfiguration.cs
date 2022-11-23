using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using TeamsAllocationManager.Domain.Models;

namespace TeamsAllocationManager.Database.EntityConfiguration;

public class ConfigEntityConfiguration : IEntityTypeConfiguration<ConfigEntity>
{
	public void Configure(EntityTypeBuilder<ConfigEntity> builder)
	{
		builder.Property(p => p.Key)
			.HasConversion<string>()
			.HasMaxLength(30)
			.IsRequired();
		builder.HasIndex(p => p.Key)
			.IsUnique();
		builder.Property(p => p.Data)
			.IsRequired();
	}
}
