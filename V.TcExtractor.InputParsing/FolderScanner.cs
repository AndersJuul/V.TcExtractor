using Ardalis.GuardClauses;
using V.TcExtractor.InputParsing.Adapters.FileAdapters;
using V.TcExtractor.InputParsing.Model;

namespace V.TcExtractor.InputParsing;

public class FolderScanner : IFolderScanner
{
    private readonly IEnumerable<IFileProcessor> _fileProcessors;

    public FolderScanner(IEnumerable<IFileProcessor> fileProcessors)
    {
        _fileProcessors = fileProcessors;
    }

    public IEnumerable<TestCase> Scan(string[] args)
    {
        Guard.Against.Expression(x => x != 1, args.Length,
            "args must be single arg, specifying path to a folder with TC Files");
        return ScanFolder(args.Single());
    }

    private IEnumerable<TestCase> ScanFolder(string path)
    {
        // Width first search: Files then recurse through folders.
        var files = Directory.EnumerateFiles(path);
        foreach (var file in files)
        {
            var processors = _fileProcessors.Where(xx => xx.CanHandle(file));

            foreach (var processor in processors)
            {
                foreach (var testCase in processor.Handle(file)) yield return testCase;
            }
        }

        var directories = Directory.EnumerateDirectories(path);
        foreach (var directory in directories)
        {
            foreach (var testCase in ScanFolder(directory)) yield return testCase;
        }
    }
}