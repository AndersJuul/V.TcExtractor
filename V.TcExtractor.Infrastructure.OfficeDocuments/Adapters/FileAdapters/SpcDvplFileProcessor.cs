using ClosedXML.Excel;
using V.TcExtractor.Domain.Model;

namespace V.TcExtractor.Infrastructure.OfficeDocuments.Adapters.FileAdapters;

public class SpcDvplFileProcessor : IDvplFileProcessor
{
    public bool CanHandle(string fileName)
    {
        var nameWithoutPath = Path.GetFileName(fileName);
        return nameWithoutPath.Contains("DVPL")
               && (nameWithoutPath.Contains("SPN") || nameWithoutPath.Contains("SPC"));
    }

    public IEnumerable<DvplItem> GetDvplItems(string fileName)
    {
        using (var workbook = new XLWorkbook(fileName))
        {
            var worksheet = workbook.Worksheet(1);
            var range = worksheet.RangeUsed() ??
                        throw new NullReferenceException("Not able to get RangeUsed from worksheet.");

            foreach (var row in range.Rows().Skip(3))
            {
                var productRsCode = row.Cell(6).GetString();
                var moduleRsCode = row.Cell(3).GetString();
                var testLocation = row.Cell(17).GetString();
                yield return new DvplItem
                {
                    ProductRsCode = productRsCode,
                    ModuleRsCode = moduleRsCode,
                    TestLocation = testLocation,
                    FileName = fileName,
                };
            }
        }
    }
}