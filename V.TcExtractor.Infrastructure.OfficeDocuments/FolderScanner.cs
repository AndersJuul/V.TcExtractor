using Microsoft.Extensions.Options;
using V.TcExtractor.Domain.Model;
using V.TcExtractor.Domain.Options;
using V.TcExtractor.Domain.Processors;
using V.TcExtractor.Infrastructure.OfficeDocuments.Adapters.FileAdapters;

namespace V.TcExtractor.Infrastructure.OfficeDocuments;

public class FolderScanner : IFolderScanner
{
    private readonly IEnumerable<ITestCaseFileProcessor> _testFileProcessors;
    private readonly IEnumerable<IModuleRequirementFileProcessor> _moduleRequirementFileProcessors;
    private readonly FileLocationOptions _fileLocationOptions;
    private readonly IEnumerable<IDvplFileProcessor> _dvplFileProcessors;
    private readonly IEnumerable<ITestResultProcessor> _testResultProcessors;

    public FolderScanner(IEnumerable<ITestCaseFileProcessor> testFileProcessors,
        IEnumerable<IModuleRequirementFileProcessor> moduleRequirementFileProcessors,
        IEnumerable<IDvplFileProcessor> dvplFileProcessors,
        IEnumerable<ITestResultProcessor> testResultProcessors,
        IOptions<FileLocationOptions> fileLocationOptions)
    {
        if (testFileProcessors == null || !testFileProcessors.Any())
            throw new ArgumentException("testFileProcessors not specified. Must have at least one.");
        if (moduleRequirementFileProcessors == null || !moduleRequirementFileProcessors.Any())
            throw new ArgumentException("moduleRequirementFileProcessors not specified. Must have at least one.");
        if (dvplFileProcessors == null || !dvplFileProcessors.Any())
            throw new ArgumentException("dvplFileProcessors not specified. Must have at least one.");

        _testFileProcessors = testFileProcessors;
        _moduleRequirementFileProcessors = moduleRequirementFileProcessors;
        _dvplFileProcessors = dvplFileProcessors;
        _testResultProcessors = testResultProcessors;
        _fileLocationOptions = fileLocationOptions.Value;
    }

    public IEnumerable<string> GetFiles(string folder, string searchPattern)
    {
        var files = Directory.EnumerateFiles(folder, searchPattern);
        foreach (var file in files)
        {
            yield return file;
        }

        var directories = Directory.EnumerateDirectories(folder);
        foreach (var directory in directories)
        {
            foreach (var file in GetFiles(directory, searchPattern)) yield return file;
        }
    }

    public IEnumerable<TestCase> GetTestCases()
    {
        foreach (var fileName in GetFiles(_fileLocationOptions.Path, "*.docx"))
        {
            var processors = _testFileProcessors.Where(xx => xx.CanHandle(fileName));

            foreach (var processor in processors)
            {
                foreach (var testCase in processor.GetTestCases(fileName)) yield return testCase;
            }
        }
    }

    public IEnumerable<ModuleRequirement> GetModuleRequirements()
    {
        foreach (var fileName in GetFiles(_fileLocationOptions.Path, "*.xlsx"))
        {
            var processors = _moduleRequirementFileProcessors.Where(xx => xx.CanHandle(fileName));

            foreach (var processor in processors)
            {
                foreach (var testCase in processor.GetModuleRequirements(fileName)) yield return testCase;
            }
        }
    }

    public IEnumerable<DvplItem> GetDvplItems()
    {
        foreach (var fileName in GetFiles(_fileLocationOptions.Path, "*.xlsx"))
        {
            var processors = _dvplFileProcessors.Where(xx => xx.CanHandle(fileName));

            foreach (var processor in processors)
            {
                foreach (var dvplItem in processor.GetDvplItems(fileName)) yield return dvplItem;
            }
        }
    }

    public IEnumerable<TestResult> GetTestResults()
    {
        foreach (var fileName in GetFiles(_fileLocationOptions.Path, "*.docx"))
        {
            var processors = _testResultProcessors.Where(xx => xx.CanHandle(fileName));

            foreach (var processor in processors)
            {
                foreach (var testResult in processor.GetTestResults(fileName)) yield return testResult;
            }
        }
    }
}