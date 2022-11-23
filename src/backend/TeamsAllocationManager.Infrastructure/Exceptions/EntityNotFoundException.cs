using System;
using System.Net;
using TeamsAllocationManager.Domain;

namespace TeamsAllocationManager.Infrastructure.Exceptions;

public class EntityNotFoundException<TEntity> : ExceptionBase where TEntity : Entity
{
	public EntityNotFoundException(Guid id) : base($"Entity of type {typeof(TEntity).Name} with id {id} was not found") { }
	public EntityNotFoundException(string field, object value) : base($"Entity of type {typeof(TEntity).Name} with {field} equal to {value} was not found") { }
	public EntityNotFoundException(string message) : base(message) { }
	public EntityNotFoundException() : base($"Requested entity of type {typeof(TEntity).Name} was not found") { }

	public override int Code => 1;
	public override string Status => "entity_not_found";
	public override HttpStatusCode StatusCode => HttpStatusCode.NotFound;
}
