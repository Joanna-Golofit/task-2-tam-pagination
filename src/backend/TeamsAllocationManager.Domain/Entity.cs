using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TeamsAllocationManager.Domain;

public abstract class Entity
{
	[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
	[Key]
	public Guid Id { get; set; }
	public DateTime Created { get; set; }
	public DateTime Updated { get; set; }
}
