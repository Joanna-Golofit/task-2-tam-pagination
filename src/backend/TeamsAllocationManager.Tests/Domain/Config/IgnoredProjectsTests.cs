using NUnit.Framework;
using Shouldly;
using System;
using System.Linq;
using TeamsAllocationManager.Database;
using TeamsAllocationManager.Domain.Enums;
using TeamsAllocationManager.Domain.Models;
using TeamsAllocationManager.Domain.Models.ConfigDataModels;
using TeamsAllocationManager.Tests.Helpers;

namespace TeamsAllocationManager.Tests.Domain.Config;

[TestFixture]
public class IgnoredProjectsTests
{
	private readonly ApplicationDbContext _context;
	public IgnoredProjectsTests()
	{
		_context = TestsHelpers.CreateDbContextInMemory(this);
	}

	[SetUp]
	public void SetupBeforeEachTest()
	{
		_context.ClearDatabase();
	}

	[Test]
	public void ReadIgnoredProjects()
	{
		// given
		var config = ConfigEntity.CreateIgnoredProjectsConfigEntity();
		config.Data = @"{""123"":""Group A"",""222"":""Group B"",""333"":""Group C""}";
		_context.Configs.Add(config);
		_context.SaveChanges();

		// when
		var result = _context.Configs.Single(c => c.Key == DbConfigKey.IgnoredProjects).GetIgnoredProjects();

		result.Count.ShouldBe(3);
		result.Single(p => p.ExternalId == 123).Name.ShouldBe("Group A");
		result.Single(p => p.ExternalId == 222).Name.ShouldBe("Group B");
		result.Single(p => p.ExternalId == 333).Name.ShouldBe("Group C");
	}

	[Test]
	public void AddIgnoredProject()
	{
		// given
		var config = ConfigEntity.CreateIgnoredProjectsConfigEntity();
		config.Data = @"{""123"":""Group A"",""222"":""Group B"",""333"":""Group C""}";
		_context.Configs.Add(config);
		_context.SaveChanges();

		// when
		var configFromDb = _context.Configs.Single(c => c.Key == DbConfigKey.IgnoredProjects);
		configFromDb.AddIgnoredProject(new IgnoredProjectConfigData { ExternalId = 555, Name = "Group D" });
		_context.SaveChanges();

		var result = _context.Configs.Single(c => c.Key == DbConfigKey.IgnoredProjects).GetIgnoredProjects();

		result.Count.ShouldBe(4);
		result.Single(p => p.ExternalId == 123).Name.ShouldBe("Group A");
		result.Single(p => p.ExternalId == 222).Name.ShouldBe("Group B");
		result.Single(p => p.ExternalId == 333).Name.ShouldBe("Group C");
		result.Single(p => p.ExternalId == 555).Name.ShouldBe("Group D");
	}

	[Test]
	public void RemoveIgnoredProject()
	{
		// given
		var config = ConfigEntity.CreateIgnoredProjectsConfigEntity();
		config.Data = @"{""123"":""Group A"",""222"":""Group B"",""333"":""Group C""}";
		_context.Configs.Add(config);
		_context.SaveChanges();

		// when
		var configFromDb = _context.Configs.Single(c => c.Key == DbConfigKey.IgnoredProjects);
		configFromDb.RemoveIgnoredProject(222);
		_context.SaveChanges();

		var result = _context.Configs.Single(c => c.Key == DbConfigKey.IgnoredProjects).GetIgnoredProjects();

		result.Count.ShouldBe(2);
		result.Single(p => p.ExternalId == 123).Name.ShouldBe("Group A");
		result.Single(p => p.ExternalId == 333).Name.ShouldBe("Group C");
	}

	[Test]
	public void ShouldThrowOnAddDuplicate()
	{
		// given
		var config = ConfigEntity.CreateIgnoredProjectsConfigEntity();
		config.Data = @"{""123"":""Group A"",""222"":""Group B"",""333"":""Group C""}";
		_context.Configs.Add(config);
		_context.SaveChanges();

		// when
		var configFromDb = _context.Configs.Single(c => c.Key == DbConfigKey.IgnoredProjects);
		Should.Throw<Exception>(() => configFromDb.AddIgnoredProject(new IgnoredProjectConfigData { ExternalId = 123, Name = "Group D" }));
	}
}
