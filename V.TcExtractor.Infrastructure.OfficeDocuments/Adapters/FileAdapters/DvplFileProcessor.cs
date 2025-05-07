using ClosedXML.Excel;
using V.TcExtractor.Domain.Model;

namespace V.TcExtractor.Infrastructure.OfficeDocuments.Adapters.FileAdapters;

public class DvplFileProcessor : IDvplFileProcessor
{
    public bool CanHandle(string fileName)
    {
        var nameWithoutPath = Path.GetFileName(fileName);
        return nameWithoutPath.Contains("DVPL");
    }

    public IEnumerable<DvplItem> GetDvplItems(string fileName)
    {
        using (var workbook = new XLWorkbook(fileName))
        {
            var worksheet = workbook.Worksheet(1);
            var range = worksheet.RangeUsed();

            foreach (var row in range.Rows().Skip(2))
            {
                var id = row.Cell(1).GetString();
                if (!string.IsNullOrWhiteSpace(id))
                    yield return new DvplItem
                    {
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