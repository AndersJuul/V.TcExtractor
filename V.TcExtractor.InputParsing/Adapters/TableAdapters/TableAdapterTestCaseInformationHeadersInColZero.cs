using DocumentFormat.OpenXml.Spreadsheet;
using DocumentFormat.OpenXml.Wordprocessing;
using V.TcExtractor.InputParsing.Model;
using Table = DocumentFormat.OpenXml.Wordprocessing.Table;

namespace V.TcExtractor.InputParsing.Adapters.TableAdapters;

public class TableAdapterTestCaseInformationHeadersInColZero(ICellAdapter cellAdapter) : ITableAdapter
{
    private readonly ICellAdapter _cellAdapter = cellAdapter;

    public bool CanHandle(Table table)
    {
        var firstColumn = table.Elements<TableColumn>().FirstOrDefault();

        if (firstColumn != null)
        {
            var cells = firstColumn.Elements<TableCell>().ToArray();
            if (!_cellAdapter.GetCellText(cells.FirstOrDefault()).Contains("Test Case Information"))
            {
                return false;
            }

            if (!_cellAdapter.GetCellText(cells.FirstOrDefault()).Contains("Test Case Information"))
            {
                return false;
            }

            return true;
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
                var headerCell = _cellAdapter.GetCellText(cells[0]);
                var valueCell = _cellAdapter.GetCellText(cells[1]);

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
                else if (cells.Count >= 5)
                {
                    // Req ID is typically in the 5th column of the header row
                    testCase.ReqId = _cellAdapter.GetCellText(cells[4]).Trim();
                }
            }
        }

        return testCase;
    }
}