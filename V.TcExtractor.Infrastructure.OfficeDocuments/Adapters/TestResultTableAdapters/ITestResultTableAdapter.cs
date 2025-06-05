using DocumentFormat.OpenXml.Wordprocessing;
using V.TcExtractor.Domain.Model;

namespace V.TcExtractor.Infrastructure.OfficeDocuments.Adapters.TestResultTableAdapters
{
    public interface ITestResultTableAdapter
    {
        bool CanHandle(Table table);
        IEnumerable<TestResult> GetTestResults(Table table, string fileName, string dmsNumber);
    }
}