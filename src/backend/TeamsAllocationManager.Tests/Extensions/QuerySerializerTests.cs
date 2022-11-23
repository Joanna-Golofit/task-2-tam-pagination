using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using NUnit.Framework;
using Shouldly;
using System.Collections.Generic;
using TeamsAllocationManager.Infrastructure.Extensions;

namespace TeamsAllocationManager.Tests.Extensions;

[TestFixture]
public class QuerySerializerTests
{
	[Test]
	public void Deserialize_Primitives_DeserializesParamsSuccessfully()
	{
		// given
		string numberValue = "10";
		string textValue = "napis";
		string logicValue = "true";

		IQueryCollection query = new QueryCollection(new Dictionary<string, StringValues>
		{
			{ "number", numberValue },
			{ "text", textValue },
			{ "logic", logicValue }
		});

		// when
		int numberResult = (int)QuerySerializer.Deserialize(typeof(int), query, parameterName: "number")!;
		string textResult = (string)QuerySerializer.Deserialize(typeof(string), query, parameterName: "text")!;
		bool logicResult = (bool)QuerySerializer.Deserialize(typeof(bool), query, parameterName: "logic")!;

		// then
		numberResult.ShouldBe(10);
		textResult.ShouldBe(textValue);
		logicResult.ShouldBe(true);
	}

	[Test]
	public void Deserialize_NullablePrimitivesWithValues_DeserializesParamsSuccessfully()
	{
		// given
		string numberValue = "10";
		string logicValue = "true";

		IQueryCollection query = new QueryCollection(new Dictionary<string, StringValues>
		{
			{ "number", numberValue },
			{ "logic", logicValue }
		});

		// when
		int? numberResult = (int?)QuerySerializer.Deserialize(typeof(int?), query, parameterName: "number");
		bool? logicResult = (bool?)QuerySerializer.Deserialize(typeof(bool?), query, parameterName: "logic");

		// then
		numberResult.ShouldBe(10);
		logicResult.ShouldBe(true);
	}

	[Test]
	public void Deserialize_PrimitivesIncorrectParams_ReturnsDefaultValues()
	{
		// given
		string numberValue = "text1";
		string logicValue = "text2";

		IQueryCollection query = new QueryCollection(new Dictionary<string, StringValues>
		{
			{ "number", numberValue },
			{ "logic", logicValue }
		});

		// when
		int numberResult = (int)QuerySerializer.Deserialize(typeof(int), query, parameterName: "number")!;
		bool logicResult = (bool)QuerySerializer.Deserialize(typeof(bool), query, parameterName: "logic")!;

		// then
		numberResult.ShouldBe(0);
		logicResult.ShouldBeFalse();
	}

	[Test]
	public void Deserialize_NullablePrimitivesIncorrectParams_ReturnsNullValues()
	{
		// given
		string numberValue = "text1";
		string logicValue = "text2";

		IQueryCollection query = new QueryCollection(new Dictionary<string, StringValues>
		{
			{ "number", numberValue },
			{ "logic", logicValue }
		});

		// when
		int? numberResult = (int?)QuerySerializer.Deserialize(typeof(int?), query, parameterName: "number");
		bool? logicResult = (bool?)QuerySerializer.Deserialize(typeof(bool?), query, parameterName: "logic");

		// then
		numberResult.ShouldBeNull();
		logicResult.ShouldBeNull();
	}

	[Test]
	public void Deserialize_PrimitivesEmptyValues_ReturnsDefaultValues()
	{
		// given
		string numberValue = "";
		string textValue = "";
		string logicValue = "";

		IQueryCollection query = new QueryCollection(new Dictionary<string, StringValues>
		{
			{ "number", numberValue },
			{ "text", textValue },
			{ "logic", logicValue }
		});

		// when
		int numberResult = (int)QuerySerializer.Deserialize(typeof(int), query, parameterName: "number")!;
		string textResult = (string)QuerySerializer.Deserialize(typeof(string), query, parameterName: "text")!;
		bool logicResult = (bool)QuerySerializer.Deserialize(typeof(bool), query, parameterName: "logic")!;

		// then
		numberResult.ShouldBe(0);
		textResult.ShouldBe(string.Empty);
		logicResult.ShouldBeFalse();
	}

