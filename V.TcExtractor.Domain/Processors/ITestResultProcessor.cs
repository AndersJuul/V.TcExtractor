using V.TcExtractor.Domain.Model;

namespace V.TcExtractor.Domain.Processors;

public interface ITestResultProcessor
{
    bool CanHandle(string fileName);
    IEnumerable<TestResult> GetTestResults(string fileName);
}