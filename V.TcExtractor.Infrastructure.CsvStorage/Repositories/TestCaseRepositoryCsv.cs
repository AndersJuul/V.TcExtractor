using Microsoft.Extensions.Options;
using CsvHelper;
using V.TcExtractor.Domain.Model;
using V.TcExtractor.Domain.Options;
using V.TcExtractor.Domain.Repositories;
using V.TcExtractor.Infrastructure.CsvStorage.Mappers;

namespace V.TcExtractor.Infrastructure.CsvStorage.Repositories;

public class TestCaseRepositoryCsv : RepositoryCsv, ITestCaseRepository
{
    private readonly FileLocationOptions _fileLocationOptions;

    public TestCaseRepositoryCsv(IOptions<FileLocationOptions> fileLocationOptions)
    {
        _fileLocationOptions = fileLocationOptions.Value;
    }

    public void AddRange(TestCase[] testCases)
    {
        var filePath = GetFileName();

        var config = new QuotedStringCsvConfig();
        using var writer = new StreamWriter(filePath);
        using var csv = new CsvWriter(writer, config);

        // Register the class map
        csv.Context.RegisterClassMap<TestCaseMap>();

        // Write the records
        csv.WriteRecords(testCases);
    }

    public TestCase[] GetAll()
    {
        var filePath = GetFileName();

        var config = new QuotedStringCsvConfig();
        using var reader = new StreamReader(filePath);
        using var csv = new CsvReader(reader, config);

        csv.Context.RegisterClassMap<TestCaseMap>();

        return csv
            .GetRecords<TestCase>()
            .ToArray();
    }

    protected override string GetFileName()
    {
        if (!Path.Exists(_fileLocationOptions.Path))
            Directory.CreateDirectory(_fileLocationOptions.Path);
        return Path.Combine(_fileLocationOptions.Path, "tc.csv");
    }
}