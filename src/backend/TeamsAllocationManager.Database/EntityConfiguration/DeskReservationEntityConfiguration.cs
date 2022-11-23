using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System;
using System.Collections.Generic;
using System.Linq;
using TeamsAllocationManager.Domain.Models;

namespace TeamsAllocationManager.Database.EntityConfiguration;

public class DeskReservationEntityConfiguration : IEntityTypeConfiguration<DeskReservationEntity>
{
	public void Configure(EntityTypeBuilder<DeskReservationEntity> builder)
	{
		builder.HasOne(dr => dr.Desk)
			    .WithMany(d => d.DeskReservations)
			    .HasForeignKey(dr => dr.DeskId)
			    .OnDelete(DeleteBehavior.Cascade);
		builder.HasOne(dr => dr.Employee)
			    .WithMany(e => e.EmployeeDeskReservations)
			    .HasForeignKey(dr => dr.EmployeeId)
			    .OnDelete(DeleteBehavior.Cascade);

		var dayOfWeekConverter = new ValueConverter<IEnumerable<DayOfWeek>, string>(
			input => string.Join(",", input),
			dbValue => string.IsNullOrWhiteSpace(dbValue) 
				? new List<DayOfWeek>() 
				: dbValue.Split(new[] { ',' }).Select(value => Enum.Parse<DayOfWeek>(value)).ToList());

		builder.Property(dr => dr.ScheduledWeekdays).HasConversion(dayOfWeekConverter);
	}
}
