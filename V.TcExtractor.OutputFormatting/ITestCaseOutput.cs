using V.TcExtractor.Model;

namespace V.TcExtractor.OutputFormatting;

public interface ITestCaseOutput
{
    bool CanHandle(string formatId);
    void Write(TestCase[] testCases);
    void Write(ModuleRequirement[] moduleRequirements);
}