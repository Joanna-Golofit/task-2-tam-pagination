using NUnit.Framework;
using Shouldly;
using TeamsAllocationManager.Integrations.Builders;

namespace TeamsAllocationManager.Tests.ApiIntegrations;

public class ODataQueryBuilderTests
{
	private const string RelativePath = "RelativePath";
	private IODataQueryBuilder _builder = null!;

	[SetUp]
	public void SetupBeforeEachTest()
	{
		_builder = new ODataQueryBuilder(RelativePath);
	}

	[TestCase("")]
	[TestCase("expand_test")]
	public void Expand_SingleStringValue_AppendsParameter(string value)
	{
		string expected = $"{RelativePath}?$expand={value}";

		string result = _builder
			.Expand(value)
			.Build();

		result.ShouldBe(expected);
	}

	[Test]
	public void Expand_NoValues_AppendsParameter()
	{
		string expected = $"{RelativePath}?$expand=";

		string result = _builder
			.Expand()
			.Build();

		result.ShouldBe(expected);
	}

	[Test]
	public void Expand_MultipleStringValues_AppendsParameter()
	{
		string param1 = "test1";
		string param2 = "test2";
		string param3 = "test3";
		string expected = $"{RelativePath}?$expand={param1},{param2},{param3}";

		string result = _builder
			.Expand(param1, param2, param3)
			.Build();

		result.ShouldBe(expected);
	}

	[Test]
	public void Filter_SingleStringValue_AppendsParameter()
	{
		string value = "filter_test";
		string expected = $"{RelativePath}?$filter={value}";

		string result = _builder
			.Filter(value)
			.Build();

		result.ShouldBe(expected);
	}

	[Test]
	public void OrderBy_SingleStringValue_AppendsParameter()
	{
		string value = "orderby_test";
		string expected = $"{RelativePath}?$orderby={value}";

		string result = _builder
			.OrderBy(value)
			.Build();

		result.ShouldBe(expected);
	}

	[Test]
	public void Select_TypeParameter_AppendsParameter()
	{
		string expected = $"{RelativePath}?$select=Property1,Property2,Property3,Property4";

		string result = _builder
			.Select<TestSelectClass>()
			.Build();

		result.ShouldBe(expected);
	}

	[TestCase("")]
	[TestCase("select_test")]
	public void Select_SingleStringValue_AppendsParameter(string value)
	{
		string expected = $"{RelativePath}?$select={value}";

		string result = _builder
			.Select(value)
			.Build();

		result.ShouldBe(expected);
	}

	[Test]
	public void Select_NoValues_AppendsParameter()
	{
		string expected = $"{RelativePath}?$select=";

		string result = _builder
			.Select()
			.Build();

		result.ShouldBe(expected);
	}

	[Test]
	public void Select_MultipleStringValues_AppendsParameter()
	{
		string param1 = "test1";
		string param2 = "test2";
		string param3 = "test3";
		string expected = $"{RelativePath}?$select={param1},{param2},{param3}";

		string result = _builder
			.Select(param1, param2, param3)
			.Build();

		result.ShouldBe(expected);
	}

	[Test]
	public void Skip_SingleUintValue_AppendsParameter()
	{
		uint value = 142;
		string expected = $"{RelativePath}?$skip={value}";

		string result = _builder
			.Skip(value)
			.Build();

		result.ShouldBe(expected);
	}

	[Test]
	public void Top_SingleUintValue_AppendsParameter()
	{
		uint value = 142;
		string expected = $"{RelativePath}?$top={value}";

		string result = _builder
			.Top(value)
			.Build();

		result.ShouldBe(expected);
	}

	[Test]
	public void Build_NoParameters_ReturnsSamePath()
	{
		string expected = RelativePath;

		string result = _builder.Build();
		result.ShouldBe(expected);
	}

	[Test]
	public void Build_MultipleParameters_ReturnsPathWithParameters()
	{
		string selectOption = "select1";
		string expandOption = "expand1";
		string orderByOption = "orderby1";
		string filterOption = "filter1";
		uint skipOption = 13;
		uint topOption = 175;

		string expected = $"{RelativePath}?$select={selectOption}" +
			$"&$expand={expandOption}" +
			$"&$orderby={orderByOption}" +
			$"&$filter={filterOption}" +
			$"&$skip={skipOption}" +
			$"&$top={topOption}";

		string result = _builder
			.Select(selectOption)
			.Expand(expandOption)
			.OrderBy(orderByOption)
			.Filter(filterOption)
			.Skip(skipOption)
			.Top(topOption)
			.Build();


		result.ShouldBe(expected);
	}

	public class TestSelectClass
	{
		public int Property1 { get; set; }

		public string Property2 { get; set; } = null!;

		public System.DateTime? Property3 { get; set; }

		public TestDerivedSelectClass Property4 { get; set; } = null!;

		public const string Constant = "testConst";

		public int Method() => 12;

		public class TestDerivedSelectClass
		{
			public int DerivedProperty1 { get; set; }
		}
	}
}
