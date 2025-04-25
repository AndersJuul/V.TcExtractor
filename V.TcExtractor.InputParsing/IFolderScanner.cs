using V.TcExtractor.InputParsing.Model;

namespace V.TcExtractor.InputParsing;

public interface IFolderScanner
{
    IEnumerable<TestCase> Scan(string[] args);
}