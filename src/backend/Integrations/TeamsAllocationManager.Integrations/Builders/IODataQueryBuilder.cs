namespace TeamsAllocationManager.Integrations.Builders;

public interface IODataQueryBuilder
{
	string Build();
	IODataQueryBuilder Expand(string value);
	IODataQueryBuilder Expand(params string[] values);
	IODataQueryBuilder Filter(string value);
	IODataQueryBuilder OrderBy(string value);
	IODataQueryBuilder Select<TEntity>() where TEntity : class;
	IODataQueryBuilder Select(params string[] values);
	IODataQueryBuilder Select(string value);
	IODataQueryBuilder Skip(uint count);
	IODataQueryBuilder Top(uint count);
}
