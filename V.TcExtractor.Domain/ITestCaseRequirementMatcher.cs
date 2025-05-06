using V.TcExtractor.Domain.Model;

namespace V.TcExtractor.Domain;

public interface ITestCaseRequirementMatcher
{
    bool IsMatch(ModuleRequirement moduleRequirement, TestCase testCase);
}