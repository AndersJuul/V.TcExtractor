using Microsoft.Extensions.Logging;
using V.TcExtractor.Domain.Refreshers;
using V.TcExtractor.Domain.Repositories;
using V.TcExtractor.Infrastructure.OfficeDocuments;

namespace V.TcExtractor.Application;

public class TestCaseRefresher : ITestCaseRefresher
{
    private readonly IFolderScanner _folderScanner;
    private readonly ITestCaseRepository _testCaseRepository;
    private readonly ILogger<TestCaseRefresher> _logger;

    public TestCaseRefresher(IFolderScanner folderScanner, ITestCaseRepository testCaseRepository,
        ILogger<TestCaseRefresher> logger)
    {
        _folderScanner = folderScanner;
        _testCaseRepository = testCaseRepository;
        _logger = logger;
    }

    public void Execute()
    {
        _logger.LogInformation("Refreshing Test Cases.");

        var testCases = _folderScanner
            .GetTestCases()
            .ToArray();

        _testCaseRepository.DeleteAll();
        _testCaseRepository.AddRange(testCases);

        _logger.LogInformation("Done Refreshing Test Cases: " + testCases.Length);
    }
}