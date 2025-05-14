using V.TcExtractor.Domain.Model;

namespace V.TcExtractor.Domain.Processors;

public interface ITestCaseFileProcessor
{
    bool CanHandle(string fileName);
    List<TestCase> GetTestCases(string fileName);
}