using DocumentFormat.OpenXml.Wordprocessing;
using V.TcExtractor.Domain.Model;

namespace V.TcExtractor.Infrastructure.OfficeDocuments.Adapters.TestCaseTableAdapters;

public interface ITestCaseTableAdapter
{
    bool CanHandle(Table table);
    IEnumerable<TestCase> GetTestCases(Table table, string filePath, string dmsNumber);
}