using DocumentFormat.OpenXml.Wordprocessing;
using V.TcExtractor.InputParsing.Model;

namespace V.TcExtractor.InputParsing.Adapters.TableAdapters;

public interface ITableAdapter
{
    bool CanHandle(Table table);
    TestCase? GetTestCase(Table table, string filePath);
}