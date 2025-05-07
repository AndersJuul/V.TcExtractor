using V.TcExtractor.Domain.Model;

namespace V.TcExtractor.Domain;

public class TestCaseRequirementMatcher : ITestCaseRequirementMatcher
{
    public bool IsMatch(ModuleRequirement moduleRequirement, TestCase testCase)
    {
        return (testCase.ReqId == moduleRequirement.Id) ||
               testCase.ReqId.Contains(moduleRequirement.Id) ||
               testCase.ReqId.Contains(moduleRequirement.Source + moduleRequirement.Id) ||
               testCase.ReqId.Contains(moduleRequirement.Source + "_" + moduleRequirement.Id) ||
               testCase.ReqId.Contains(moduleRequirement.Source + "." + moduleRequirement.Id)
            ;
    }
}