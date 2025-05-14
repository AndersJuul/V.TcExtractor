using ClosedXML.Excel;
using V.TcExtractor.Domain.Model;
using V.TcExtractor.Domain.Processors;

namespace V.TcExtractor.Infrastructure.OfficeDocuments.Adapters.FileAdapters;

public class ModuleRequirementFileProcessorPsi : ModuleRequirementFileProcessorBase, IModuleRequirementFileProcessor
{
    public bool CanHandle(string fileName)
    {
        var extension = Path.GetExtension(fileName);
        if (!extension.Equals(".xlsx", StringComparison.InvariantCultureIgnoreCase)) return false;

        if (!File.Exists(fileName)) return false;

        if (!fileName.Contains("PSI"))
            return false;

        using var workbook = new XLWorkbook(fileName);
        var worksheet = workbook.Worksheet(1);
        var range = worksheet.RangeUsed();

        return range != null && range.Row(1).Cell(1).GetString().Equals("ID");
    }

    public IEnumerable<ModuleRequirement> GetModuleRequirements(string fileName)
    {
        using var workbook = new XLWorkbook(fileName);
        var source = GetSource(fileName);
        var worksheet = workbook.Worksheet(1);
        var range = worksheet.RangeUsed() ??
                    throw new NullReferenceException("Not able to get RangeUsed from worksheet.");

        foreach (var row in range.Rows().Skip(2))
        {
            var id = row.Cell(1).GetString();
            var reqCode = row.Cell(3).GetString();
            var productRequirementLink = row.Cell(4).GetString();
            if (!string.IsNullOrWhiteSpace(id))
                yield return new ModuleRequirement
                {
                    Id = id,
                    ProductRequirement =
                        ExtractProductRequirementReferences(!string.IsNullOrEmpty(reqCode)
                            ? reqCode
                            : productRequirementLink),
                    RsTitle = row.Cell(5).GetString(),
                    CombinedRequirement = row.Cell(6).GetString(),
                    Motivation = row.Cell(12).GetString(),
                    FileName = fileName,
                    Source = source
                };
        }
    }
}