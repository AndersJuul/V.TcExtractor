using Microsoft.Extensions.Logging;
using V.TcExtractor.Domain.Model;
using V.TcExtractor.Domain.Refreshers;
using V.TcExtractor.Domain.Repositories;

namespace V.TcExtractor.Application;

public class BigJoinRefresher : IBigJoinRefresher
{
    private readonly ITestCaseRepository _testCaseRepository;
    private readonly IModuleRequirementRepository _moduleRequirementRepository;
    private readonly IBigJoinRepository _bigJoinRepository;
    private readonly ILogger<BigJoinRefresher> _logger;

    public BigJoinRefresher(ITestCaseRepository testCaseRepository,
        IModuleRequirementRepository moduleRequirementRepository,
        IBigJoinRepository bigJoinRepository,
        ILogger<BigJoinRefresher> logger)
    {
        _testCaseRepository = testCaseRepository;
        _moduleRequirementRepository = moduleRequirementRepository;
        _bigJoinRepository = bigJoinRepository;
        _logger = logger;
    }

    public void Execute()
    {
        _logger.LogInformation("Refreshing Big Joins.");


        var moduleRequirements = _moduleRequirementRepository.GetAll();
        var testCases = _testCaseRepository.GetAll();


        _bigJoinRepository.DeleteAll();
        _bigJoinRepository.AddRange(new BigJoin[] { });

        _logger.LogInformation("Done Refreshing Big Joins: " + testCases.Length);
    }
}