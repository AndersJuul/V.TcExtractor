namespace V.TcExtractor.Domain.Model;

public class Match1
{
    public Match1(ModuleRequirement moduleRequirement, TestCase[] matchingTestCases)
    {
        ModuleRequirementId = moduleRequirement.Id;
        TestCases = string.Join(',', matchingTestCases.Select(x => x.TestNo + x.FileName));
    }

    public string TestCases { get; set; }

    public string ModuleRequirementId { get; }
}