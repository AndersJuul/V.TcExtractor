using Microsoft.Extensions.Logging;
using V.TcExtractor.Domain.Model;
using V.TcExtractor.Domain.Refreshers;
using V.TcExtractor.Domain.Repositories;

namespace V.TcExtractor.Application;

public class BigJoinRefresher : IBigJoinRefresher
{
    private readonly ITestCaseRepository _testCaseRepository;
    private readonly IModuleRequirementRepository _moduleRequirementRepository;
    private readonly IDvplItemRepository _dvplItemRepository;
    private readonly IBigJoinRepository _bigJoinRepository;
    private readonly ILogger<BigJoinRefresher> _logger;

    public BigJoinRefresher(ITestCaseRepository testCaseRepository,
        IModuleRequirementRepository moduleRequirementRepository,
        IDvplItemRepository dvplItemRepository,
        IBigJoinRepository bigJoinRepository,
        ILogger<BigJoinRefresher> logger)
    {
        _testCaseRepository = testCaseRepository;
        _moduleRequirementRepository = moduleRequirementRepository;
        _dvplItemRepository = dvplItemRepository;
        _bigJoinRepository = bigJoinRepository;
        _logger = logger;
    }

    public void Execute()
    {
        _logger.LogInformation("Refreshing Big Joins.");


        var testCases = _testCaseRepository.GetAll();
        var moduleRequirements = _moduleRequirementRepository.GetAll();
        var dvpls = _dvplItemRepository.GetAll();

        var bigJoins = new List<BigJoin>();

        foreach (var dvplItem in dvpls)
        {
            foreach (var moduleRequirement in moduleRequirements.Where(x =>
                         x.ProductRequirement.Contains(dvplItem.ProductRsCode)))
            {
                foreach (var testCase in testCases.Where(x => x.ReqId.Contains(moduleRequirement.Id)))
                {
                    bigJoins.Add(
                        new BigJoin
                        {
                            ProductRsCode = dvplItem.ProductRsCode,
                            TestLocation = dvplItem.TestLocation,
                            ModuleRequirementId = moduleRequirement.Id,
                            TestNo = testCase.TestNo,
                            TestCaseFileName = testCase.FileName,
                            TestCaseDmsNumber = testCase.DmsNumber
                        }
                    );
                }
            }
        }


        _bigJoinRepository.DeleteAll();
        _bigJoinRepository.AddRange(bigJoins.ToArray());

        _logger.LogInformation("Done Refreshing Big Joins: " + bigJoins.Count);
    }
}