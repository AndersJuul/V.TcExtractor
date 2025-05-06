using System.Globalization;
using CsvHelper.Configuration;

namespace V.TcExtractor.Infrastructure.CsvStorage;

public record QuotedStringCsvConfig : CsvConfiguration
{
    public QuotedStringCsvConfig() : base(CultureInfo.InvariantCulture)
    {
        ShouldQuote = args => true; // Quote all fields
    }
}