	[Test]
	public void Deserialize_NullablePrimitivesEmptyValues_ReturnsNullValues()
	{
		// given
		string numberValue = "";
		string textValue = "";
		string logicValue = "";

		IQueryCollection query = new QueryCollection(new Dictionary<string, StringValues>
		{
			{ "number", numberValue },
			{ "text", textValue },
			{ "logic", logicValue }
		});

		// when
		int? numberResult = (int?)QuerySerializer.Deserialize(typeof(int?), query, parameterName: "number")!;
		bool? logicResult = (bool?)QuerySerializer.Deserialize(typeof(bool?), query, parameterName: "logic")!;

		// then
		numberResult.ShouldBeNull();
		logicResult.ShouldBeNull();
	}

	[Test]
	public void Deserialize_PrimitivesMissingParams_ReturnsDefaultValues()
	{
		// given
		IQueryCollection query = new QueryCollection();

		// when
		int numberResult = (int)QuerySerializer.Deserialize(typeof(int), query, parameterName: "number")!;
		string textResult = (string)QuerySerializer.Deserialize(typeof(string), query, parameterName: "text")!;
		bool logicResult = (bool)QuerySerializer.Deserialize(typeof(bool), query, parameterName: "logic")!;

		// then
		numberResult.ShouldBe(0);
		textResult.ShouldBeNull();
		logicResult.ShouldBeFalse();
	}

	[Test]
	public void Deserialize_NullablePrimitivesMissingParams_ReturnsNullValues()
	{
		// given
		IQueryCollection query = new QueryCollection();

		// when
		int? numberResult = (int?)QuerySerializer.Deserialize(typeof(int?), query, parameterName: "number")!;
		bool? logicResult = (bool?)QuerySerializer.Deserialize(typeof(bool?), query, parameterName: "logic")!;

		// then
		numberResult.ShouldBeNull();
		logicResult.ShouldBeNull();
	}

	[Test]
	public void Deserialize_SimpleObject_DeserializesObjectSuccessfully()
	{
		// given
		string numberValue = "10";
		string textValue = "napis";
		string logicValue = "true";

		IQueryCollection query = new QueryCollection(new Dictionary<string, StringValues>
		{
			{ "number", numberValue },
			{ "text", textValue },
			{ "logic", logicValue }
		});

		// when
		var result = (TestSimpleClass)QuerySerializer.Deserialize(typeof(TestSimpleClass), query)!;

		// then
		result.Number.ShouldBe(10);
		result.Text.ShouldBe(textValue);
		result.Logic.ShouldBe(true);
	}

	[Test]
	public void Deserialize_IncorrectDataType_SkipsPropertyDeserialization()
	{
		// given
		string numberValue = "NaN";
		string textValue = "napis";
		IQueryCollection query = new QueryCollection(new Dictionary<string, StringValues>
		{
			{ "number", numberValue },
			{ "text", textValue }
		});

		// when
		var result = (TestSimpleClass)QuerySerializer.Deserialize(typeof(TestSimpleClass), query)!;

		// then
		result.Number.ShouldBe(default);
		result.Text.ShouldBe(textValue);
	}

	[Test]
	public void Deserialize_SomePropertiesMissing_LeavesDefaultValues()
	{
		// given
		IQueryCollection query = new QueryCollection();

		// when
		var result = (TestSimpleClass)QuerySerializer.Deserialize(typeof(TestSimpleClass), query)!;

		// then
		result.Number.ShouldBe(default);
		result.Text.ShouldBeNull();
	}

	[Test]
	public void Deserialize_AdditionalQueryParameters_SkipsUnknownParameters()
	{
		// given
		string numberValue = "10";
		string textValue = "napis";
		IQueryCollection query = new QueryCollection(new Dictionary<string, StringValues>
		{
			{ "number", numberValue },
			{ "text", textValue },
			{ "extraParam1", "wow" },
			{ "extraParam2", "13" }
		});

		// when
		var result = (TestSimpleClass)QuerySerializer.Deserialize(typeof(TestSimpleClass), query)!;

		// then
		result.Number.ShouldBe(10);
		result.Text.ShouldBe(textValue);
	}

