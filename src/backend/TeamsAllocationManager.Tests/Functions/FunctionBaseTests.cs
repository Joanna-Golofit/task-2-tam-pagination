using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Primitives;
using Moq;
using NUnit.Framework;
using Shouldly;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeamsAllocationManager.Api.Functions;
using TeamsAllocationManager.Contracts.Base;

namespace TeamsAllocationManager.Tests.Functions;

[TestFixture]
public class FunctionBaseTests
{
	private const string SimpleGetResult = "TestText";
	private const int SimplePostResult = 5;
	private Mock<IDispatcher> mockDispatcher = new Mock<IDispatcher>();

	[Test]
	public async Task ShouldExecuteSimpleGet()
	{
		var reqMock = new Mock<HttpRequest>();
		reqMock.Setup(r => r.Method).Returns("GET");
		reqMock.Setup(r => r.Query).Returns(new QueryCollection());
		reqMock.Setup(r => r.Body).Returns(new MemoryStream());

		var function = new TestFunctionImplementation(mockDispatcher.Object);

		var result = (await function.RunAsync(reqMock.Object, null, new Mock<ILogger>().Object)) as OkObjectResult;

		result?.StatusCode.ShouldBe(200);
		result?.Value.ShouldBe(SimpleGetResult);
	}

	[Test]
	public async Task ShouldExecuteGetByGuidParamType()
	{
		var reqMock = new Mock<HttpRequest>();
		reqMock.Setup(r => r.Method).Returns("GET");
		reqMock.Setup(r => r.Query).Returns(new QueryCollection());
		reqMock.Setup(r => r.Body).Returns(new MemoryStream());

		var expected = Guid.NewGuid();

		var function = new TestFunctionImplementation(mockDispatcher.Object);
		var result = (await function.RunAsync(reqMock.Object, $"{expected}", new Mock<ILogger>().Object)) as OkObjectResult;

		result?.StatusCode.ShouldBe(200);
		result?.Value.ShouldBe(expected);
	}

	[Test]
	public async Task ShouldExecuteSimplePost()
	{
		var reqMock = new Mock<HttpRequest>();
		reqMock.Setup(r => r.Method).Returns("Post");
		reqMock.Setup(r => r.Query).Returns(new QueryCollection());
		reqMock.Setup(r => r.Body).Returns(new MemoryStream());

		var function = new TestFunctionImplementation(mockDispatcher.Object);

		var result = (await function.RunAsync(reqMock.Object, null, new Mock<ILogger>().Object)) as OkObjectResult;

		result?.StatusCode.ShouldBe(200);
		result?.Value.ShouldBe(SimplePostResult);
	}

	[Test]
	public async Task ShouldExecuteDeleteWithParam()
	{
		var reqMock = new Mock<HttpRequest>();
		reqMock.Setup(r => r.Method).Returns("delete");
		reqMock.Setup(r => r.Query).Returns(new QueryCollection());
		reqMock.Setup(r => r.Body).Returns(new MemoryStream());

		const int expected = 6;

		var function = new TestFunctionImplementation(mockDispatcher.Object);

		var result = (await function.RunAsync(reqMock.Object, $"{expected}", new Mock<ILogger>().Object)) as OkObjectResult;

		result?.StatusCode.ShouldBe(200);
		result?.Value.ShouldBe(expected);
	}

	[Test]
	public async Task ShouldExecutePutWithOneParamFromPathAndOneFromBody()
	{
		var reqMock = new Mock<HttpRequest>();
		reqMock.Setup(r => r.Method).Returns("Put");
		reqMock.Setup(r => r.Query).Returns(new QueryCollection());
		reqMock.Setup(r => r.Body).Returns(new MemoryStream(Encoding.UTF8.GetBytes("{'Test1':5,'Test2':'ManaMana'}")));

		var function = new TestFunctionImplementation(mockDispatcher.Object);

		var result = (await function.RunAsync(reqMock.Object, "5", new Mock<ILogger>().Object)) as OkObjectResult;

		result?.StatusCode.ShouldBe(200);
		(result?.Value as TestItem1).ShouldNotBeNull();
		(result?.Value as TestItem1)?.Test1.ShouldBe(10);
		(result?.Value as TestItem1)?.Test2?.ShouldBe("ManaMana");
	}

