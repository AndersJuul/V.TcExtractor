﻿using CsvHelper;
using Microsoft.Extensions.Options;
using V.TcExtractor.Domain.Model;
using V.TcExtractor.Domain.Options;
using V.TcExtractor.Domain.Repositories;
using V.TcExtractor.Infrastructure.CsvStorage.Mappers;

namespace V.TcExtractor.Infrastructure.CsvStorage.Repositories;

public class BigJoinRepositoryCsv : RepositoryCsv, IBigJoinRepository
{
    private readonly FileLocationOptions _fileLocationOptions;

    public BigJoinRepositoryCsv(IOptions<FileLocationOptions> fileLocationOptions)
    {
        _fileLocationOptions = fileLocationOptions.Value;
    }

    public void AddRange(BigJoin[] bigJoins)
    {
        var filePath = GetFileName();

        var config = new QuotedStringCsvConfig();
        using var writer = new StreamWriter(filePath);
        using var csv = new CsvWriter(writer, config);

        // Register the class map
        csv.Context.RegisterClassMap<BigJoinMap>();

        // Write the records
        csv.WriteRecords(bigJoins);
    }

    protected override string GetFileName()
    {
        if (!Path.Exists(_fileLocationOptions.Path))
            Directory.CreateDirectory(_fileLocationOptions.Path);
        return Path.Combine(_fileLocationOptions.Path, "bj.csv");
    }

    public BigJoin[] GetAll()
    {
        var filePath = GetFileName();

        var config = new QuotedStringCsvConfig();
        using var reader = new StreamReader(filePath);
        using var csv = new CsvReader(reader, config);

        csv.Context.RegisterClassMap<BigJoinMap>();

        return csv
            .GetRecords<BigJoin>()
            .ToArray();
    }
}