using NUnit.Framework;
using Shouldly;
using System.Linq;
using TeamsAllocationManager.Database;
using TeamsAllocationManager.Domain.Enums;
using TeamsAllocationManager.Domain.Models;
using TeamsAllocationManager.Tests.Helpers;

namespace TeamsAllocationManager.Tests.Domain.Config;

[TestFixture]
public class DivisionsTests
{
	private readonly ApplicationDbContext _context;
	public DivisionsTests()
	{
		_context = TestsHelpers.CreateDbContextInMemory(this);
	}

	[SetUp]
	public void SetupBeforeEachTest()
	{
		_context.ClearDatabase();
	}

	[Test]
	public void ReadDivisions()
	{
		// given
		var config = ConfigEntity.CreateDivisionsConfigEntity();
		config.Data = @"{""123"":""Group A"",""222"":""Group B"",""333"":""Group C""}";
		_context.Configs.Add(config);
		_context.SaveChanges();

		// when
		var result = _context.Configs.Single(c => c.Key == DbConfigKey.Divisions).GetDivisions();

		result.Count.ShouldBe(3);
		result.Single(d => d.ExternalGroupId == 123).Name.ShouldBe("Group A");
		result.Single(d => d.ExternalGroupId == 222).Name.ShouldBe("Group B");
		result.Single(d => d.ExternalGroupId == 333).Name.ShouldBe("Group C");
	}
}
