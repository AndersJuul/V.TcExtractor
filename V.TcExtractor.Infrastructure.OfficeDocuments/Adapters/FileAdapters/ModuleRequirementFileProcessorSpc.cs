using ClosedXML.Excel;
using V.TcExtractor.Domain.Model;

namespace V.TcExtractor.Infrastructure.OfficeDocuments.Adapters.FileAdapters;

public class ModuleRequirementFileProcessorSpc : ModuleRequirementFileProcessorBase, IModuleRequirementFileProcessor
{
    public bool CanHandle(string fileName)
    {
        var extension = Path.GetExtension(fileName);
        if (!extension.Equals(".xlsx", StringComparison.InvariantCultureIgnoreCase)) return false;

        if (!File.Exists(fileName)) return false;

        if (!fileName.Contains("SPC"))
            return false;

        using var workbook = new XLWorkbook(fileName);
        var worksheet = workbook.Worksheet(4);
        var range = worksheet.RangeUsed();

        if (range == null) return false;
        var colHeader = range.Row(1).Cell(1).GetString();
        return colHeader == "ID";
    }

    public IEnumerable<ModuleRequirement> GetModuleRequirements(string fileName)
    {
        using var workbook = new XLWorkbook(fileName);
        var source = GetSource(fileName);
        var worksheet = workbook.Worksheet(4);
        var range = worksheet.RangeUsed() ??
                    throw new NullReferenceException("Not able to get RangeUsed from worksheet.");

        foreach (var row in range.Rows().Skip(1))
        {
            var id = row.Cell(1).GetString();
            var productRequirementLink = row.Cell(3).GetString();
            if (!string.IsNullOrWhiteSpace(id))
                yield return new ModuleRequirement
                {
                    Id = id,
                    ProductRequirement = ExtractProductRequirementReferences(productRequirementLink),
                    RsTitle = row.Cell(6).GetString(),
                    CombinedRequirement = row.Cell(7).GetString(),
                    Motivation = row.Cell(8).GetString(),
                    FileName = fileName,
                    Source = source
                };
        }
    }
}