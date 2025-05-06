using Microsoft.Extensions.Options;
using System.Formats.Asn1;
using CsvHelper;
using V.TcExtractor.Domain;
using V.TcExtractor.Domain.Model;
using V.TcExtractor.Domain.Options;

namespace V.TcExtractor.Infrastructure.CsvStorage;

public class TestCaseRepositoryCsv : ITestCaseRepository
{
    private readonly FileLocationOptions _fileLocationOptions;

    public TestCaseRepositoryCsv(IOptions<FileLocationOptions> fileLocationOptions)
    {
        _fileLocationOptions = fileLocationOptions.Value;
    }

    public void DeleteAll()
    {
        var fileName = GetFileName();
        if (!File.Exists(fileName))
            return;

        File.Delete(fileName);
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

    private string GetFileName()
    {
        return Path.Combine(_fileLocationOptions.Path, "tc.csv");
    }
}