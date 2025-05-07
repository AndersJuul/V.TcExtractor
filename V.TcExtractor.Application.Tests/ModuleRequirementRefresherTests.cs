using Bogus;
using Microsoft.Extensions.Logging;
using Moq;
using V.TcExtractor.Application;
using V.TcExtractor.Domain.Model;
using V.TcExtractor.Domain.Refreshers;
using V.TcExtractor.Domain.Repositories;
using V.TcExtractor.Infrastructure.OfficeDocuments;
using Xunit;

namespace V.TcExtractor.Application.Tests;

public class ModuleRequirementRefresherTests
{
    private readonly Mock<IFolderScanner> _folderScannerMock;
    private readonly Mock<IModuleRequirementRepository> _moduleRequirementRepositoryMock;
    private readonly Mock<ILogger<ModuleRequirementRefresher>> _loggerMock;
    private readonly ModuleRequirementRefresher _refresher;
    private readonly Faker<ModuleRequirement> _moduleRequirementFaker;

    public ModuleRequirementRefresherTests()
    {
        _folderScannerMock = new Mock<IFolderScanner>();
        _moduleRequirementRepositoryMock = new Mock<IModuleRequirementRepository>();
        _loggerMock = new Mock<ILogger<ModuleRequirementRefresher>>();

        // Set up Bogus fake data generator
        _moduleRequirementFaker = new Faker<ModuleRequirement>()
            .RuleFor(t => t.Id, f => f.Random.Guid().ToString())
            .RuleFor(t => t.CombinedRequirement, f => f.Lorem.Sentence(3))
            .RuleFor(t => t.Motivation, f => f.Lorem.Sentence(3))
            .RuleFor(t => t.RsTitle, f => f.Lorem.Sentence(3))
            .RuleFor(t => t.FileName, f => f.System.FileName()
            );

        _refresher = new ModuleRequirementRefresher(
            _folderScannerMock.Object,
            _moduleRequirementRepositoryMock.Object,
            _loggerMock.Object);
    }

    [Fact]
    public void Execute_ShouldGetModuleRequirementsFromFolderScanner()
    {
        // Arrange
        var requirements = _moduleRequirementFaker.Generate(2).ToArray;
        _folderScannerMock.Setup(x => x.GetModuleRequirements()).Returns(requirements);

        // Act
        _refresher.Execute();

        // Assert
        _folderScannerMock.Verify(x => x.GetModuleRequirements(), Times.Once);
    }

    [Fact]
    public void Execute_ShouldDeleteAllExistingModuleRequirements()
    {
        // Arrange
        _folderScannerMock.Setup(x => x.GetModuleRequirements()).Returns(Array.Empty<ModuleRequirement>());

        // Act
        _refresher.Execute();

        // Assert
        _moduleRequirementRepositoryMock.Verify(x => x.DeleteAll(), Times.Once);
    }

    [Fact]
    public void Execute_ShouldAddAllNewModuleRequirements()
    {
        // Arrange
        var requirements = _moduleRequirementFaker.Generate(2).ToArray();
        _folderScannerMock.Setup(x => x.GetModuleRequirements()).Returns(requirements);

        // Act
        _refresher.Execute();

        // Assert
        _moduleRequirementRepositoryMock.Verify(x => x.AddRange(requirements), Times.Once);
    }

    [Fact]
    public void Execute_ShouldLogStartAndCompletionMessages()
    {
        // Arrange
        var requirements = _moduleRequirementFaker.Generate(2).ToArray();
        _folderScannerMock.Setup(x => x.GetModuleRequirements()).Returns(requirements);

        // Act
        _refresher.Execute();

        // Assert
        _loggerMock.VerifyLog(LogLevel.Information, "Refreshing Module Requirements.", Times.Exactly(1));
        _loggerMock.VerifyLog(LogLevel.Information, "Done Refreshing Module Requirements: 2", Times.Exactly(1));
    }

    [Fact]
    public void Execute_ShouldHandleEmptyModuleRequirementsList()
    {
        // Arrange
        _folderScannerMock.Setup(x => x.GetModuleRequirements()).Returns([]);

        // Act
        _refresher.Execute();

        // Assert
        _moduleRequirementRepositoryMock.Verify(x => x.DeleteAll(), Times.Once);
        _moduleRequirementRepositoryMock.Verify(x => x.AddRange(It.IsAny<ModuleRequirement[]>()), Times.Once);
        _loggerMock.VerifyLog(LogLevel.Information, "Done Refreshing Module Requirements: 0", Times.Exactly(1));
    }
}