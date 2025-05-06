using CsvHelper;
using Microsoft.Extensions.Options;
using V.TcExtractor.Domain.Model;
using V.TcExtractor.Domain.Options;
using V.TcExtractor.Domain.Repositories;
using V.TcExtractor.Infrastructure.CsvStorage.Maps;

namespace V.TcExtractor.Infrastructure.CsvStorage.Repositories;

public class ModuleRequirementRepositoryCsv : RepositoryCsv, IModuleRequirementRepository
{
    private readonly FileLocationOptions _fileLocationOptions;

    public ModuleRequirementRepositoryCsv(IOptions<FileLocationOptions> fileLocationOptions)
    {
        _fileLocationOptions = fileLocationOptions.Value;
    }

    protected override string GetFileName()
    {
        if (!Path.Exists(_fileLocationOptions.Path))
            Directory.CreateDirectory(_fileLocationOptions.Path);

        return Path.Combine(_fileLocationOptions.Path, "mr.csv");
    }

    public void AddRange(ModuleRequirement[] moduleRequirements)
    {
        var filePath = GetFileName();

        var config = new QuotedStringCsvConfig();
        using var writer = new StreamWriter(filePath);
        using var csv = new CsvWriter(writer, config);

        // Register the class map
        csv.Context.RegisterClassMap<ModuleRequirementMap>();

        // Write the records
        csv.WriteRecords(moduleRequirements);
    }

    public ModuleRequirement[] GetAll()
    {
        var filePath = GetFileName();

        var config = new QuotedStringCsvConfig();
        using var reader = new StreamReader(filePath);
        using var csv = new CsvReader(reader, config);

        csv.Context.RegisterClassMap<TestCaseMap>();

        return csv
            .GetRecords<ModuleRequirement>()
            .ToArray();
    }
}