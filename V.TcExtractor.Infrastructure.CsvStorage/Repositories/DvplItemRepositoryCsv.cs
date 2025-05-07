using CsvHelper;
using Microsoft.Extensions.Options;
using V.TcExtractor.Domain.Model;
using V.TcExtractor.Domain.Options;
using V.TcExtractor.Domain.Repositories;
using V.TcExtractor.Infrastructure.CsvStorage.Mappers;

namespace V.TcExtractor.Infrastructure.CsvStorage.Repositories
{
    public class DvplItemRepositoryCsv : RepositoryCsv, IDvplItemRepository
    {
        private readonly FileLocationOptions _fileLocationOptions;

        public DvplItemRepositoryCsv(IOptions<FileLocationOptions> fileLocationOptions)
        {
            _fileLocationOptions = fileLocationOptions.Value;
        }

        protected override string GetFileName()
        {
            if (!Path.Exists(_fileLocationOptions.Path))
                Directory.CreateDirectory(_fileLocationOptions.Path);
            return Path.Combine(_fileLocationOptions.Path, "dvpl.csv");
        }

        public void AddRange(DvplItem[] dvplItems)
        {
            var filePath = GetFileName();

            var config = new QuotedStringCsvConfig();
            using var writer = new StreamWriter(filePath);
            using var csv = new CsvWriter(writer, config);

            // Register the class map
            csv.Context.RegisterClassMap<DvplItemMap>();

            // Write the records
            csv.WriteRecords(dvplItems);
        }

        public DvplItem[] GetAll()
        {
            var filePath = GetFileName();

            var config = new QuotedStringCsvConfig();
            using var reader = new StreamReader(filePath);
            using var csv = new CsvReader(reader, config);

            csv.Context.RegisterClassMap<DvplItemMap>();

            return csv
                .GetRecords<DvplItem>()
                .ToArray();
        }
    }
}