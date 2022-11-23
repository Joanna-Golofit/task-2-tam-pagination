using TeamsAllocationManager.Domain.Models;

namespace TeamsAllocationManager.Infrastructure.Exceptions;

public class ExternalCompanyEntityDuplicateException : ExceptionBase
{
	private readonly string _field;

	public ExternalCompanyEntityDuplicateException(string field) : base(
		$"Unique constraint of External Company ({nameof(ProjectEntity)}) on field {field} violation")
	{
		_field = field;
	}

	public override int Code => 2;
	public override string Status => $"external_company_{_field}_duplication";
}