	[Test]
	public async Task ShouldExecutePutWithAdditionalPathAndOneParamFromPathAndOneFromBody()
	{
		var reqMock = new Mock<HttpRequest>();
		reqMock.Setup(r => r.Method).Returns("Put");
		reqMock.Setup(r => r.Query).Returns(new QueryCollection());
		reqMock.Setup(r => r.Body).Returns(new MemoryStream(Encoding.UTF8.GetBytes("{'Test1':5,'Test2':'ManaMana'}")));

		var function = new TestFunctionImplementation(mockDispatcher.Object);

		var result = (await function.RunAsync(reqMock.Object, "TestPath/5", new Mock<ILogger>().Object)) as OkObjectResult;

		result?.StatusCode.ShouldBe(200);
		(result?.Value as TestItem1).ShouldNotBeNull();
		(result?.Value as TestItem1)?.Test1.ShouldBe(12);
		(result?.Value as TestItem1)?.Test2?.ShouldBe("ManaMana");
	}

	[Test]
	public async Task ShouldExecutePutWithOneParamAndAdditionalPathFromPathAndOneFromBody()
	{
		var reqMock = new Mock<HttpRequest>();
		reqMock.Setup(r => r.Method).Returns("Put");
		reqMock.Setup(r => r.Query).Returns(new QueryCollection());
		reqMock.Setup(r => r.Body).Returns(new MemoryStream(Encoding.UTF8.GetBytes("{'Test1':5,'Test2':'ManaMana'}")));

		var function = new TestFunctionImplementation(mockDispatcher.Object);

		var result = (await function.RunAsync(reqMock.Object, "5/TestPath", new Mock<ILogger>().Object)) as OkObjectResult;

		result?.StatusCode.ShouldBe(200);
		(result?.Value as TestItem1).ShouldNotBeNull();
		(result?.Value as TestItem1)?.Test1.ShouldBe(14);
		(result?.Value as TestItem1)?.Test2?.ShouldBe("ManaMana");
	}

	[Test]
	public async Task ShouldExecutePutWithOneGuidParamFromPathAndOneFromBody()
	{
		var reqMock = new Mock<HttpRequest>();
		reqMock.Setup(r => r.Method).Returns("Put");
		reqMock.Setup(r => r.Query).Returns(new QueryCollection());
		reqMock.Setup(r => r.Body)
			.Returns(new MemoryStream(Encoding.UTF8.GetBytes("{'Test1':5,'Test2':'ceecfa8d-d91f-4b37-8833-846ecd889200','Test3':'ceecfa8d-d91f-4b37-8833-846ecd889200'}")));

		var function = new TestFunctionImplementation(mockDispatcher.Object);

		var result = (await function.RunAsync(reqMock.Object, "64888d21-d6ac-4508-44fd-08d79f53cfa4", new Mock<ILogger>().Object)) as OkObjectResult;

		result?.StatusCode.ShouldBe(200);
		(result?.Value as TestItem3).ShouldNotBeNull();
		(result?.Value as TestItem3)?.Test1.ShouldBe(6);
		(result?.Value as TestItem3)?.Test2.ShouldBe(new Guid("ceecfa8d-d91f-4b37-8833-846ecd889200"));
		(result?.Value as TestItem3)?.Test3.ShouldBe(new Guid("64888d21-d6ac-4508-44fd-08d79f53cfa4"));
	}

	[Test]
	public async Task ShouldExecutePutWithAdditionalPathAndOneGuidParamFromPathAndOneFromBody()
	{
		var reqMock = new Mock<HttpRequest>();
		reqMock.Setup(r => r.Method).Returns("Put");
		reqMock.Setup(r => r.Query).Returns(new QueryCollection());
		reqMock.Setup(r => r.Body)
			.Returns(new MemoryStream(Encoding.UTF8.GetBytes("{'Test1':5,'Test2':'ceecfa8d-d91f-4b37-8833-846ecd889200','Test3':'ceecfa8d-d91f-4b37-8833-846ecd889200'}")));

		var function = new TestFunctionImplementation(mockDispatcher.Object);

		var result = (await function.RunAsync(reqMock.Object, "TestPath/64888d21-d6ac-4508-44fd-08d79f53cfa4", new Mock<ILogger>().Object)) as OkObjectResult;

		result?.StatusCode.ShouldBe(200);
		(result?.Value as TestItem3).ShouldNotBeNull();
		(result?.Value as TestItem3)?.Test1.ShouldBe(7);
		(result?.Value as TestItem3)?.Test2.ShouldBe(new Guid("ceecfa8d-d91f-4b37-8833-846ecd889200"));
		(result?.Value as TestItem3)?.Test3.ShouldBe(new Guid("64888d21-d6ac-4508-44fd-08d79f53cfa4"));
	}

