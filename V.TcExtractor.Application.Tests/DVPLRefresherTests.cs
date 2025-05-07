using Bogus;
using Microsoft.Extensions.Logging;
using Moq;
using V.TcExtractor.Domain.Model;
using V.TcExtractor.Domain.Repositories;
using V.TcExtractor.Infrastructure.OfficeDocuments;

namespace V.TcExtractor.Application.Tests;

public class DvplRefresherTests
{
    private readonly Mock<IFolderScanner> _folderScannerMock;
    private readonly Mock<IDvplItemRepository> _dvplRepositoryMock;
    private readonly Mock<ILogger<DVPLRefresher>> _loggerMock;
    private readonly DVPLRefresher _refresher;
    private readonly Faker<TestCase> _testCaseFaker;
    private readonly Faker<DvplItem> _dvplFaker;

    public DvplRefresherTests()
    {
        _folderScannerMock = new Mock<IFolderScanner>();
        _dvplRepositoryMock = new Mock<IDvplItemRepository>();
        _loggerMock = new Mock<ILogger<DVPLRefresher>>();

        // Set up Bogus fake data generator
        _testCaseFaker = new Faker<TestCase>()
                .RuleFor(t => t.ReqId, f => f.Random.Guid().ToString())
                .RuleFor(t => t.Description, f => f.Lorem.Sentence(3))
                .RuleFor(t => t.TestNo, f => f.Random.Int(min: 1, max: 1000).ToString())
                .RuleFor(t => t.FileName, f => f.System.FileName())
            ;

        _dvplFaker = new Faker<DvplItem>()
            ;

        _refresher = new DVPLRefresher(
            _folderScannerMock.Object,
            _dvplRepositoryMock.Object,
            _loggerMock.Object);
    }

    [Fact]
    public void Execute_ShouldGetTestCasesFromFolderScanner()
    {
        // Arrange
        var dvplItems = _dvplFaker.Generate(2).ToArray();
        _folderScannerMock.Setup(x => x.GetDvplItems()).Returns(dvplItems);

        // Act
        _refresher.Execute();

        // Assert
        _folderScannerMock.Verify(x => x.GetDvplItems(), Times.Once);
    }

    [Fact]
    public void Execute_ShouldDeleteAllExistingTestCases()
    {
        // Arrange
        _folderScannerMock.Setup(x => x.GetTestCases()).Returns([]);

        // Act
        _refresher.Execute();

        // Assert
        _dvplRepositoryMock.Verify(x => x.DeleteAll(), Times.Once);
    }

    [Fact]
    public void Execute_ShouldAddAllNewTestCases()
    {
        // Arrange
        var dvplItems = _dvplFaker.Generate(2).ToArray();
        _folderScannerMock.Setup(x => x.GetDvplItems()).Returns(dvplItems);

        // Act
        _refresher.Execute();

        // Assert
        _dvplRepositoryMock.Verify(x => x.AddRange(dvplItems), Times.Once);
    }

    [Fact]
    public void Execute_ShouldLogStartAndCompletionMessages()
    {
        // Arrange
        var dvplItems = _dvplFaker.Generate(2).ToArray();
        _folderScannerMock.Setup(x => x.GetDvplItems()).Returns(dvplItems);

        // Act
        _refresher.Execute();

        // Assert
        _loggerMock.VerifyLog(LogLevel.Information, "Refreshing DVPL Items.", Times.Exactly(1));
        _loggerMock.VerifyLog(LogLevel.Information, "Done Refreshing DVPL Items: 2", Times.Exactly(1));
    }

    [Fact]
    public void Execute_ShouldHandleEmptyTestCaseList()
    {
        // Arrange
        _folderScannerMock.Setup(x => x.GetTestCases()).Returns([]);

        // Act
        _refresher.Execute();

        // Assert
        _dvplRepositoryMock.Verify(x => x.DeleteAll(), Times.Once);
        _dvplRepositoryMock.Verify(x => x.AddRange(It.IsAny<DvplItem[]>()), Times.Once);
        _loggerMock.VerifyLog(LogLevel.Information, "Done Refreshing DVPL Items: 0", Times.Exactly(1));
    }
}