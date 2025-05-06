using CsvHelper;
using Microsoft.Extensions.Options;
using V.TcExtractor.Domain.Model;
using V.TcExtractor.Domain.Options;
using V.TcExtractor.Domain.Repositories;

namespace V.TcExtractor.Infrastructure.CsvStorage;

public class ModuleRequirementRepositoryCsv : RepositoryCsv, IModuleRequirementRepository
{
    private readonly FileLocationOptions _fileLocationOptions;

    public ModuleRequirementRepositoryCsv(IOptions<FileLocationOptions> fileLocationOptions)
    {
        _fileLocationOptions = fileLocationOptions.Value;
    }

    protected override string GetFileName()
    {
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
}