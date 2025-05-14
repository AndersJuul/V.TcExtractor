using ClosedXML.Excel;
using V.TcExtractor.Domain.Model;
using V.TcExtractor.Domain.Processors;

namespace V.TcExtractor.Infrastructure.OfficeDocuments.Adapters.FileAdapters;

public class PsiDvplFileProcessor : IDvplFileProcessor
{
    public bool CanHandle(string fileName)
    {
        var nameWithoutPath = Path.GetFileName(fileName);
        return nameWithoutPath.Contains("DVPL") && nameWithoutPath.Contains("PSI");
    }

    public IEnumerable<DvplItem> GetDvplItems(string fileName)
    {
        using var workbook = new XLWorkbook(fileName);
        var worksheet = workbook.Worksheet(1);
        var range = worksheet.RangeUsed() ??
                    throw new NullReferenceException("Not able to get RangeUsed from worksheet.");

        var productRsCode = "";
        var moduleRsCode = "";
        var testLocation = "";

        foreach (var row in range.Rows().Skip(3))
        {
            productRsCode = GetValueDefaultIfEmpty(row.Cell(4).GetString(), productRsCode);
            moduleRsCode = GetValueDefaultIfEmpty(row.Cell(8).GetString(), moduleRsCode);
            testLocation = GetValueDefaultIfEmpty(row.Cell(27).GetString(), testLocation);
            yield return new DvplItem
            {
                ProductRsCode = productRsCode,
                ModuleRsCode = GetCommaSeparatedRequirements(moduleRsCode),
                TestLocation = testLocation,
                FileName = fileName,
            };
        }
    }

    private string GetCommaSeparatedRequirements(string moduleRsCode)
    {
        var lines = moduleRsCode
            .Split(['\n'], StringSplitOptions.RemoveEmptyEntries)
            .Where(x => x.Contains("_"));
        return string.Join(',', lines);
    }

    private string GetValueDefaultIfEmpty(string newValue, string defaultValue)
    {
        if (!string.IsNullOrWhiteSpace(newValue))
            return newValue;

        return defaultValue;
    }
}