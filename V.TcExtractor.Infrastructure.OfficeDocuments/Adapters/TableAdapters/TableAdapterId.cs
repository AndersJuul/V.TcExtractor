using DocumentFormat.OpenXml.Wordprocessing;
using V.TcExtractor.Domain.Model;
using V.TcExtractor.Infrastructure.OfficeDocuments.Adapters.CellAdapters;

namespace V.TcExtractor.Infrastructure.OfficeDocuments.Adapters.TableAdapters;

public class TableAdapterId(ICellAdapter cellAdapter) : ITableAdapter
{
    public bool CanHandle(Table table)
    {
        var firstRow = table.Elements<TableRow>().FirstOrDefault();

        if (firstRow != null)
        {
            var cellText = cellAdapter.GetCellText(firstRow.Elements<TableCell>().FirstOrDefault());
            if (cellText.StartsWith("ID:"))
            {
                return true;
            }
        }

        return false;
    }

    public IEnumerable<TestCase> GetTestCases(Table table, string filePath, string dmsNumber)
    {
        var testCase = new TestCase
        {
            FileName = filePath,
            DmsNumber = dmsNumber,
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
            var cellText = cellAdapter.GetCellText(x.Elements<TableCell>().FirstOrDefault());
            return cellText.StartsWith(id);
        });

        if (row != null)
        {
            var cellText = cellAdapter.GetCellText(row.Elements<TableCell>().FirstOrDefault());
            if (cellText.StartsWith(id))
            {
                return cellText.Substring(id.Length).Trim();
            }
        }

        return string.Empty;
    }
}