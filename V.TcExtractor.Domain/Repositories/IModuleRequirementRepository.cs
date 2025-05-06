using V.TcExtractor.Domain.Model;

namespace V.TcExtractor.Domain.Repositories;

public interface IModuleRequirementRepository
{
    void DeleteAll();
    void AddRange(ModuleRequirement[] moduleRequirements);
}