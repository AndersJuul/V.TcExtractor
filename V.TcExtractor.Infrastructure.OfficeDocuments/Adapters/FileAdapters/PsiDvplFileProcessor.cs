using ClosedXML.Excel;
using V.TcExtractor.Domain.Model;

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
        using (var workbook = new XLWorkbook(fileName))
        {
            var worksheet = workbook.Worksheet(1);
            var range = worksheet.RangeUsed();

            foreach (var row in range.Rows().Skip(3))
            {
                var id = row.Cell(1).GetString();
                var moduleRsCode = row.Cell(8).GetString();
                var testLocation = row.Cell(27).GetString();
                if (!string.IsNullOrWhiteSpace(id))
                {
                    yield return new DvplItem
                    {
                        ModuleRsCode = moduleRsCode,
                        TestLocation = testLocation,
                        //Id = id,
                        //RsTitle = row.Cell(5).GetString(),
                        //CombinedRequirement = row.Cell(6).GetString(),
                        //Motivation = row.Cell(12).GetString(),
                        FileName = fileName,
                    };
                }
            }
        }
    }
}