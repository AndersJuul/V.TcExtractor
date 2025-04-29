using V.TcExtractor.Model;

namespace V.TcExtractor.InputParsing;

public interface IFolderScanner
{
    IEnumerable<TestCase> GetTestCases(string pathToFiles);
    IEnumerable<ModuleRequirement> GetModuleRequirements(string pathToFiles);
}