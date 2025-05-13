using V.TcExtractor.Domain.Model;

namespace V.TcExtractor.Infrastructure.OfficeDocuments;

public interface IFolderScanner
{
    IEnumerable<TestCase> GetTestCases();
    IEnumerable<ModuleRequirement> GetModuleRequirements();
    IEnumerable<DvplItem> GetDvplItems();
    IEnumerable<TestResult> GetTestResults();
}