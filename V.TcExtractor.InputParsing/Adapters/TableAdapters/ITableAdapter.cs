using DocumentFormat.OpenXml.Wordprocessing;
using V.TcExtractor.Model;

namespace V.TcExtractor.InputParsing.Adapters.TableAdapters;

public interface ITableAdapter
{
    bool CanHandle(Table table);
    IEnumerable<TestCase> GetTestCases(Table table, string filePath);
}