	[Test]
	public async Task ShouldExecutePutWithOneGuidParamFromPathAndAdditionalPathAndOneParamFromBody()
	{
		var reqMock = new Mock<HttpRequest>();
		reqMock.Setup(r => r.Method).Returns("Put");
		reqMock.Setup(r => r.Query).Returns(new QueryCollection());
		reqMock.Setup(r => r.Body)
			.Returns(new MemoryStream(Encoding.UTF8.GetBytes("{'Test1':5,'Test2':'ceecfa8d-d91f-4b37-8833-846ecd889200','Test3':'ceecfa8d-d91f-4b37-8833-846ecd889200'}")));

		var function = new TestFunctionImplementation(mockDispatcher.Object);

		var result = (await function.RunAsync(reqMock.Object, "64888d21-d6ac-4508-44fd-08d79f53cfa4/TestPath", new Mock<ILogger>().Object)) as OkObjectResult;

		result?.StatusCode.ShouldBe(200);
		(result?.Value as TestItem3).ShouldNotBeNull();
		(result?.Value as TestItem3)?.Test1.ShouldBe(9);
		(result?.Value as TestItem3)?.Test2.ShouldBe(new Guid("ceecfa8d-d91f-4b37-8833-846ecd889200"));
		(result?.Value as TestItem3)?.Test3.ShouldBe(new Guid("64888d21-d6ac-4508-44fd-08d79f53cfa4"));
	}

	[Test]
	public async Task ShouldExecutePutWithOneGuidParamFromPathAndAdditionalPathAndOneParamFromBodyAsInterface()
	{
		var reqMock = new Mock<HttpRequest>();
		reqMock.Setup(r => r.Method).Returns("Put");
		reqMock.Setup(r => r.Query).Returns(new QueryCollection());
		reqMock.Setup(r => r.Body)
			.Returns(new MemoryStream(Encoding.UTF8.GetBytes("['ceecfa8d-d91f-4b37-8833-846ecd889200','ceecfa8d-d91f-4b37-8833-846ecd889200']")));

		var function = new TestFunctionImplementation(mockDispatcher.Object);

		var result = (await function.RunAsync(reqMock.Object, "64888d21-d6ac-4508-44fd-08d79f53cfa4/TestPath2", new Mock<ILogger>().Object)) as OkObjectResult;

		result?.StatusCode.ShouldBe(200);
		(result!.Value as IEnumerable<Guid>).ShouldNotBeNull();
		((result.Value! as IEnumerable<Guid>)!).Count().ShouldBe(2);
		((result.Value! as IEnumerable<Guid>)!).ToList()[0].ShouldBe(new Guid("64888d21-d6ac-4508-44fd-08d79f53cfa4"));
		((result.Value! as IEnumerable<Guid>)!).ToList()[1].ShouldBe(new Guid("ceecfa8d-d91f-4b37-8833-846ecd889200"));
	}

	[Test]
	public async Task ShouldExecuteostWithTwoPathParams()
	{
		var reqMock = new Mock<HttpRequest>();
		reqMock.Setup(r => r.Method).Returns("Post");
		reqMock.Setup(r => r.Query).Returns(new QueryCollection());
		reqMock.Setup(r => r.Body).Returns(new MemoryStream());

		var function = new TestFunctionImplementation(mockDispatcher.Object);

		var result = (await function.RunAsync(reqMock.Object, "5/6", new Mock<ILogger>().Object)) as OkObjectResult;

		result?.StatusCode.ShouldBe(200);
		result?.Value.ShouldBe("56");
	}

	[Test]
	public async Task ShouldExecutePostWithOneParamFromPathAndOneFromBody()
	{
		var reqMock = new Mock<HttpRequest>();
		reqMock.Setup(r => r.Method).Returns("Post");
		reqMock.Setup(r => r.Query).Returns(new QueryCollection());
		reqMock.Setup(r => r.Body).Returns(new MemoryStream(Encoding.UTF8.GetBytes("{'Test1':5,'Test2':'ManaMana'}")));

		var function = new TestFunctionImplementation(mockDispatcher.Object);

		var result = (await function.RunAsync(reqMock.Object, "5", new Mock<ILogger>().Object)) as OkObjectResult;

		result?.StatusCode.ShouldBe(200);
		(result?.Value as TestItem1)?.Test1.ShouldBe(10);
		(result?.Value as TestItem1)?.Test2?.ShouldBe("ManaMana");
	}

