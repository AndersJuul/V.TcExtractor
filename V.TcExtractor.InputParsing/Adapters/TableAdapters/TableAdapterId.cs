using DocumentFormat.OpenXml.Wordprocessing;
using V.TcExtractor.Model;

namespace V.TcExtractor.InputParsing.Adapters.TableAdapters;

public class TableAdapterId(ICellAdapter cellAdapter) : ITableAdapter
{
    private readonly ICellAdapter _cellAdapter = cellAdapter;

    public bool CanHandle(Table table)
    {
        var firstRow = table.Elements<TableRow>().FirstOrDefault();

        if (firstRow != null)
        {
            var cellText = _cellAdapter.GetCellText(firstRow.Elements<TableCell>().FirstOrDefault());
            if (cellText.StartsWith("ID:"))
            {
                return true;
            }
        }

        return false;
    }

    public IEnumerable<TestCase> GetTestCases(Table table, string filePath)
    {
        var testCase = new TestCase
        {
            FileName = filePath,
            TestNo = GetLastPartOfCellString(table, "ID:").Replace(" ", ""),
            ReqId = GetLastPartOfCellString(table, "DVPL ID:").Replace(" ", ""),
            Description = GetLastPartOfCellString(table, "Case Description:")
        };

        yield return testCase;
    }

    private string GetLastPartOfCellString(Table table, string id)
    {
        var row = table.Elements<TableRow>().FirstOrDefault(x =>
        {
            var cellText = _cellAdapter.GetCellText(x.Elements<TableCell>().FirstOrDefault());
            return cellText.StartsWith(id);
        });

        if (row != null)
        {
            var cellText = _cellAdapter.GetCellText(row.Elements<TableCell>().FirstOrDefault());
            if (cellText.StartsWith(id))
            {
                return cellText.Substring(id.Length).Trim();
            }
        }

        return string.Empty;
    }
}