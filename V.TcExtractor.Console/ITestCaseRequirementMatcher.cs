using V.TcExtractor.Domain.Model;

namespace V.TcExtractor.Console;

public interface ITestCaseRequirementMatcher
{
    bool IsMatch(ModuleRequirement moduleRequirement, TestCase testCase);
}