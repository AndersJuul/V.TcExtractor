using Bogus;
using Microsoft.Extensions.Options;
using V.TcExtractor.Domain.Model;
using V.TcExtractor.Domain.Options;
using V.TcExtractor.Infrastructure.CsvStorage.Repositories;

namespace V.TcExtractor.Infrastructure.CsvStorage.Tests;

public class TestCaseRepositoryCsvTests : IDisposable
{
    private readonly Faker<TestCase> _testCaseFaker;
    private readonly string _testDirectory;

    public TestCaseRepositoryCsvTests()
    {
        // Set up Bogus fake data generator
        _testCaseFaker = new Faker<TestCase>()
            .RuleFor(t => t.ReqId, f => f.Random.Guid().ToString())
            .RuleFor(t => t.Description, f => f.Lorem.Sentence(3))
            .RuleFor(t => t.TestNo, f => f.Random.Int(min: 1, max: 1000).ToString())
            .RuleFor(t => t.FileName, f => f.System.FileName())
            .RuleFor(t => t.DmsNumber,
                f => f.Random.Int(min: 1000, max: 9999) + "-" + f.Random.Int(min: 1000, max: 9999)
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
        var repository = new TestCaseRepositoryCsv(options);

        var testCases = _testCaseFaker.Generate(5).ToArray();

        // Act
        repository.AddRange(testCases);

        // Assert
        var filePath = Path.Combine(_testDirectory, "tc.csv");
        Assert.True(File.Exists(filePath));

        var savedTestCases = repository.GetAll();
        Assert.Equal(testCases.Length, savedTestCases.Length);

        for (var i = 0; i < testCases.Length; i++)
        {
            Assert.Equal(testCases[i].ReqId, savedTestCases[i].ReqId);
            Assert.Equal(testCases[i].Description, savedTestCases[i].Description);
        }
    }

    [Fact]
    public void AddRange_ShouldOverwriteExistingFile()
    {
        // Arrange
        var options = Options.Create(new FileLocationOptions { Path = _testDirectory });
        var repository = new TestCaseRepositoryCsv(options);

        var firstBatch = _testCaseFaker.Generate(3).ToArray();
        var secondBatch = _testCaseFaker.Generate(5).ToArray();

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
        var repository = new TestCaseRepositoryCsv(options);

        var testCases = _testCaseFaker.Generate(1).ToArray();

        repository.AddRange(testCases);

        Assert.True(Directory.Exists(nonExistentDir));
    }

    [Fact]
    public void AddRange_WithEmptyArray_ShouldCreateEmptyFile()
    {
        var options = Options.Create(new FileLocationOptions { Path = _testDirectory });
        var repository = new TestCaseRepositoryCsv(options);

        repository.AddRange([]);

        var filePath = Path.Combine(_testDirectory, "tc.csv");
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