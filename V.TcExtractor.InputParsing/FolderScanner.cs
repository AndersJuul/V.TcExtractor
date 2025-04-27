using Ardalis.GuardClauses;
using V.TcExtractor.InputParsing.Adapters.FileAdapters;
using V.TcExtractor.Model;

namespace V.TcExtractor.InputParsing;

public class FolderScanner : IFolderScanner
{
    private readonly IEnumerable<IFileProcessor> _fileProcessors;

    public FolderScanner(IEnumerable<IFileProcessor> fileProcessors)
    {
        _fileProcessors = fileProcessors;
    }

    public IEnumerable<TestCase> GetTestCases(string pathToFiles)
    {
        // Width first search: Files then recurse through folders.
        var files = Directory.EnumerateFiles(pathToFiles);
        foreach (var file in files)
        {
            var processors = _fileProcessors.Where(xx => xx.CanHandle(file));

            foreach (var processor in processors)
            {
                foreach (var testCase in processor.Handle(file)) yield return testCase;
            }
        }

        var directories = Directory.EnumerateDirectories(pathToFiles);
        foreach (var directory in directories)
        {
            foreach (var testCase in GetTestCases(directory)) yield return testCase;
        }
    }
}