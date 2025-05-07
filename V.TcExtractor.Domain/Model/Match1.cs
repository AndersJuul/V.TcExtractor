namespace V.TcExtractor.Domain.Model;

public class Match1
{
    public Match1(string moduleRequirementId, string testCases)
    {
        TestCases = testCases;
        ModuleRequirementId = moduleRequirementId;
    }

    public string TestCases { get; set; }

    public string ModuleRequirementId { get; set; }
}