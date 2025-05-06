using V.TcExtractor.Domain.Model;

namespace V.TcExtractor.Infrastructure.OfficeDocuments.Adapters.FileAdapters;

public interface ITestCaseFileProcessor
{
    bool CanHandle(string fileName);
    List<TestCase> GetTestCases(string fileName);
}