using V.TcExtractor.InputParsing.Model;

namespace V.TcExtractor.InputParsing;

public interface IFileProcessor
{
    bool CanHandle(string fileName);
    List<TestCase> Handle(string file);
}