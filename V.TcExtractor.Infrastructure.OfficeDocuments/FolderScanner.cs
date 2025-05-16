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
    private readonly IEnumerable<ITestResultFileProcessor> _testResultProcessors;
    private readonly IEnumerable<IFileProcessor> _fileProcessors;

    public FolderScanner(IEnumerable<ITestCaseFileProcessor> testFileProcessors,
        IEnumerable<IModuleRequirementFileProcessor> moduleRequirementFileProcessors,
        IEnumerable<IDvplFileProcessor> dvplFileProcessors,
        IEnumerable<ITestResultFileProcessor> testResultProcessors,
        IOptions<FileLocationOptions> fileLocationOptions, IEnumerable<IFileProcessor> fileProcessors)
    {
        if (testFileProcessors == null || !testFileProcessors.Any())
            throw new ArgumentException("testFileProcessors not specified. Must have at least one.");
        if (testResultProcessors == null || !testResultProcessors.Any())
            throw new ArgumentException("testResultProcessors not specified. Must have at least one.");
        if (moduleRequirementFileProcessors == null || !moduleRequirementFileProcessors.Any())
            throw new ArgumentException("moduleRequirementFileProcessors not specified. Must have at least one.");
        if (dvplFileProcessors == null || !dvplFileProcessors.Any())
            throw new ArgumentException("dvplFileProcessors not specified. Must have at least one.");
        if (fileProcessors == null || !fileProcessors.Any())
            throw new ArgumentException("fileProcessors not specified. Must have at least one.");

        _testFileProcessors = testFileProcessors;
        _moduleRequirementFileProcessors = moduleRequirementFileProcessors;
        _dvplFileProcessors = dvplFileProcessors;
        _testResultProcessors = testResultProcessors;
        _fileProcessors = fileProcessors;
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

    public IEnumerable<FileItem> GetFileItems()
    {
        var wordDocuments = GetFiles(_fileLocationOptions.Path, "*.docx");
        var excelDocuments = GetFiles(_fileLocationOptions.Path, "*.xlsx");

        var allFiles = wordDocuments.Concat(excelDocuments);

        foreach (var file in allFiles)
        {
            var processor = _fileProcessors.FirstOrDefault(x => x.CanHandle(file));
            if (processor != null) yield return processor.GetFileItem(file);
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