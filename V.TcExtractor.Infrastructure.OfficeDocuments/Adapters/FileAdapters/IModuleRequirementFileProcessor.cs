using V.TcExtractor.Domain.Model;

namespace V.TcExtractor.Infrastructure.OfficeDocuments.Adapters.FileAdapters;

public interface IModuleRequirementFileProcessor
{
    bool CanHandle(string fileName);
    IEnumerable<ModuleRequirement> GetModuleRequirements(string fileName);
}