namespace V.TcExtractor.Domain.Model;

public class Match1(ModuleRequirement moduleRequirement, TestCase[] matchingTestCases)
{
    public ModuleRequirement ModuleRequirement { get; } = moduleRequirement;
    public TestCase[] MatchingTestCases { get; } = matchingTestCases;
}