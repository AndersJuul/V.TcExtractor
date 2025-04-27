using V.TcExtractor.Model;

namespace V.TcExtractor.InputParsing.Adapters.FileAdapters;

public interface IFileProcessor
{
    bool CanHandle(string fileName);
    List<TestCase> Handle(string file);
}