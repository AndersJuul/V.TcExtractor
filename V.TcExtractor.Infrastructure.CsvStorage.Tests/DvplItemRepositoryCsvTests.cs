using Bogus;
using Microsoft.Extensions.Options;
using V.TcExtractor.Domain.Model;
using V.TcExtractor.Domain.Options;
using V.TcExtractor.Infrastructure.CsvStorage.Repositories;

namespace V.TcExtractor.Infrastructure.CsvStorage.Tests;

public class DvplItemRepositoryCsvTests : IDisposable
{
    private readonly string _testDirectory;
    private readonly Faker<DvplItem> _dvplItemFaker;

    public DvplItemRepositoryCsvTests()
    {
        _dvplItemFaker = new Faker<DvplItem>()
                .RuleFor(t => t.ProductRsCode, f => f.Random.Guid().ToString())
                .RuleFor(t => t.FileName, f => f.System.FileName())
            ;

        // Create a temp directory for testing
        _testDirectory = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
        Directory.CreateDirectory(_testDirectory);
    }

    [Fact]
    public void AddRange_ShouldWriteAllTestCasesToFile()
    {
        // Arrange
        var options = Options.Create(new FileLocationOptions { Path = _testDirectory });
        var repository = new DvplItemRepositoryCsv(options);

        var dvplItems = _dvplItemFaker.Generate(5).ToArray();

        // Act
        repository.AddRange(dvplItems);

        // Assert
        var filePath = Path.Combine(_testDirectory, "dvpl.csv");
        Assert.True(File.Exists(filePath));

        var savedTestCases = repository.GetAll();
        Assert.Equal(dvplItems.Length, savedTestCases.Length);

        for (var i = 0; i < dvplItems.Length; i++)
        {
            Assert.Equal(dvplItems[i].FileName, savedTestCases[i].FileName);
        }
    }

    [Fact]
    public void AddRange_ShouldOverwriteExistingFile()
    {
        // Arrange
        var options = Options.Create(new FileLocationOptions { Path = _testDirectory });
        var repository = new DvplItemRepositoryCsv(options);

        var firstBatch = _dvplItemFaker.Generate(3).ToArray();
        var secondBatch = _dvplItemFaker.Generate(5).ToArray();

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
        var repository = new DvplItemRepositoryCsv(options);

        var testCases = _dvplItemFaker.Generate(1).ToArray();

        repository.AddRange(testCases);

        Assert.True(Directory.Exists(nonExistentDir));
    }

    [Fact]
    public void AddRange_WithEmptyArray_ShouldCreateEmptyFile()
    {
        var options = Options.Create(new FileLocationOptions { Path = _testDirectory });
        var repository = new DvplItemRepositoryCsv(options);

        repository.AddRange([]);

        var filePath = Path.Combine(_testDirectory, "dvpl.csv");
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