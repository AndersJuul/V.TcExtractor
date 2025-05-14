using V.TcExtractor.Domain.Model;

namespace V.TcExtractor.Domain.Processors;

public interface IModuleRequirementFileProcessor
{
    bool CanHandle(string fileName);
    IEnumerable<ModuleRequirement> GetModuleRequirements(string fileName);
}