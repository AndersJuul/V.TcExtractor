using DocumentFormat.OpenXml.Wordprocessing;
using V.TcExtractor.Model;

namespace V.TcExtractor.InputParsing.Adapters.TableAdapters;

public class TableAdapterTestCaseInformationHeadersInRowZero(ICellAdapter cellAdapter) : ITableAdapter
{
    private readonly ICellAdapter _cellAdapter = cellAdapter;

    public bool CanHandle(Table table)
    {
        var firstRow = table.Elements<TableRow>().FirstOrDefault();

        if (firstRow != null)
        {
            var cells = firstRow.Elements<TableCell>().ToArray();
            if (!_cellAdapter.GetCellText(cells.FirstOrDefault()).Contains("Test Case Information"))
            {
                return false;
            }

            if (!_cellAdapter.GetCellText(cells.Skip(3).FirstOrDefault()).Contains("Req. ID"))
            {
                return false;
            }

            return true;
        }

        return false;
    }

    public IEnumerable<TestCase> GetTestCases(Table table, string filePath)
    {
        var testCase = new TestCase { FileName = filePath };

        // Process rows to find Test No, Description, and Req ID
        foreach (var row in table.Elements<TableRow>())
        {
            var cells = row.Elements<TableCell>().ToList();
            if (cells.Count >= 2)
            {
                var headerCell = _cellAdapter.GetCellText(cells[0]);

                // Extract Test No
                if (headerCell.Contains("Test No"))
                {
                    testCase.TestNo = _cellAdapter.GetCellText(cells[1]).Trim();
                    testCase.ReqId = _cellAdapter.GetCellText(cells[4]).Trim();
                }
                // Extract Description
                else if (headerCell.Contains("Description"))
                {
                    testCase.Description = _cellAdapter.GetCellText(cells[1]).Trim();
                }
            }
        }

        yield return testCase;
    }
}