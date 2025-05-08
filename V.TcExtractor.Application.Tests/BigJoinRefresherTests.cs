using Bogus;
using Microsoft.Extensions.Logging;
using Moq;
using V.TcExtractor.Domain.Model;
using V.TcExtractor.Domain.Repositories;

namespace V.TcExtractor.Application.Tests;

public class BigJoinRefresherTests
{
    private readonly Mock<IBigJoinRepository> _bigJoinRepositoryMock;
    private readonly Mock<ITestCaseRepository> _testCaseRepositoryMock;
    private readonly Mock<IModuleRequirementRepository> _moduleRequirementRepositoryMock;
    private readonly Mock<IDvplItemRepository> _dvplItemRepositoryMock;
    private readonly Mock<ILogger<BigJoinRefresher>> _loggerMock;
    private readonly BigJoinRefresher _refresher;
    private readonly Faker<BigJoin> _bigJoinFaker;
    private readonly Faker<TestCase> _testCaseFaker;

    public BigJoinRefresherTests()
    {
        _moduleRequirementRepositoryMock = new Mock<IModuleRequirementRepository>();
        _testCaseRepositoryMock = new Mock<ITestCaseRepository>();
        _dvplItemRepositoryMock = new Mock<IDvplItemRepository>();
        _bigJoinRepositoryMock = new Mock<IBigJoinRepository>();
        _loggerMock = new Mock<ILogger<BigJoinRefresher>>();

        _bigJoinFaker = new Faker<BigJoin>()
            //.RuleFor(t => t.ReqId, f => f.Random.Guid().ToString())
            //.RuleFor(t => t.Description, f => f.Lorem.Sentence(3))
            //.RuleFor(t => t.TestNo, f => f.Random.Int(min: 1, max: 1000).ToString())
            //.RuleFor(t => t.FileName, f => f.System.FileName())
            ;
        _testCaseFaker = new Faker<TestCase>()
            //.RuleFor(t => t.ReqId, f => f.Random.Guid().ToString())
            //.RuleFor(t => t.Description, f => f.Lorem.Sentence(3))
            //.RuleFor(t => t.TestNo, f => f.Random.Int(min: 1, max: 1000).ToString())
            //.RuleFor(t => t.FileName, f => f.System.FileName())
            ;

        _refresher = new BigJoinRefresher(_testCaseRepositoryMock.Object,
            _moduleRequirementRepositoryMock.Object,
            _dvplItemRepositoryMock.Object,
            _bigJoinRepositoryMock.Object,
            _loggerMock.Object);
    }

    [Fact]
    public void Execute_ShouldGetTestCasesFromRepository()
    {
        // Arrange
        var testCases = _testCaseFaker.Generate(2).ToArray();
        _testCaseRepositoryMock.Setup(x => x.GetAll()).Returns(testCases);

        // Act
        _refresher.Execute();

        // Assert
        _testCaseRepositoryMock.Verify(x => x.GetAll(), Times.Once);
    }

    [Fact]
    public void Execute_ShouldDeleteAllExistingBigJoins()
    {
        // Arrange

        // Act
        _refresher.Execute();

        // Assert
        _bigJoinRepositoryMock.Verify(x => x.DeleteAll(), Times.Once);
    }

    [Fact]
    public void Execute_ShouldAddAllNewBigJoins()
    {
        // Arrange

        // Act
        _refresher.Execute();

        // Assert
        _bigJoinRepositoryMock.Verify(x => x.AddRange(It.IsAny<BigJoin[]>()), Times.Once);
    }

    [Fact]
    public void Execute_ShouldLogStartAndCompletionMessages()
    {
        // Arrange

        // Act
        _refresher.Execute();

        // Assert
        _loggerMock.VerifyLog(LogLevel.Information, "Refreshing Big Joins.", Times.Exactly(1));
        //_loggerMock.VerifyLog(LogLevel.Information, "Done Refreshing Big Joins: 2", Times.Exactly(1));
    }

    [Fact]
    public void Execute_ShouldHandleEmptyBigJoinList()
    {
        // Arrange

        // Act
        _refresher.Execute();

        // Assert
        _bigJoinRepositoryMock.Verify(x => x.DeleteAll(), Times.Once);
        _bigJoinRepositoryMock.Verify(x => x.AddRange(It.IsAny<BigJoin[]>()), Times.Once);
    }
}