using TeamsAllocationManager.Domain;

namespace TeamsAllocationManager.Infrastructure.Exceptions;

public class EntityDuplicateException<TEntity> : ExceptionBase where TEntity : Entity
{
	private string _field = "";

	public EntityDuplicateException(string field) : base(
		$"Unique constraint of {typeof(TEntity).Name} on field {field} violation")
	{
		_field = field;
	}
	public EntityDuplicateException() : base($"Unique constraint of {typeof(TEntity).Name} violation") { }

	public override int Code => 2;
	public override string Status => string.IsNullOrEmpty(_field) ? "entity_duplication_exception" : $"{_field}_duplication_exception";
}
