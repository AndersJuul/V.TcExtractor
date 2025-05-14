using Microsoft.Extensions.Logging;
using V.TcExtractor.Domain.Refreshers;
using V.TcExtractor.Domain.Repositories;
using V.TcExtractor.Infrastructure.OfficeDocuments;

namespace V.TcExtractor.Application;

public class TestResultRefresher : ITestResultRefresher
{
    private readonly IFolderScanner _folderScanner;
    private readonly ITestResultRepository _testResultRepository;
    private readonly ILogger<TestResultRefresher> _logger;

    public TestResultRefresher(IFolderScanner folderScanner, ITestResultRepository testResultRepository,
        ILogger<TestResultRefresher> logger)
    {
        _folderScanner = folderScanner;
        _testResultRepository = testResultRepository;
        _logger = logger;
    }

    public void Execute()
    {
        _logger.LogInformation("Refreshing Test Results.");

        var testResults = _folderScanner
            .GetTestResults()
            .ToArray();

        _testResultRepository.DeleteAll();
        _testResultRepository.AddRange(testResults);

        _logger.LogInformation("Done Refreshing Test Results: " + testResults.Length);
    }
}