	[Test]
	public void Deserialize_ParametersContainingArrays_DeserializesObjectSuccessfully()
	{
		// given
		string idValue = "10";
		string strings = "napis1,napis2,trzeci";
		string ints = "1,3,7,8,9";

		IQueryCollection query = new QueryCollection(new Dictionary<string, StringValues>
		{
			{ "id", idValue },
			{ "stringCollection", strings },
			{ "intCollection", ints }
		});

		// when
		var result = (TestClassWithCollection)QuerySerializer.Deserialize(typeof(TestClassWithCollection), query)!;

		// then
		result.Id.ShouldBe(10);
		result.StringCollection!.Count.ShouldBe(3);
		result.IntCollection.Count.ShouldBe(5);
	}

	[Test]
	public void Deserialize_ArraysWithIncorrectValues_DeserializesCorrectItemsOnly()
	{
		// given
		string idValue = "10";
		string strings = "";
		string ints = "1,3,wrong,false";

		IQueryCollection query = new QueryCollection(new Dictionary<string, StringValues>
		{
			{ "id", idValue },
			{ "stringCollection", strings },
			{ "intCollection", ints }
		});

		// when
		var result = (TestClassWithCollection)QuerySerializer.Deserialize(typeof(TestClassWithCollection), query)!;

		// then
		result.Id.ShouldBe(10);

		result.StringCollection!.Count.ShouldBe(1);
		result.StringCollection!.ShouldContain(string.Empty);

		result.IntCollection.Count.ShouldBe(2);
		result.IntCollection.ShouldContain(1);
		result.IntCollection.ShouldContain(3);
	}

	[Test]
	public void Deserialize_SomeArrayParametersMissing_ReturnsDefaultValues()
	{
		// given
		string idValue = "10";

		IQueryCollection query = new QueryCollection(new Dictionary<string, StringValues>
		{
			{ "id", idValue }
		});

		// when
		var result = (TestClassWithCollection)QuerySerializer.Deserialize(typeof(TestClassWithCollection), query)!;

		// then
		result.Id.ShouldBe(10);
		result.StringCollection.ShouldBeEmpty();
		result.IntCollection.ShouldBeEmpty();
	}

	[Test]
	public void Deserialize_ComplexObject_DeserializesObjectSuccessfully()
	{
		// given
		string idValue = "14";
		string simpleNumberValue = "12";
		string simpleTextValue = "napis";
		string collectionsIdValue = "9";
		string collectionsStrings = "napis1,napis2,trzeci";
		string collectionsInts = "1,3,7,8,9";

		IQueryCollection query = new QueryCollection(new Dictionary<string, StringValues>
		{
			{ "id", idValue },
			{ "simple.number", simpleNumberValue },
			{ "simple.text", simpleTextValue },
			{ "collections.id", collectionsIdValue },
			{ "collections.stringCollection", collectionsStrings },
			{ "collections.intCollection", collectionsInts },
		});

		// when
		var result = (TestComplexClass)QuerySerializer.Deserialize(typeof(TestComplexClass), query)!;

		// then
		result.Id.ShouldBe(14);
		result.Simple!.Number.ShouldBe(12);
		result.Simple!.Text.ShouldBe(simpleTextValue);
		result.Collections!.Id.ShouldBe(9);

		result.Collections!.StringCollection!.Count.ShouldBe(3);
		result.Collections!.StringCollection!.ShouldContain("napis1");
		result.Collections!.StringCollection!.ShouldContain("napis2");
		result.Collections!.StringCollection!.ShouldContain("trzeci");

		result.Collections!.IntCollection.Count.ShouldBe(5);
		result.Collections!.IntCollection.ShouldContain(1);
		result.Collections!.IntCollection.ShouldContain(3);
		result.Collections!.IntCollection.ShouldContain(7);
		result.Collections!.IntCollection.ShouldContain(8);
		result.Collections!.IntCollection.ShouldContain(9);
	}

	private class TestSimpleClass
	{
		public int Number { get; set; }

		public string? Text { get; set; }

		public bool Logic { get; set; }
	}

	private class TestClassWithCollection
	{
		public int Id { get; set; }

		public ICollection<string>? StringCollection { get; set; }

		public ICollection<int> IntCollection { get; set; } = new List<int>();
	}

	private class TestComplexClass
	{
		public int Id { get; set; }

		public TestSimpleClass? Simple { get; set; }

		public TestClassWithCollection? Collections { get; set; }
	}
}
