using V.TcExtractor.Domain.Model;

namespace V.TcExtractor.Infrastructure.OfficeDocuments.Adapters.FileAdapters;

public abstract class ModuleRequirementFileProcessorBase
{
    protected RequirementSource GetSource(string fileName)
    {
        if (fileName.Contains("PSI")) return RequirementSource.PSI;
        if (fileName.Contains("SPC")) return RequirementSource.SPC;

        return RequirementSource.Unknown;
    }

    protected string ExtractProductRequirementReferences(string productRequirementText)
    {
        string[] parts = productRequirementText.Split(new[] { " ", "\t", "\n", ",", "&", Environment.NewLine },
            StringSplitOptions.RemoveEmptyEntries);

        // Filter to only keep items starting with VPP_
        var productRequirementReferences = parts.Where(p => p.StartsWith("VPP_")).ToList();
        return string.Join(",", productRequirementReferences);
    }
}