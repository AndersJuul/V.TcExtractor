using CsvHelper;
using Microsoft.Extensions.Options;
using V.TcExtractor.Domain.Model;
using V.TcExtractor.Domain.Options;
using V.TcExtractor.Domain.Repositories;
using V.TcExtractor.Infrastructure.CsvStorage.Mappers;

namespace V.TcExtractor.Infrastructure.CsvStorage.Repositories;

public class Match1RepositoryCsv : RepositoryCsv, IMatch1Repository
{
    private readonly FileLocationOptions _fileLocationOptions;

    public Match1RepositoryCsv(IOptions<FileLocationOptions> fileLocationOptions)
    {
        _fileLocationOptions = fileLocationOptions.Value;
    }

    protected override string GetFileName()
    {
        if (!Path.Exists(_fileLocationOptions.Path))
            Directory.CreateDirectory(_fileLocationOptions.Path);

        return Path.Combine(_fileLocationOptions.Path, "matches.csv");
    }

    public void AddRange(Match1[] matches)
    {
        var filePath = GetFileName();

        var config = new QuotedStringCsvConfig();
        using var writer = new StreamWriter(filePath);
        using var csv = new CsvWriter(writer, config);

        // Register the class map
        csv.Context.RegisterClassMap<Match1Map>();

        // Write the records
        csv.WriteRecords(matches);
    }

    public Match1[] GetAll()
    {
        var filePath = GetFileName();

        var config = new QuotedStringCsvConfig();
        using var reader = new StreamReader(filePath);
        using var csv = new CsvReader(reader, config);

        csv.Context.RegisterClassMap<Match1Map>();

        return csv
            .GetRecords<Match1>()
            .ToArray();
    }
}