	[Test]
	public async Task ShouldExecutePostWithOneParamFromPathAndOneFromBody2()
	{
		var reqMock = new Mock<HttpRequest>();
		reqMock.Setup(r => r.Method).Returns("Post");
		reqMock.Setup(r => r.Query).Returns(new QueryCollection());
		reqMock.Setup(r => r.Body).Returns(new MemoryStream(Encoding.UTF8.GetBytes("{'Test4':5,'Test3':'ManaMana'}")));

		var function = new TestFunctionImplementation(mockDispatcher.Object);

		var result = (await function.RunAsync(reqMock.Object, "5", new Mock<ILogger>().Object)) as OkObjectResult;

		result?.StatusCode.ShouldBe(200);
		(result?.Value as TestItem2)?.Test4.ShouldBe(10);
		(result?.Value as TestItem2)?.Test3?.ShouldBe("ManaMana");
	}

	[Test]
	public async Task ShouldExecuteGetWithQuery()
	{
		var queryDictionary = new Dictionary<string, StringValues>
		{
			["q1"] = new StringValues("1"),
			["q2"] = new StringValues("Test"),

		};
		var reqMock = new Mock<HttpRequest>();
		reqMock.Setup(r => r.Method).Returns("GET");
		reqMock.Setup(r => r.Query).Returns(new QueryCollection(queryDictionary));
		reqMock.Setup(r => r.Body).Returns(new MemoryStream());

		var function = new TestFunctionImplementation(mockDispatcher.Object);
		var result = (await function.RunAsync(reqMock.Object, "", new Mock<ILogger>().Object)) as OkObjectResult;

		result?.StatusCode.ShouldBe(200);
		result?.Value.ShouldBe("1 Test");
	}

	[Test]
	public async Task ShouldExecuteGetWithUninitializedBody()
	{
		var queryDictionary = new Dictionary<string, StringValues>
		{
			["q1"] = new StringValues("1"),
			["q2"] = new StringValues("Test"),

		};
		var reqMock = new Mock<HttpRequest>();
		reqMock.Setup(r => r.Method).Returns("GET");
		reqMock.Setup(r => r.Query).Returns(new QueryCollection(queryDictionary));
		reqMock.Setup(r => r.Body).Returns<MemoryStream>(null);

		var function = new TestFunctionImplementation(mockDispatcher.Object);
		var result = (await function.RunAsync(reqMock.Object, "", new Mock<ILogger>().Object)) as OkObjectResult;

		result?.StatusCode.ShouldBe(200);
		result?.Value.ShouldBe("1 Test");
	}

	[Test]
	public async Task ShouldExecuteGetReturningTask()
	{
		var reqMock = new Mock<HttpRequest>();
		reqMock.Setup(r => r.Method).Returns("GET");
		reqMock.Setup(r => r.Query).Returns(new QueryCollection());
		reqMock.Setup(r => r.Body).Returns(new MemoryStream());

		var function = new TestFunctionImplementation(mockDispatcher.Object);
		var result = (await function.RunAsync(reqMock.Object, "TestPath", new Mock<ILogger>().Object)) as OkResult;

		result?.StatusCode.ShouldBe(200);
	}

	[Test]
	public async Task ShouldExecuteMethodWithMoreParameters()
	{
		var reqMock = new Mock<HttpRequest>();
		reqMock.Setup(r => r.Method).Returns("GET");
		reqMock.Setup(r => r.Query).Returns(new QueryCollection());
		reqMock.Setup(r => r.Body).Returns(new MemoryStream());

		var function = new TestFunctionImplementation(mockDispatcher.Object);
		var result = (await function.RunAsync(reqMock.Object, "TestPath/Test", new Mock<ILogger>().Object)) as OkObjectResult;

		result?.StatusCode.ShouldBe(200);
		result?.Value.ShouldBe("Test");
	}

	[Test]
	public async Task ShouldReturnError()
	{
		var reqMock = new Mock<HttpRequest>();
		reqMock.Setup(r => r.Method).Returns("GET");
		reqMock.Setup(r => r.Query).Returns(new QueryCollection());
		reqMock.Setup(r => r.Body).Returns(new MemoryStream());

		var function = new TestFunctionImplementation(mockDispatcher.Object);
		var result = (await function.RunAsync(reqMock.Object, "DuplicatedPath/5", new Mock<ILogger>().Object)) as BadRequestObjectResult;

		result?.StatusCode.ShouldBe(400);
		result?.Value.ShouldBe("Found 2 handlers for this request. Make sure, there is only one possible handler.");
	}

