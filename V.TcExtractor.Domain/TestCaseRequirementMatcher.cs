using V.TcExtractor.Domain.Model;

namespace V.TcExtractor.Domain;

public class TestCaseRequirementMatcher : ITestCaseRequirementMatcher
{
    public bool IsMatch(ModuleRequirement moduleRequirement, TestCase testCase)
    {
        return testCase.ReqId == moduleRequirement.Id;
    }
}