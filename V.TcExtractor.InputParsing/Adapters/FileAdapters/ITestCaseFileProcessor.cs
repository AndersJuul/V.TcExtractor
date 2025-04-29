using V.TcExtractor.Model;

namespace V.TcExtractor.InputParsing.Adapters.FileAdapters;

public interface ITestCaseFileProcessor
{
    bool CanHandle(string fileName);
    List<TestCase> GetTestCases(string fileName);
}