using Microsoft.Extensions.Logging;
using V.TcExtractor.Domain;
using V.TcExtractor.Domain.Model;
using V.TcExtractor.Domain.Refreshers;
using V.TcExtractor.Domain.Repositories;

namespace V.TcExtractor.Application;

public class ModuleReqTestCaseMappingRefresher : IModuleReqTestCaseMappingRefresher
{
    private readonly ITestCaseRepository _testCaseRepository;
    private readonly IModuleRequirementRepository _moduleRequirementRepository;
    private readonly ITestCaseRequirementMatcher _testCaseRequirementMatcher;
    private readonly IMatch1Repository _match1Repository;
    private readonly ILogger<ModuleReqTestCaseMappingRefresher> _logger;

    public ModuleReqTestCaseMappingRefresher(ITestCaseRepository testCaseRepository,
        IModuleRequirementRepository moduleRequirementRepository,
        ITestCaseRequirementMatcher testCaseRequirementMatcher,
        IMatch1Repository match1Repository,
        ILogger<ModuleReqTestCaseMappingRefresher> logger)
    {
        _testCaseRepository = testCaseRepository;
        _moduleRequirementRepository = moduleRequirementRepository;
        _testCaseRequirementMatcher = testCaseRequirementMatcher;
        _match1Repository = match1Repository;
        _logger = logger;
    }

    public void Execute()
    {
        _logger.LogInformation("Refreshing Module Requirements/Test Case mapping.");

        var testCases = _testCaseRepository.GetAll();
        _logger.LogInformation($"Found {testCases.Length} Test Cases.");

        var moduleRequirements = _moduleRequirementRepository.GetAll();
        _logger.LogInformation($"Found {moduleRequirements.Length} Module Requirements.");

        var matches = new List<Match1>();
        foreach (var moduleRequirement in moduleRequirements)
        {
            var matchingTestCases = testCases
                .Where(x => _testCaseRequirementMatcher.IsMatch(moduleRequirement, x))
                .ToArray();
            if (matchingTestCases.Any())
            {
                matches.Add(
                    new Match1
                    {
                        ModuleRequirementId = moduleRequirement.Id,
                        TestCases = string.Join(';',
                            matchingTestCases.Select(x => $"{x.TestNo}:{x.FileName}/{x.DmsNumber}"))
                    }
                );
            }
        }

        _match1Repository.DeleteAll();
        _match1Repository.AddRange(matches.ToArray());

        _logger.LogInformation($"Done Refreshing Module Requirements/Test Case mapping: {matches.Count}");
    }
}