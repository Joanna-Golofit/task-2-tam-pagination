using System.ComponentModel.DataAnnotations;

namespace TeamsAllocationManager.Dtos.Equipment;

public class AddEquipmentDto
{
	[Required]
	public string Name { get; set; } = "";

	public string AdditionalInfo { get; set; } = "";
		
	[Required]
	public int Count { get; set; }
}
