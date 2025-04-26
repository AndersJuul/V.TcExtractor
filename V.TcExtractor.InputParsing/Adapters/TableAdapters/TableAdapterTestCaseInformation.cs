using DocumentFormat.OpenXml.Wordprocessing;
using V.TcExtractor.InputParsing.Model;

namespace V.TcExtractor.InputParsing.Adapters.TableAdapters;

public class TableAdapterTestCaseInformation(ICellAdapter cellAdapter) : ITableAdapter
{
    private readonly ICellAdapter _cellAdapter = cellAdapter;

    public bool CanHandle(Table table)
    {
        var firstRow = table.Elements<TableRow>().FirstOrDefault();

        if (firstRow != null)
        {
            var cellText = _cellAdapter.GetCellText(firstRow.Elements<TableCell>().FirstOrDefault());
            if (cellText.Contains("Test Case Information"))
            {
                return true;
            }
        }

        return false;
    }

    public TestCase GetTestCase(Table table, string filePath)
    {
        var testCase = new TestCase { FileName = filePath };

        // Process rows to find Test No, Description, and Req ID
        foreach (var row in table.Elements<TableRow>())
        {
            var cells = row.Elements<TableCell>().ToList();
            if (cells.Count >= 2)
            {
                string headerCell = _cellAdapter.GetCellText(cells[0]);
                string valueCell = _cellAdapter.GetCellText(cells[1]);

                // Extract Test No
                if (headerCell.Contains("Test No"))
                {
                    testCase.TestNo = valueCell.Trim();
                }
                // Extract Description
                else if (headerCell.Contains("Description"))
                {
                    testCase.Description = valueCell.Trim();
                }
                // Extract Req ID
                else if (cells.Count >= 5 && headerCell.Contains("Test Case Information"))
                {
                    // Req ID is typically in the 5th column of the header row
                    if (cells.Count >= 5)
                    {
                        testCase.ReqId = _cellAdapter.GetCellText(cells[4]).Trim();
                    }
                }
            }
        }

        return testCase;
    }
}