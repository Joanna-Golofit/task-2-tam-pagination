using System;

namespace TeamsAllocationManager.Integrations.Exceptions;

public class ApiClientException : Exception
{
	public ApiClientException(string? message)
		: base(message)
	{
	}
}
