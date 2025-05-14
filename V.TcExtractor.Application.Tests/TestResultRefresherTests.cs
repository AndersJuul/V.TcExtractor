using Bogus;
using Microsoft.Extensions.Logging;
using Moq;
using V.TcExtractor.Domain.Model;
using V.TcExtractor.Domain.Repositories;
using V.TcExtractor.Infrastructure.OfficeDocuments;

namespace V.TcExtractor.Application.Tests;

public class TestResultRefresherTests
{
    private readonly Mock<IFolderScanner> _folderScannerMock;
    private readonly Mock<ITestResultRepository> _testCaseRepositoryMock;
    private readonly Mock<ILogger<TestResultRefresher>> _loggerMock;
    private readonly TestResultRefresher _refresher;
    private readonly Faker<TestResult> _testResultFaker;

    public TestResultRefresherTests()
    {
        _folderScannerMock = new Mock<IFolderScanner>();
        _testCaseRepositoryMock = new Mock<ITestResultRepository>();
        _loggerMock = new Mock<ILogger<TestResultRefresher>>();

        // Set up Bogus fake data generator
        _testResultFaker = new Faker<TestResult>()
            //.RuleFor(t => t.ReqId, f => f.Random.Guid().ToString())
            //.RuleFor(t => t.Description, f => f.Lorem.Sentence(3))
            //.RuleFor(t => t.TestNo, f => f.Random.Int(min: 1, max: 1000).ToString())
            //.RuleFor(t => t.FileName, f => f.System.FileName())
            ;

        _refresher = new TestResultRefresher(
            _folderScannerMock.Object,
            _testCaseRepositoryMock.Object,
            _loggerMock.Object);
    }

    [Fact]
    public void Execute_ShouldGetTestCasesFromFolderScanner()
    {
        // Arrange
        var testResults = _testResultFaker.Generate(2).ToArray();
        _folderScannerMock.Setup(x => x.GetTestResults()).Returns(testResults);

        // Act
        _refresher.Execute();

        // Assert
        _folderScannerMock.Verify(x => x.GetTestResults(), Times.Once);
    }

    [Fact]
    public void Execute_ShouldDeleteAllExistingTestCases()
    {
        // Arrange
        _folderScannerMock.Setup(x => x.GetTestResults()).Returns([]);

        // Act
        _refresher.Execute();

        // Assert
        _testCaseRepositoryMock.Verify(x => x.DeleteAll(), Times.Once);
    }

    [Fact]
    public void Execute_ShouldAddAllNewTestCases()
    {
        // Arrange
        var testCases = _testResultFaker.Generate(2).ToArray();
        _folderScannerMock.Setup(x => x.GetTestResults()).Returns(testCases);

        // Act
        _refresher.Execute();

        // Assert
        _testCaseRepositoryMock.Verify(x => x.AddRange(testCases), Times.Once);
    }

    [Fact]
    public void Execute_ShouldLogStartAndCompletionMessages()
    {
        // Arrange
        var testCases = _testResultFaker.Generate(2).ToArray();
        _folderScannerMock.Setup(x => x.GetTestResults()).Returns(testCases);

        // Act
        _refresher.Execute();

        // Assert
        _loggerMock.VerifyLog(LogLevel.Information, "Refreshing Test Results.", Times.Exactly(1));
        _loggerMock.VerifyLog(LogLevel.Information, "Done Refreshing Test Results: 2", Times.Exactly(1));
    }

    [Fact]
    public void Execute_ShouldHandleEmptyTestCaseList()
    {
        // Arrange
        _folderScannerMock.Setup(x => x.GetTestResults()).Returns([]);

        // Act
        _refresher.Execute();

        // Assert
        _testCaseRepositoryMock.Verify(x => x.DeleteAll(), Times.Once);
        _testCaseRepositoryMock.Verify(x => x.AddRange(It.IsAny<TestResult[]>()), Times.Once);
        _loggerMock.VerifyLog(LogLevel.Information, "Done Refreshing Test Results: 0", Times.Exactly(1));
    }
}