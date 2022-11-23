namespace TeamsAllocationManager.Infrastructure.Exceptions;

public class EquipmentEntityDuplicateException : ExceptionBase
{
	private readonly string _field;	

	public EquipmentEntityDuplicateException(string field) : base(
		$"Duplicate name's non-it-equipment(${field}) found! field  violation")
	{
		_field = field;
		TranslationKey = ExceptionMessage.Equipments_DuplicateName;
	}

	public override int Code => 2;
	public override string Status => $"equipment_{_field}_duplication";
}
