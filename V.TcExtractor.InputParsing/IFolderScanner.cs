using V.TcExtractor.Model;

namespace V.TcExtractor.InputParsing;

public interface IFolderScanner
{
    IEnumerable<TestCase> GetTestCases();
    IEnumerable<ModuleRequirement> GetModuleRequirements();
}