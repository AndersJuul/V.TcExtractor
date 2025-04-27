using DocumentFormat.OpenXml.Wordprocessing;
using V.TcExtractor.Model;

namespace V.TcExtractor.InputParsing.Adapters.TableAdapters;

public class TableAdapterTestCaseIdHeadersInRowZero(ICellAdapter cellAdapter) : ITableAdapter
{
    private readonly ICellAdapter _cellAdapter = cellAdapter;

    public bool CanHandle(Table table)
    {
        var firstRow = table.Elements<TableRow>().FirstOrDefault();

        if (firstRow != null)
        {
            var cells = firstRow.Elements<TableCell>().ToArray();
            if (!_cellAdapter.GetCellText(cells.FirstOrDefault()).Contains("Test case ID"))
            {
                return false;
            }

            if (!_cellAdapter.GetCellText(cells.Skip(2).FirstOrDefault()).Contains("Initial Conditions"))
            {
                return false;
            }

            return true;
        }

        return false;
    }

    public IEnumerable<TestCase> GetTestCases(Table table, string filePath)
    {
        // Process rows to find Test No, Description, and Req ID
        foreach (var row in table.Elements<TableRow>().Skip(1))
        {
            var testCase = new TestCase { FileName = filePath };

            var cells = row.Elements<TableCell>().ToList();

            testCase.TestNo = _cellAdapter.GetCellText(cells[0]).Trim();
            testCase.Description = _cellAdapter.GetCellText(cells[1]).Trim();

            yield return testCase;
        }
    }
}