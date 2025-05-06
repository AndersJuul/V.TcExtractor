using Microsoft.Extensions.Options;
using V.TcExtractor.Domain.Model;
using V.TcExtractor.Domain.Options;
using V.TcExtractor.Infrastructure.OfficeDocuments.Adapters.FileAdapters;

namespace V.TcExtractor.Infrastructure.OfficeDocuments;

public class FolderScanner : IFolderScanner
{
    private readonly IEnumerable<ITestCaseFileProcessor> _testFileProcessors;
    private readonly IEnumerable<IModuleRequirementFileProcessor> _moduleRequirementFileProcessor;
    private readonly FileLocationOptions _fileLocationOptions;

    public FolderScanner(IEnumerable<ITestCaseFileProcessor> testFileProcessors,
        IEnumerable<IModuleRequirementFileProcessor> moduleRequirementFileProcessor,
        IOptions<FileLocationOptions> fileLocationOptions)
    {
        _testFileProcessors = testFileProcessors;
        _moduleRequirementFileProcessor = moduleRequirementFileProcessor;
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
            var processors = _moduleRequirementFileProcessor.Where(xx => xx.CanHandle(fileName));

            foreach (var processor in processors)
            {
                foreach (var testCase in processor.GetModuleRequirements(fileName)) yield return testCase;
            }
        }
    }
}