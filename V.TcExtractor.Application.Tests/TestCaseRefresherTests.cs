using Bogus;
using Microsoft.Extensions.Logging;
using Moq;
using V.TcExtractor.Domain.Model;
using V.TcExtractor.Domain.Repositories;
using V.TcExtractor.Infrastructure.OfficeDocuments;

namespace V.TcExtractor.Application.Tests;

public class TestCaseRefresherTests
{
    private readonly Mock<IFolderScanner> _folderScannerMock;
    private readonly Mock<ITestCaseRepository> _testCaseRepositoryMock;
    private readonly Mock<ILogger<TestCaseRefresher>> _loggerMock;
    private readonly TestCaseRefresher _refresher;
    private readonly Faker<TestCase> _testCaseFaker;

    public TestCaseRefresherTests()
    {
        _folderScannerMock = new Mock<IFolderScanner>();
        _testCaseRepositoryMock = new Mock<ITestCaseRepository>();
        _loggerMock = new Mock<ILogger<TestCaseRefresher>>();

        // Set up Bogus fake data generator
        _testCaseFaker = new Faker<TestCase>()
            .RuleFor(t => t.ReqId, f => f.Random.Guid().ToString())
            .RuleFor(t => t.Description, f => f.Lorem.Sentence(3))
            .RuleFor(t => t.TestNo, f => f.Random.Int(min: 1, max: 1000).ToString())
            .RuleFor(t => t.FileName, f => f.System.FileName()
            );

        _refresher = new TestCaseRefresher(
            _folderScannerMock.Object,
            _testCaseRepositoryMock.Object,
            _loggerMock.Object);
    }

    [Fact]
    public void Execute_ShouldGetTestCasesFromFolderScanner()
    {
        // Arrange
        var testCases = _testCaseFaker.Generate(2).ToArray();
        _folderScannerMock.Setup(x => x.GetTestCases()).Returns(testCases);

        // Act
        _refresher.Execute();

        // Assert
        _folderScannerMock.Verify(x => x.GetTestCases(), Times.Once);
    }

    [Fact]
    public void Execute_ShouldDeleteAllExistingTestCases()
    {
        // Arrange
        _folderScannerMock.Setup(x => x.GetTestCases()).Returns([]);

        // Act
        _refresher.Execute();

        // Assert
        _testCaseRepositoryMock.Verify(x => x.DeleteAll(), Times.Once);
    }

    [Fact]
    public void Execute_ShouldAddAllNewTestCases()
    {
        // Arrange
        var testCases = _testCaseFaker.Generate(2).ToArray();
        _folderScannerMock.Setup(x => x.GetTestCases()).Returns(testCases);

        // Act
        _refresher.Execute();

        // Assert
        _testCaseRepositoryMock.Verify(x => x.AddRange(testCases), Times.Once);
    }

    [Fact]
    public void Execute_ShouldLogStartAndCompletionMessages()
    {
        // Arrange
        var testCases = _testCaseFaker.Generate(2).ToArray();
        _folderScannerMock.Setup(x => x.GetTestCases()).Returns(testCases);

        // Act
        _refresher.Execute();

        // Assert
        _loggerMock.VerifyLog(LogLevel.Information, "Refreshing Test Cases.", Times.Exactly(1));
        _loggerMock.VerifyLog(LogLevel.Information, "Done Refreshing Test Cases: 2", Times.Exactly(1));
    }

    [Fact]
    public void Execute_ShouldHandleEmptyTestCaseList()
    {
        // Arrange
        _folderScannerMock.Setup(x => x.GetTestCases()).Returns([]);

        // Act
        _refresher.Execute();

        // Assert
        _testCaseRepositoryMock.Verify(x => x.DeleteAll(), Times.Once);
        _testCaseRepositoryMock.Verify(x => x.AddRange(It.IsAny<TestCase[]>()), Times.Once);
        _loggerMock.VerifyLog(LogLevel.Information, "Done Refreshing Test Cases: 0", Times.Exactly(1));
    }
}