	[Test]
	public async Task ShouldExecuteMethodWithCorrectParamType()
	{
		var reqMock = new Mock<HttpRequest>();
		reqMock.Setup(r => r.Method).Returns("GET");
		reqMock.Setup(r => r.Query).Returns(new QueryCollection());
		reqMock.Setup(r => r.Body).Returns(new MemoryStream());

		const string expected = "SomeText";

		var function = new TestFunctionImplementation(mockDispatcher.Object);
		var result = (await function.RunAsync(reqMock.Object, $"DuplicatedPath/{expected}", new Mock<ILogger>().Object)) as OkObjectResult;

		result?.StatusCode.ShouldBe(200);
		result?.Value.ShouldBe(expected);
	}
}

internal class TestFunctionImplementation : FunctionBase
{
	public TestFunctionImplementation(IDispatcher dispatcher) : base(dispatcher) { }

	public string Get()
		=> "TestText";

	[HttpGet("{i}")]
	public async Task<Guid> GetGuid(Guid guid)
		=> await Task.FromResult(guid);

	[HttpPost]
	public async Task<int> MakeMyDreamComeTrue()
		=> await Task.FromResult(5);

	[HttpDelete("{i}")]
	public async Task<int> MakeMyDreamComeTrue(int i)
		=> await Task.FromResult(i);

	[HttpPut("{i}")]
	public async Task<TestItem1> MakeMyDreamComeTrue(int i, TestItem1 item)
	{
		item.Test1 += i;

		return await Task.FromResult(item);
	}

	[HttpPut("TestPath/{i}")]
	public async Task<TestItem1> MakeMyDreamComeTrue2(int i, TestItem1 item)
	{
		item.Test1 += i + 2;

		return await Task.FromResult(item);
	}

	[HttpPut("{i}/TestPath")]
	public async Task<TestItem1> MakeMyDreamComeTrue3(int i, TestItem1 item)
	{
		item.Test1 += i + 4;

		return await Task.FromResult(item);
	}

	[HttpPut("{id}")]
	public async Task<TestItem3> MakeMyDreamComeTrue(Guid id, TestItem3 item)
	{
		item.Test1++;
		item.Test3 = id;

		return await Task.FromResult(item);
	}

	[HttpPut("TestPath/{id}")]
	public async Task<TestItem3> MakeMyDreamComeTrue2(Guid id, TestItem3 item)
	{
		item.Test1 += 2;
		item.Test3 = id;

		return await Task.FromResult(item);
	}

	[HttpPut("{id}/TestPath")]
	public async Task<TestItem3> MakeMyDreamComeTrue3(Guid id, TestItem3 item)
	{
		item.Test1 += 4;
		item.Test3 = id;

		return await Task.FromResult(item);
	}

	[HttpPut("{id}/TestPath2")]
	public async Task<IEnumerable<Guid>> MakeMyDreamComeTrue3(Guid id, IEnumerable<Guid> item)
	{
		return await Task.FromResult(new List<Guid> { id, item.ToList()[1] });
	}

	[HttpPost("{i}/{j}")]
	public IActionResult MakeMyDay(int i, string j)
		=> new OkObjectResult(i + j);

	[HttpPost("{i}")]
	public async Task<TestItem1> DoIt(int i, TestItem1 item)
	{
		item.Test1 += i;

		return await Task.FromResult(item);
	}

	[HttpPost("{i}")]
	public async Task<TestItem2> DoIt(int i, TestItem2 item)
	{
		item.Test3 += i;

		return await Task.FromResult(item);
	}

	[HttpGet("TestPath")]
	public Task WithPath()
	{
		return Task.CompletedTask;
	}

	[HttpGet("TestPath/{i}")]
	public Task<string> CoveredPath(string i)
	{
		return Task.FromResult(i);
	}

	[HttpGet("TestPath/Test")]
	public Task<string> CoveredPath()
	{
		return Task.FromResult("Parameterless");
	}

	[HttpGet("DuplicatedPath/{i}")]
	public Task<string> DuplicatedPath(float i)
	{
		return Task.FromResult($"{i}");
	}

	[HttpGet("DuplicatedPath/{i}")]
	public Task<string> DuplicatedPath(string i)
	{
		return Task.FromResult($"{i}");
	}

	public async Task<string> GetWithQuery(int q1, string q2)
		=> await Task.FromResult($"{q1} {q2}");
}

internal class TestItem1
{
	public int Test1 { get; set; }
	public string Test2 { get; set; } = null!;
}

internal class TestItem2
{
	public int Test4 { get; set; }
	public string Test3 { get; set; } = null!;
}

internal class TestItem3
{
	public int Test1 { get; set; }
	public Guid Test2 { get; set; }
	public Guid Test3 { get; set; }
}
