using Microsoft.Extensions.Logging;
using V.TcExtractor.Domain;

namespace V.TcExtractor.Application;

public class ModuleReqTestCaseMappingRefresher : IModuleReqTestCaseMappingRefresher
{
    private readonly ILogger<ModuleReqTestCaseMappingRefresher> _logger;

    public ModuleReqTestCaseMappingRefresher(ILogger<ModuleReqTestCaseMappingRefresher> logger)
    {
        _logger = logger;
    }

    public void Execute()
    {
        _logger.LogInformation("Refreshing Module Requirements/Test Case mapping.");
    }
}