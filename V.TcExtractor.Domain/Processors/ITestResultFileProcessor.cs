using V.TcExtractor.Domain.Model;

namespace V.TcExtractor.Domain.Processors;

public interface ITestResultFileProcessor
{
    bool CanHandle(string fileName);
    List<TestResult> GetTestResults(string fileName);
}