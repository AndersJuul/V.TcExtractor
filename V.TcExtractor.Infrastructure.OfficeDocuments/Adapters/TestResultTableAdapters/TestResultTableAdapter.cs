using DocumentFormat.OpenXml.Wordprocessing;
using V.TcExtractor.Domain.Adapters;
using V.TcExtractor.Domain.Model;
using V.TcExtractor.Infrastructure.OfficeDocuments.Adapters.CellAdapters;

namespace V.TcExtractor.Infrastructure.OfficeDocuments.Adapters.TestResultTableAdapters;

public class TestResultTableAdapter : ITestResultTableAdapter
{
    private readonly ICellAdapter _cellAdapter;
    private readonly IPassedTextAdapter _passedTextAdapter;

    public TestResultTableAdapter(ICellAdapter cellAdapter, IPassedTextAdapter passedTextAdapter)
    {
        _cellAdapter = cellAdapter;
        _passedTextAdapter = passedTextAdapter;
    }

    public bool CanHandle(Table table)
    {
        var firstRow = table.Elements<TableRow>().FirstOrDefault();

        if (firstRow != null)
        {
            var cellText = _cellAdapter.GetCellText(firstRow.Elements<TableCell>().FirstOrDefault());

            // Document found with two spaces between "Review" and "ID"
            while (cellText.Contains("  "))
            {
                cellText = cellText.Replace("  ", " ");
            }

            if (cellText.StartsWith("Test ID") || cellText.StartsWith("Review ID"))
            {
                return true;
            }
        }

        return false;
    }

    public IEnumerable<TestResult> GetTestResults(Table table, string fileName, string dmsNumber)
    {
        var firstRow = table.Elements<TableRow>().FirstOrDefault();
        var secondRow = table.Elements<TableRow>().Skip(1).FirstOrDefault();
        var seventhRow = table.Elements<TableRow>().Skip(6).FirstOrDefault();
        if (firstRow != null && secondRow != null && seventhRow != null)
        {
            var testId = _cellAdapter
                .GetCellText(firstRow.Elements<TableCell>().Skip(1).FirstOrDefault())
                .Replace(" ", "");
            var subject = _cellAdapter
                .GetCellText(secondRow.Elements<TableCell>().Skip(1).FirstOrDefault());
            var result = _cellAdapter
                .GetCellText(seventhRow.Elements<TableCell>().Skip(1).FirstOrDefault());
            var testResult = new TestResult
            {
                TestId = testId,
                Subject = subject,
                FileName = fileName,
                Result = result,
                Passed = _passedTextAdapter.GetPassedFromTestResult(result),
                DmsNumber = dmsNumber
            };

            yield return testResult;
        }
        else
        {
            //
        }
    }
}