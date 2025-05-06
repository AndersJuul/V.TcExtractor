using Microsoft.Extensions.Logging;
using V.TcExtractor.Domain.Refreshers;
using V.TcExtractor.Domain.Repositories;

namespace V.TcExtractor.Application;

public class ModuleReqTestCaseMappingRefresher : IModuleReqTestCaseMappingRefresher
{
    private readonly ITestCaseRepository _testCaseRepository;
    private readonly IModuleRequirementRepository _moduleRequirementRepository;
    private readonly ILogger<ModuleReqTestCaseMappingRefresher> _logger;

    public ModuleReqTestCaseMappingRefresher(ITestCaseRepository testCaseRepository,
        IModuleRequirementRepository moduleRequirementRepository,
        ILogger<ModuleReqTestCaseMappingRefresher> logger)
    {
        _testCaseRepository = testCaseRepository;
        _moduleRequirementRepository = moduleRequirementRepository;
        _logger = logger;
    }

    public void Execute()
    {
        _logger.LogInformation("Refreshing Module Requirements/Test Case mapping.");

        var testCases = _testCaseRepository.GetAll();
        _logger.LogInformation($"Found {testCases.Length} Test Cases.");

        var moduleRequirements = _moduleRequirementRepository.GetAll();
        _logger.LogInformation($"Found {moduleRequirements.Length} Module Requirements.");

        _logger.LogInformation("Done Refreshing Module Requirements/Test Case mapping.");
    }
}