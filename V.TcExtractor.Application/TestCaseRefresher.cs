using V.TcExtractor.Domain;
using V.TcExtractor.Domain.Repositories;
using V.TcExtractor.Infrastructure.OfficeDocuments;

namespace V.TcExtractor.Application;

public class TestCaseRefresher : ITestCaseRefresher
{
    private readonly IFolderScanner _folderScanner;
    private readonly ITestCaseRepository _testCaseRepository;

    public TestCaseRefresher(IFolderScanner folderScanner, ITestCaseRepository testCaseRepository)
    {
        _folderScanner = folderScanner;
        _testCaseRepository = testCaseRepository;
    }

    public void Execute()
    {
        var testCases = _folderScanner
            .GetTestCases()
            .ToArray();

        _testCaseRepository.DeleteAll();
        _testCaseRepository.AddRange(testCases);
    }
}