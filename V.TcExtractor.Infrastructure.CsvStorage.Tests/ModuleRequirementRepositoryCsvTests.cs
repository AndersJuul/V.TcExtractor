using Bogus;
using Microsoft.Extensions.Options;
using V.TcExtractor.Domain.Model;
using V.TcExtractor.Domain.Options;
using V.TcExtractor.Infrastructure.CsvStorage.Repositories;

namespace V.TcExtractor.Infrastructure.CsvStorage.Tests;

public class ModuleRequirementRepositoryCsvTests : IDisposable
{
    private readonly Faker<ModuleRequirement> _moduleRequirementFaker;
    private readonly string _testDirectory;

    public ModuleRequirementRepositoryCsvTests()
    {
        // Set up Bogus fake data generator
        _moduleRequirementFaker = new Faker<ModuleRequirement>()
            .RuleFor(t => t.Id, f => f.Random.Guid().ToString())
            .RuleFor(t => t.ProductRequirement, f => "PRO" + f.Lorem.Word())
            .RuleFor(t => t.CombinedRequirement, f => f.Lorem.Sentence(3))
            .RuleFor(t => t.Motivation, f => f.Lorem.Sentence(3))
            .RuleFor(t => t.RsTitle, f => f.Lorem.Sentence(3))
            .RuleFor(t => t.FileName, f => f.System.FileName()
            );

        // Create a temp directory for testing
        _testDirectory = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
        Directory.CreateDirectory(_testDirectory);
    }

    [Fact]
    public void AddRange_ShouldWriteAllTestCasesToFile()
    {
        // Arrange
        var options = Options.Create(new FileLocationOptions { Path = _testDirectory });
        var repository = new ModuleRequirementRepositoryCsv(options);

        var testCases = _moduleRequirementFaker.Generate(5).ToArray();

        // Act
        repository.AddRange(testCases);

        // Assert
        var filePath = Path.Combine(_testDirectory, "mr.csv");
        Assert.True(File.Exists(filePath));

        var savedTestCases = repository.GetAll();
        Assert.Equal(testCases.Length, savedTestCases.Length);

        for (var i = 0; i < testCases.Length; i++)
        {
            Assert.Equal(testCases[i].FileName, savedTestCases[i].FileName);
            Assert.Equal(testCases[i].CombinedRequirement, savedTestCases[i].CombinedRequirement);
            Assert.Equal(testCases[i].Id, savedTestCases[i].Id);
            Assert.Equal(testCases[i].Motivation, savedTestCases[i].Motivation);
            Assert.Equal(testCases[i].RsTitle, savedTestCases[i].RsTitle);
        }
    }

    [Fact]
    public void AddRange_ShouldOverwriteExistingFile()
    {
        // Arrange
        var options = Options.Create(new FileLocationOptions { Path = _testDirectory });
        var repository = new ModuleRequirementRepositoryCsv(options);

        var firstBatch = _moduleRequirementFaker.Generate(3).ToArray();
        var secondBatch = _moduleRequirementFaker.Generate(5).ToArray();

        // Act - Write first batch
        repository.AddRange(firstBatch);

        // Verify first write
        var afterFirstWrite = repository.GetAll();
        Assert.Equal(firstBatch.Length, afterFirstWrite.Length);

        // Write second batch
        repository.AddRange(secondBatch);

        // Assert
        var afterSecondWrite = repository.GetAll();
        Assert.Equal(secondBatch.Length, afterSecondWrite.Length);
    }

    [Fact]
    public void AddRange_ShouldCreateDirectoryIfNotExists()
    {
        var nonExistentDir = Path.Combine(_testDirectory, "newdir");
        var options = Options.Create(new FileLocationOptions { Path = nonExistentDir });
        var repository = new ModuleRequirementRepositoryCsv(options);

        var testCases = _moduleRequirementFaker.Generate(1).ToArray();

        repository.AddRange(testCases);

        Assert.True(Directory.Exists(nonExistentDir));
    }

    [Fact]
    public void AddRange_WithEmptyArray_ShouldCreateEmptyFile()
    {
        var options = Options.Create(new FileLocationOptions { Path = _testDirectory });
        var repository = new ModuleRequirementRepositoryCsv(options);

        repository.AddRange([]);

        var filePath = Path.Combine(_testDirectory, "mr.csv");
        Assert.True(File.Exists(filePath));
        Assert.Empty(repository.GetAll());
    }

    public void Dispose()
    {
        // Clean up test directory
        if (Directory.Exists(_testDirectory))
        {
            Directory.Delete(_testDirectory, true);
        }
    }
}