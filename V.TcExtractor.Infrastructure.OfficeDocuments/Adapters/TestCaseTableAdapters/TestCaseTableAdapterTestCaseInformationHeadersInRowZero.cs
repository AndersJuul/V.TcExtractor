using DocumentFormat.OpenXml.Wordprocessing;
using V.TcExtractor.Domain.Model;
using V.TcExtractor.Infrastructure.OfficeDocuments.Adapters.CellAdapters;

namespace V.TcExtractor.Infrastructure.OfficeDocuments.Adapters.TestCaseTableAdapters;

public class TestCaseTableAdapterTestCaseInformationHeadersInRowZero(ICellAdapter cellAdapter) : ITestCaseTableAdapter
{
    public bool CanHandle(Table table)
    {
        var firstRow = table.Elements<TableRow>().FirstOrDefault();

        if (firstRow != null)
        {
            var cells = firstRow.Elements<TableCell>().ToArray();
            if (!cellAdapter.GetCellText(cells.FirstOrDefault()).Contains("Test Case Information"))
            {
                return false;
            }

            if (!cellAdapter.GetCellText(cells.Skip(3).FirstOrDefault()).Contains("Req. ID"))
            {
                return false;
            }

            return true;
        }

        return false;
    }

    public IEnumerable<TestCase> GetTestCases(Table table, string filePath, string dmsNumber)
    {
        var testCase = new TestCase
        {
            FileName = filePath,
            DmsNumber = dmsNumber
        };

        // Process rows to find Test No, Description, and Req ID
        foreach (var row in table.Elements<TableRow>())
        {
            var cells = row.Elements<TableCell>().ToList();
            if (cells.Count >= 2)
            {
                var headerCell = cellAdapter.GetCellText(cells[0]);

                // Extract Test No
                if (headerCell.Contains("Test No"))
                {
                    testCase.TestNo = cellAdapter.GetCellText(cells[1]).Replace(" ", "").Trim();
                    testCase.ReqId = cellAdapter.GetCellText(cells[4]).Trim();
                }
                // Extract Description
                else if (headerCell.Contains("Description"))
                {
                    testCase.Description = cellAdapter.GetCellText(cells[1]).Trim();
                }
            }
        }

        yield return testCase;
    }
}