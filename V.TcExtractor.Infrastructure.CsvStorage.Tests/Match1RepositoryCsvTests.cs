using Bogus;
using V.TcExtractor.Domain.Model;
using V.TcExtractor.Domain.Options;
using V.TcExtractor.Infrastructure.CsvStorage.Repositories;

namespace V.TcExtractor.Infrastructure.CsvStorage.Tests;

public class Match1RepositoryCsvTests : IDisposable
{
    private readonly Faker<TestCase> _testCaseFaker;
    private readonly Faker<ModuleRequirement> _moduleRequirementFaker;
    private readonly string _testDirectory;
    private readonly FileLocationOptions _options;
    private readonly Match1RepositoryCsv _repository;

    public Match1RepositoryCsvTests()
    {
        _testCaseFaker = new Faker<TestCase>()
            .RuleFor(t => t.ReqId, f => f.Random.Guid().ToString())
            .RuleFor(t => t.Description, f => f.Lorem.Sentence(3))
            .RuleFor(t => t.TestNo, f => f.Random.Int(min: 1, max: 1000).ToString())
            .RuleFor(t => t.FileName, f => f.System.FileName())
            .RuleFor(t => t.DmsNumber,
                f => f.Random.Int(min: 1000, max: 9999) + "-" + f.Random.Int(min: 1000, max: 9999));

        _moduleRequirementFaker = new Faker<ModuleRequirement>()
            .RuleFor(t => t.Id, f => f.Random.Guid().ToString())
            .RuleFor(t => t.CombinedRequirement, f => f.Lorem.Sentence(3))
            .RuleFor(t => t.Motivation, f => f.Lorem.Sentence(3))
            .RuleFor(t => t.RsTitle, f => f.Lorem.Sentence(3))
            .RuleFor(t => t.FileName, f => f.System.FileName()
            );

        // Create a unique test directory
        _testDirectory = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
        Directory.CreateDirectory(_testDirectory);

        // Configure options
        _options = new FileLocationOptions { Path = _testDirectory };

        // Create repository instance
        _repository = new Match1RepositoryCsv(Microsoft.Extensions.Options.Options.Create(_options));
    }

    [Fact]
    public void AddRange_ShouldCreateCsvFileInSpecifiedLocation()
    {
        // Arrange
        var match = CreateTestMatch();

        // Act
        _repository.AddRange(new[] { match });

        // Assert
        var expectedFilePath = Path.Combine(_testDirectory, "matches.csv");
        Assert.True(File.Exists(expectedFilePath));
    }

    [Fact]
    public void AddRangeAndGetAll_ShouldRoundTripSingleMatch()
    {
        // Arrange
        var originalMatch = CreateTestMatch();

        // Act
        _repository.AddRange(new[] { originalMatch });
        var retrievedMatches = _repository.GetAll();
        var retrievedMatch = retrievedMatches.First();

        // Assert
        Assert.Single(retrievedMatches);
        Assert.Equal(originalMatch.ModuleRequirementId, retrievedMatch.ModuleRequirementId);
        Assert.Equal(originalMatch.TestCases, retrievedMatch.TestCases);
    }

    [Fact]
    public void AddRangeAndGetAll_ShouldRoundTripMultipleMatches()
    {
        // Arrange
        var matches = new[]
        {
            CreateTestMatch(),
            CreateTestMatch(),
            CreateTestMatch()
        };

        // Act
        _repository.AddRange(matches);
        var retrievedMatches = _repository.GetAll();

        // Assert
        Assert.Equal(matches.Length, retrievedMatches.Length);
        for (int i = 0; i < matches.Length; i++)
        {
            Assert.Equal(matches[i].ModuleRequirementId, retrievedMatches[i].ModuleRequirementId);
            Assert.Equal(matches[i].TestCases.First(),
                retrievedMatches[i].TestCases.First());
        }
    }

    [Fact]
    public void DeleteAll_ShouldRemoveCsvFile()
    {
        // Arrange
        _repository.AddRange(new[] { CreateTestMatch() });
        var filePath = Path.Combine(_testDirectory, "matches.csv");
        Assert.True(File.Exists(filePath));

        // Act
        _repository.DeleteAll();

        // Assert
        Assert.False(File.Exists(filePath));
    }

    [Fact]
    public void DeleteAll_ShouldNotThrowWhenFileDoesNotExist()
    {
        // Arrange
        var filePath = Path.Combine(_testDirectory, "matches.csv");
        Assert.False(File.Exists(filePath));

        // Act & Assert
        var exception = Record.Exception(() => _repository.DeleteAll());
        Assert.Null(exception);
    }

    [Fact]
    public void AddRange_ShouldOverwriteExistingFile()
    {
        // Arrange
        var firstBatch = new[] { CreateTestMatch(), CreateTestMatch() };
        var secondBatch = new[] { CreateTestMatch() };

        // Act
        _repository.AddRange(firstBatch);
        var afterFirstWrite = _repository.GetAll();
        _repository.AddRange(secondBatch);
        var afterSecondWrite = _repository.GetAll();

        // Assert
        Assert.Equal(firstBatch.Length, afterFirstWrite.Length);
        Assert.Equal(secondBatch.Length, afterSecondWrite.Length);
    }

    [Fact]
    public void GetAll_ShouldHandleEmptyFile()
    {
        // Arrange
        File.WriteAllText(Path.Combine(_testDirectory, "matches.csv"), "");

        // Act
        var result = _repository.GetAll();

        // Assert
        Assert.Empty(result);
    }

    private Match1 CreateTestMatch()
    {
        var moduleRequirementId = _moduleRequirementFaker.Generate().Id;
        var values = _testCaseFaker.Generate(2).ToArray().Select(x => $"{x.TestNo}:{x.FileName}/{x.DmsNumber}");
        var testCases = string.Join(';', values);

        return new Match1(moduleRequirementId, testCases);
    }

    public void Dispose()
    {
        // Clean up test directory
        if (Directory.Exists(_testDirectory))
        {
            try
            {
                Directory.Delete(_testDirectory, true);
            }
            catch
            {
                // Ignore cleanup errors
            }
        }
    }
}