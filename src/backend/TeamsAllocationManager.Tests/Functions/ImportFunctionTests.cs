using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using System;
using System.IO;
using System.Linq.Expressions;
using TeamsAllocationManager.Api.Functions;
using TeamsAllocationManager.Contracts.Base;
using TeamsAllocationManager.Contracts.Import.Commands;
using TeamsAllocationManager.Dtos.Import;

namespace TeamsAllocationManager.Tests.Functions;

[TestFixture]
public class ImportFunctionTests
{
	private readonly ILogger _mockedLogger;
	private readonly Mock<IDispatcher> _dispatcherMock;

	public ImportFunctionTests()
	{
		_mockedLogger = new Mock<ILogger>().Object;
		_dispatcherMock = new Mock<IDispatcher>();
	}

	[Test]
	public void ShouldCallGetImportAllProjectsAndEmployeesCommand()
		=> VerifyFunctionExecutionAsync(c => c.DispatchAsync<ImportProjectsAndEmployeesCommand, ImportReportDto>(It.IsAny<ImportProjectsAndEmployeesCommand>(), default), "POST");

	private void VerifyFunctionExecutionAsync(Expression<Action<IDispatcher>> expression, string verb, string path = "")
	{
		// given
		var function = new ImportFunction(_dispatcherMock.Object);
		var reqMock = new Mock<HttpRequest>();
		reqMock.Setup(r => r.Method).Returns(verb);
		reqMock.Setup(r => r.Query).Returns(new QueryCollection());
		reqMock.Setup(r => r.Body).Returns(new MemoryStream());

		// when
		function.RunAsync(reqMock.Object, path, _mockedLogger).Wait();

		// then
		_dispatcherMock.Verify(expression, Times.Once);
	}
}
