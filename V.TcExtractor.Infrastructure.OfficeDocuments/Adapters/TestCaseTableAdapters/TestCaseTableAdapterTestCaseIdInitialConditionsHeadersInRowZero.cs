using DocumentFormat.OpenXml.Wordprocessing;
using V.TcExtractor.Domain.Model;
using V.TcExtractor.Infrastructure.OfficeDocuments.Adapters.CellAdapters;

namespace V.TcExtractor.Infrastructure.OfficeDocuments.Adapters.TestCaseTableAdapters;

public class TestCaseTableAdapterTestCaseIdInitialConditionsHeadersInRowZero(ICellAdapter cellAdapter)
    : ITestCaseTableAdapter
{
    public bool CanHandle(Table table)
    {
        var firstRow = table.Elements<TableRow>().FirstOrDefault();

        if (firstRow != null)
        {
            var cells = firstRow.Elements<TableCell>().ToArray();
            if (!cellAdapter.GetCellText(cells.FirstOrDefault()).Contains("Test case ID"))
            {
                return false;
            }

            if (!cellAdapter.GetCellText(cells.Skip(1).FirstOrDefault()).Contains("Test Description"))
            {
                return false;
            }

            if (!cellAdapter.GetCellText(cells.Skip(2).FirstOrDefault()).Contains("Initial Conditions"))
            {
                return false;
            }

            return true;
        }

        return false;
    }

    public IEnumerable<TestCase> GetTestCases(Table table, string filePath, string dmsNumber)
    {
        // Process rows to find Test No, Description, and Req ID
        var desc = "";
        foreach (var row in table.Elements<TableRow>().Skip(1))
        {
            var testCase = new TestCase
            {
                FileName = filePath,
                DmsNumber = dmsNumber
            };

            var cells = row.Elements<TableCell>().ToList();

            if (string.IsNullOrEmpty(cellAdapter.GetCellText(cells[0]).Trim()))
                continue;

            testCase.TestNo = cellAdapter.GetCellText(cells[0]).Replace(" ", "").Trim();

            if (!string.IsNullOrEmpty(cellAdapter.GetCellText(cells[1]).Trim()))
                desc = cellAdapter.GetCellText(cells[1]).Trim();
            testCase.Description = desc;

            yield return testCase;
        }
    }
}