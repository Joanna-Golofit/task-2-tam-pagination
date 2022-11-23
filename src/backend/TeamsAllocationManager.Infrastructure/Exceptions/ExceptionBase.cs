using System;
using System.Net;

namespace TeamsAllocationManager.Infrastructure.Exceptions;

public abstract class ExceptionBase : Exception
{
	public ExceptionBase(string message) : base(message) { }

	public abstract int Code { get; }
	public abstract string Status { get; }
	public virtual string? TranslationKey { get; init; }
	public virtual HttpStatusCode StatusCode { get; } = HttpStatusCode.BadRequest;
}
