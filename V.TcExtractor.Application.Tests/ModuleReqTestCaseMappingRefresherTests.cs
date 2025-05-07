using Bogus;
using Microsoft.Extensions.Logging;
using Moq;
using V.TcExtractor.Domain;
using V.TcExtractor.Domain.Model;
using V.TcExtractor.Domain.Repositories;

namespace V.TcExtractor.Application.Tests;

public class ModuleReqTestCaseMappingRefresherTests
{
    private readonly Mock<ITestCaseRepository> _testCaseRepositoryMock;
    private readonly Mock<IModuleRequirementRepository> _moduleRequirementRepositoryMock;
    private readonly Mock<ITestCaseRequirementMatcher> _testCaseRequirementMatcherMock;
    private readonly Mock<IMatch1Repository> _match1RepositoryMock;
    private readonly Mock<ILogger<ModuleReqTestCaseMappingRefresher>> _loggerMock;
    private readonly ModuleReqTestCaseMappingRefresher _refresher;
    private readonly Faker<TestCase> _testCaseFaker;
    private readonly Faker<ModuleRequirement> _moduleRequirementFaker;

    public ModuleReqTestCaseMappingRefresherTests()
    {
        _testCaseRepositoryMock = new Mock<ITestCaseRepository>();
        _moduleRequirementRepositoryMock = new Mock<IModuleRequirementRepository>();
        _testCaseRequirementMatcherMock = new Mock<ITestCaseRequirementMatcher>();
        _match1RepositoryMock = new Mock<IMatch1Repository>();
        _loggerMock = new Mock<ILogger<ModuleReqTestCaseMappingRefresher>>();

        _testCaseFaker = new Faker<TestCase>()
            .RuleFor(t => t.ReqId, f => f.Random.Guid().ToString())
            .RuleFor(t => t.Description, f => f.Lorem.Sentence(3))
            .RuleFor(t => t.TestNo, f => f.Random.Int(min: 1, max: 1000).ToString())
            .RuleFor(t => t.FileName, f => f.System.FileName()
            );
        _moduleRequirementFaker = new Faker<ModuleRequirement>()
            .RuleFor(t => t.Id, f => f.Random.Guid().ToString())
            .RuleFor(t => t.CombinedRequirement, f => f.Lorem.Sentence(3))
            .RuleFor(t => t.Motivation, f => f.Lorem.Sentence(3))
            .RuleFor(t => t.RsTitle, f => f.Lorem.Sentence(3))
            .RuleFor(t => t.FileName, f => f.System.FileName()
            );

        _refresher = new ModuleReqTestCaseMappingRefresher(
            _testCaseRepositoryMock.Object,
            _moduleRequirementRepositoryMock.Object,
            _testCaseRequirementMatcherMock.Object,
            _match1RepositoryMock.Object,
            _loggerMock.Object);
    }

    [Fact]
    public void Execute_ShouldGetAllTestCasesAndRequirements()
    {
        // Arrange
        var testCases = _testCaseFaker.Generate(2).ToArray();
        var requirements = _moduleRequirementFaker.Generate(1).ToArray();
        _testCaseRepositoryMock.Setup(x => x.GetAll()).Returns(testCases);
        _moduleRequirementRepositoryMock.Setup(x => x.GetAll()).Returns(requirements);
        _testCaseRequirementMatcherMock.Setup(x => x.IsMatch(It.IsAny<ModuleRequirement>(), It.IsAny<TestCase>()))
            .Returns(false);

        // Act
        _refresher.Execute();

        // Assert
        _testCaseRepositoryMock.Verify(x => x.GetAll(), Times.Once);
        _moduleRequirementRepositoryMock.Verify(x => x.GetAll(), Times.Once);
    }

    [Fact]
    public void Execute_ShouldLogTestCaseAndRequirementCounts()
    {
        // Arrange
        var testCases = _testCaseFaker.Generate(2).ToArray();
        var requirements = _moduleRequirementFaker.Generate(2).ToArray();
        _testCaseRepositoryMock.Setup(x => x.GetAll()).Returns(testCases);
        _moduleRequirementRepositoryMock.Setup(x => x.GetAll()).Returns(requirements);
        _testCaseRequirementMatcherMock.Setup(x => x.IsMatch(It.IsAny<ModuleRequirement>(), It.IsAny<TestCase>()))
            .Returns(false);

        // Act
        _refresher.Execute();

        // Assert
        _loggerMock.VerifyLog(LogLevel.Information, "Found 2 Test Cases.", Times.Exactly(1));
        _loggerMock.VerifyLog(LogLevel.Information, "Found 2 Module Requirements.", Times.Exactly(1));
    }

