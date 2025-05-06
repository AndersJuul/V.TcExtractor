using Microsoft.Extensions.Logging;
using V.TcExtractor.Domain.Refreshers;
using V.TcExtractor.Domain.Repositories;
using V.TcExtractor.Infrastructure.OfficeDocuments;

namespace V.TcExtractor.Application;

public class ModuleRequirementRefresher : IModuleRequirementRefresher
{
    private readonly IFolderScanner _folderScanner;
    private readonly IModuleRequirementRepository _moduleRequirementRepository;
    private readonly ILogger<ModuleRequirementRefresher> _logger;

    public ModuleRequirementRefresher(IFolderScanner folderScanner,
        IModuleRequirementRepository moduleRequirementRepository,
        ILogger<ModuleRequirementRefresher> logger)
    {
        _folderScanner = folderScanner;
        _moduleRequirementRepository = moduleRequirementRepository;
        _logger = logger;
    }

    public void Execute()
    {
        _logger.LogInformation("Refreshing Module Requirements.");

        var moduleRequirements = _folderScanner
            .GetModuleRequirements()
            .ToArray();

        _moduleRequirementRepository.DeleteAll();
        _moduleRequirementRepository.AddRange(moduleRequirements);

        _logger.LogInformation("Done Refreshing Module Requirements: " + moduleRequirements.Length);
    }
}