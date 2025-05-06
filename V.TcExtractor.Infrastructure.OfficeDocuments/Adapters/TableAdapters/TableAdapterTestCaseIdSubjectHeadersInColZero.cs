using DocumentFormat.OpenXml.Wordprocessing;
using V.TcExtractor.Domain.Model;
using V.TcExtractor.Infrastructure.OfficeDocuments.Adapters.CellAdapters;
using Table = DocumentFormat.OpenXml.Wordprocessing.Table;

namespace V.TcExtractor.Infrastructure.OfficeDocuments.Adapters.TableAdapters;

public class TableAdapterTestCaseIdSubjectHeadersInColZero(ICellAdapter cellAdapter) : ITableAdapter
{
    public bool CanHandle(Table table)
    {
        // Get all the rows in the table
        var rows = table.Elements<TableRow>();

        if (rows.Any())
        {
            // Select the first cell of each row
            var firstColumnCells = rows
                .Select(row => row.Elements<TableCell>().FirstOrDefault())
                .Where(cell => cell != null)
                .ToArray();

            if (!cellAdapter.GetCellText(firstColumnCells.FirstOrDefault()).Contains("Test ID"))
            {
                return false;
            }

            if (!cellAdapter.GetCellText(firstColumnCells.Skip(1).FirstOrDefault()).Contains("Subject"))
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
                var headerCell = cellAdapter.GetCellText(cells[0]);
                var valueCell = cellAdapter.GetCellText(cells[1]);

                if (headerCell.Contains("Test ID"))
                {
                    testCase.TestNo = valueCell.Trim();
                }

                if (headerCell.Contains("Description"))
                {
                    testCase.Description = valueCell.Trim();
                }

                if (headerCell.Contains("Line in DVPL"))
                {
                    testCase.ReqId = valueCell.Trim();
                }
            }
        }

        yield return testCase;
    }
}