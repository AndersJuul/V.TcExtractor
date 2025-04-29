using V.TcExtractor.InputParsing.Adapters.FileAdapters;
using V.TcExtractor.Model;

namespace V.TcExtractor.InputParsing;

public class FolderScanner : IFolderScanner
{
    private readonly IEnumerable<ITestCaseFileProcessor> _testFileProcessors;
    private readonly IEnumerable<IModuleRequirementFileProcessor> _moduleRequirementFileProcessor;

    public FolderScanner(IEnumerable<ITestCaseFileProcessor> testFileProcessors,
        IEnumerable<IModuleRequirementFileProcessor> moduleRequirementFileProcessor)
    {
        _testFileProcessors = testFileProcessors;
        _moduleRequirementFileProcessor = moduleRequirementFileProcessor;
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

    public IEnumerable<TestCase> GetTestCases(string pathToFiles)
    {
        foreach (var fileName in GetFiles(pathToFiles, "*.docx"))
        {
            var processors = _testFileProcessors.Where(xx => xx.CanHandle(fileName));

            foreach (var processor in processors)
            {
                foreach (var testCase in processor.GetTestCases(fileName)) yield return testCase;
            }
        }
    }

    public IEnumerable<ModuleRequirement> GetModuleRequirements(string pathToFiles)
    {
        foreach (var fileName in GetFiles(pathToFiles, "*.xlsx"))
        {
            var processors = _moduleRequirementFileProcessor.Where(xx => xx.CanHandle(fileName));

            foreach (var processor in processors)
            {
                foreach (var testCase in processor.GetModuleRequirements(fileName)) yield return testCase;
            }
        }
    }
}