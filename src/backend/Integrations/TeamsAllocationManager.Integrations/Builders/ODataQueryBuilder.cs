using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace TeamsAllocationManager.Integrations.Builders;

public class ODataQueryBuilder : IODataQueryBuilder
{
	private readonly string _relativePath;
	private readonly Dictionary<string, string> _options = new Dictionary<string, string>();

	public ODataQueryBuilder(string relativePath)
	{
		_relativePath = relativePath;
	}

	public string Build()
	{
		if (_options.Any())
		{
			string optionsString = string.Join('&', _options.Select(entry => $"{entry.Key}={entry.Value}"));

			return $"{_relativePath}?{optionsString}";
		}

		return _relativePath;
	}

	public IODataQueryBuilder Expand(string value)
	{
		_options["$expand"] = value;

		return this;
	}

	public IODataQueryBuilder Expand(params string[] values)
	{
		_options["$expand"] = string.Join(',', values);

		return this;
	}

	public IODataQueryBuilder Filter(string value)
	{
		_options["$filter"] = value;

		return this;
	}

	public IODataQueryBuilder OrderBy(string value)
	{
		_options["$orderby"] = value;

		return this;
	}

	public IODataQueryBuilder Select<TEntity>() where TEntity : class
	{
		Type type = typeof(TEntity);
		PropertyInfo[] properties = type.GetProperties();

		_options["$select"] = string.Join(',', properties.Select(x => x.Name));

		return this;
	}

	public IODataQueryBuilder Select(params string[] values)
	{
		_options["$select"] = string.Join(',', values);

		return this;
	}

	public IODataQueryBuilder Select(string value)
	{
		_options["$select"] = value;

		return this;
	}

	public IODataQueryBuilder Skip(uint value)
	{
		_options["$skip"] = value.ToString();

		return this;
	}

	public IODataQueryBuilder Top(uint value)
	{
		_options["$top"] = value.ToString();

		return this;
	}
}
