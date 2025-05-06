using DocumentFormat.OpenXml.Wordprocessing;
using V.TcExtractor.Domain.Model;

namespace V.TcExtractor.Infrastructure.OfficeDocuments.Adapters.TableAdapters;

public interface ITableAdapter
{
    bool CanHandle(Table table);
    IEnumerable<TestCase> GetTestCases(Table table, string filePath);
}