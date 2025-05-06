using V.TcExtractor.Domain;
using V.TcExtractor.Domain.Repositories;
using V.TcExtractor.Infrastructure.OfficeDocuments;

namespace V.TcExtractor.Application;

public class ModuleRequirementRefresher : IModuleRequirementRefresher
{
    private readonly IFolderScanner _folderScanner;
    private readonly IModuleRequirementRepository _moduleRequirementRepository;

    public ModuleRequirementRefresher(IFolderScanner folderScanner,
        IModuleRequirementRepository moduleRequirementRepository)
    {
        _folderScanner = folderScanner;
        _moduleRequirementRepository = moduleRequirementRepository;
    }

    public void Execute()
    {
        var moduleRequirements = _folderScanner
            .GetModuleRequirements()
            .ToArray();

        _moduleRequirementRepository.DeleteAll();
        _moduleRequirementRepository.AddRange(moduleRequirements);
    }
}