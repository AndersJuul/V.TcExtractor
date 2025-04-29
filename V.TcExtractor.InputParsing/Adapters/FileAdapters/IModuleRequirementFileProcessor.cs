using V.TcExtractor.Model;

namespace V.TcExtractor.InputParsing.Adapters.FileAdapters;

public interface IModuleRequirementFileProcessor
{
    bool CanHandle(string file);
    IEnumerable<ModuleRequirement> GetModuleRequirements(string file);
}