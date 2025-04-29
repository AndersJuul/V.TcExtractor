using V.TcExtractor.Model;

namespace V.TcExtractor.InputParsing.Adapters.FileAdapters;

public interface IModuleRequirementFileProcessor
{
    bool CanHandle(string fileName);
    IEnumerable<ModuleRequirement> GetModuleRequirements(string fileName);
}