    [Fact]
    public void Execute_ShouldCheckMatchingForEachRequirement()
    {
        // Arrange
        var testCases = _testCaseFaker.Generate(2).ToArray();
        var requirements = _moduleRequirementFaker.Generate(2).ToArray();
        _testCaseRepositoryMock.Setup(x => x.GetAll()).Returns(testCases);
        _moduleRequirementRepositoryMock.Setup(x => x.GetAll()).Returns(requirements);
        _testCaseRequirementMatcherMock.Setup(x => x.IsMatch(It.IsAny<ModuleRequirement>(), It.IsAny<TestCase>()))
            .Returns(false);

        // Act
        _refresher.Execute();

        // Assert
        _testCaseRequirementMatcherMock.Verify(
            x => x.IsMatch(It.IsAny<ModuleRequirement>(), It.IsAny<TestCase>()),
            Times.Exactly(requirements.Length * testCases.Length));
    }

    [Fact]
    public void Execute_ShouldCreateMatchesForSuccessfulMatches()
    {
        // Arrange
        var testCase = new TestCase { ReqId = "TC1", FileName = "" };
        var requirement = new ModuleRequirement { Id = "REQ1" };
        _testCaseRepositoryMock.Setup(x => x.GetAll()).Returns(new[] { testCase });
        _moduleRequirementRepositoryMock.Setup(x => x.GetAll()).Returns(new[] { requirement });
        _testCaseRequirementMatcherMock.Setup(x => x.IsMatch(requirement, testCase))
            .Returns(true);

        // Act
        _refresher.Execute();

        // Assert
        _match1RepositoryMock.Verify(x => x.AddRange(It.Is<Match1[]>(matches =>
            matches.Length == 1 &&
            matches[0].ModuleRequirementId == requirement.Id &&
            matches[0].TestCases.Contains(testCase.TestNo))), Times.Once);
    }

    [Fact]
    public void Execute_ShouldClearExistingMatchesBeforeAddingNewOnes()
    {
        // Arrange
        _testCaseRepositoryMock.Setup(x => x.GetAll()).Returns(Array.Empty<TestCase>());
        _moduleRequirementRepositoryMock.Setup(x => x.GetAll()).Returns(Array.Empty<ModuleRequirement>());

        // Act
        _refresher.Execute();

        // Assert
        _match1RepositoryMock.Verify(x => x.DeleteAll(), Times.Once);
        _match1RepositoryMock.Verify(x => x.AddRange(It.IsAny<Match1[]>()), Times.Once);
    }

    [Fact]
    public void Execute_ShouldLogFinalMatchCount()
    {
        // Arrange
        var testCase = _testCaseFaker.Generate();
        var requirement = _moduleRequirementFaker.Generate();
        _testCaseRepositoryMock.Setup(x => x.GetAll()).Returns(new[] { testCase });
        _moduleRequirementRepositoryMock.Setup(x => x.GetAll()).Returns(new[] { requirement });
        _testCaseRequirementMatcherMock.Setup(x => x.IsMatch(requirement, testCase))
            .Returns(true);

        // Act
        _refresher.Execute();

        // Assert
        _loggerMock.VerifyLog(LogLevel.Information,
            "Done Refreshing Module Requirements/Test Case mapping: 1", Times.Exactly(1));
    }

    [Fact]
    public void Execute_ShouldHandleEmptyRepositories()
    {
        // Arrange
        _testCaseRepositoryMock.Setup(x => x.GetAll()).Returns([]);
        _moduleRequirementRepositoryMock.Setup(x => x.GetAll()).Returns([]);

        // Act
        _refresher.Execute();

        // Assert
        _match1RepositoryMock.Verify(x => x.AddRange(Array.Empty<Match1>()), Times.Once);
        _loggerMock.VerifyLog(LogLevel.Information,
            "Done Refreshing Module Requirements/Test Case mapping: 0", Times.Exactly(1));
    }
}