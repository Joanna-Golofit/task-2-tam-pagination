using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TeamsAllocationManager.Domain.Enums;
using TeamsAllocationManager.Domain.Models.ConfigDataModels;

namespace TeamsAllocationManager.Domain.Models;

public class ConfigEntity : Entity
{
	public DbConfigKey Key { get; private set; }
	public string Data { get; set; } = null!;

	public static ConfigEntity CreateIgnoredProjectsConfigEntity()
	{
		return new ConfigEntity { Key = DbConfigKey.IgnoredProjects, Data = "" };
	}

	public static ConfigEntity CreateDivisionsConfigEntity()
	{
		return new ConfigEntity { Key = DbConfigKey.Divisions, Data = "" };
	}

	public void AddIgnoredProject(IgnoredProjectConfigData project)
	{
		if (Key != DbConfigKey.IgnoredProjects)
		{
			throw new Exception("Config data is not of IgnoredProjects data");
		}

		var list = this.GetIgnoredProjects();
		list.Add(project);
		Data = JsonConvert.SerializeObject(list.ToDictionary(p => p.ExternalId, p => p.Name));
	}

	public void RemoveIgnoredProject(int externalId)
	{
		if (Key != DbConfigKey.IgnoredProjects)
		{
			throw new Exception("Config data is not of IgnoredProjects data");
		}

		var list = this.GetIgnoredProjects().Where(p => p.ExternalId != externalId);
		Data = JsonConvert.SerializeObject(list.ToDictionary(p => p.ExternalId, p => p.Name));
	}

	public List<IgnoredProjectConfigData> GetIgnoredProjects()
	{
		if (Key != DbConfigKey.IgnoredProjects)
		{
			throw new Exception("Config data is not of IgnoredProjects data");
		}

		var data = JsonConvert.DeserializeObject<Dictionary<int, string>>(Data);

		return data == null ? new List<IgnoredProjectConfigData>() :
						data.Select(d => new IgnoredProjectConfigData { ExternalId = d.Key, Name = d.Value })
							.ToList();
	}

	public List<DivisionConfigData> GetDivisions()
	{
		if (Key != DbConfigKey.Divisions)
		{
			throw new Exception("Config data is not of Divisions data");
		}

		var data = JsonConvert.DeserializeObject<Dictionary<int, string>>(Data);

		return data == null ? new List<DivisionConfigData>() :
						data.Select(d => new DivisionConfigData { ExternalGroupId = d.Key, Name = d.Value })
							.ToList();
	}
}
