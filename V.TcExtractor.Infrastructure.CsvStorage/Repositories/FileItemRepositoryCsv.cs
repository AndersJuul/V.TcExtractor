using CsvHelper;
using Microsoft.Extensions.Options;
using V.TcExtractor.Domain.Model;
using V.TcExtractor.Domain.Options;
using V.TcExtractor.Domain.Repositories;
using V.TcExtractor.Infrastructure.CsvStorage.Mappers;

namespace V.TcExtractor.Infrastructure.CsvStorage.Repositories;

public class FileItemRepositoryCsv : RepositoryCsv, IFileItemRepository
{
    private readonly FileLocationOptions _fileLocationOptions;

    public FileItemRepositoryCsv(IOptions<FileLocationOptions> fileLocationOptions)
    {
        _fileLocationOptions = fileLocationOptions.Value;
    }

    protected override string GetFileName()
    {
        if (!Path.Exists(_fileLocationOptions.Path))
            Directory.CreateDirectory(_fileLocationOptions.Path);
        return Path.Combine(_fileLocationOptions.Path, "fi.csv");
    }

    public void AddRange(FileItem[] fileItems)
    {
        var filePath = GetFileName();

        var config = new QuotedStringCsvConfig();
        using var writer = new StreamWriter(filePath);
        using var csv = new CsvWriter(writer, config);

        // Register the class map
        csv.Context.RegisterClassMap<FileItemMap>();

        // Write the records
        csv.WriteRecords(fileItems);
    }

    public FileItem[] GetAll()
    {
        var filePath = GetFileName();

        var config = new QuotedStringCsvConfig();
        using var reader = new StreamReader(filePath);
        using var csv = new CsvReader(reader, config);

        csv.Context.RegisterClassMap<FileItemMap>();

        return csv
            .GetRecords<FileItem>()
            .ToArray();
    }
}