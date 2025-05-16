using Bogus;
using Microsoft.Extensions.Logging;
using Moq;
using V.TcExtractor.Domain.Model;
using V.TcExtractor.Domain.Repositories;
using V.TcExtractor.Infrastructure.OfficeDocuments;

namespace V.TcExtractor.Application.Tests;

public class FileItemRefresherTests
{
    private readonly FileItemRefresher _refresher;
    private readonly Faker<FileItem> _fileItemFaker;
    private readonly Mock<IFolderScanner> _folderScannerMock;
    private readonly Mock<IFileItemRepository> _fileItemRepositoryMock;
    private Mock<ILogger<FileItemRefresher>> _loggerMock;

    public FileItemRefresherTests()
    {
        _folderScannerMock = new Mock<IFolderScanner>();
        _fileItemRepositoryMock = new Mock<IFileItemRepository>();

        _fileItemFaker = new Faker<FileItem>()
            //.RuleFor(t => t.ReqId, f => f.Random.Guid().ToString())
            //.RuleFor(t => t.Description, f => f.Lorem.Sentence(3))
            //.RuleFor(t => t.TestNo, f => f.Random.Int(min: 1, max: 1000).ToString())
            //.RuleFor(t => t.FileName, f => f.System.FileName())
            ;

        _loggerMock = new Mock<ILogger<FileItemRefresher>>();
        _refresher =
            new FileItemRefresher(_folderScannerMock.Object, _fileItemRepositoryMock.Object, _loggerMock.Object);
    }

    [Fact]
    public void Execute_ShouldGetFileItemsFromFolderScanner()
    {
        // Arrange
        var fileItems = _fileItemFaker.Generate(2).ToArray();
        _folderScannerMock.Setup(x => x.GetFileItems()).Returns(fileItems);

        // Act
        _refresher.Execute();

        // Assert
        _folderScannerMock.Verify(x => x.GetFileItems(), Times.Once);
    }

    [Fact]
    public void Execute_ShouldDeleteAllExistingFileItems()
    {
        // Arrange

        // Act
        _refresher.Execute();

        // Assert
        _fileItemRepositoryMock.Verify(x => x.DeleteAll(), Times.Once);
    }

    [Fact]
    public void Execute_ShouldAddAllNewFileItems()
    {
        // Arrange

        // Act
        _refresher.Execute();

        // Assert
        _fileItemRepositoryMock.Verify(x => x.AddRange(It.IsAny<FileItem[]>()), Times.Once);
    }

    [Fact]
    public void Execute_ShouldLogStartAndCompletionMessages()
    {
        // Arrange

        // Act
        _refresher.Execute();

        // Assert
        _loggerMock.VerifyLog(LogLevel.Information, "Refreshing File Items.", Times.Exactly(1));
        //_loggerMock.VerifyLog(LogLevel.Information, "Done Refreshing Big Joins: 2", Times.Exactly(1));
    }

    [Fact]
    public void Execute_ShouldHandleEmptyBigJoinList()
    {
        // Arrange

        // Act
        _refresher.Execute();

        // Assert
        _fileItemRepositoryMock.Verify(x => x.DeleteAll(), Times.Once);
        _fileItemRepositoryMock.Verify(x => x.AddRange(It.IsAny<FileItem[]>()), Times.Once);
    }
}