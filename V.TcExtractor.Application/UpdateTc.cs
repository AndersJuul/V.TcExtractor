using V.TcExtractor.Domain;
using V.TcExtractor.Infrastructure.OfficeDocuments;

namespace V.TcExtractor.Application;

public class UpdateTc : IUpdateTc
{
    private readonly IFolderScanner _folderScanner;
    private readonly ITestCaseRepository _testCaseRepository;

    public UpdateTc(IFolderScanner folderScanner, ITestCaseRepository testCaseRepository)
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