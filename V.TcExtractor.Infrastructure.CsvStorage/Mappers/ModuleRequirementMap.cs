using CsvHelper.Configuration;
using V.TcExtractor.Domain.Model;

namespace V.TcExtractor.Infrastructure.CsvStorage;

public class ModuleRequirementMap : ClassMap<ModuleRequirement>
{
    public ModuleRequirementMap()
    {
        Map(m => m.Id).Name("Id").Index(0);
        Map(m => m.RsTitle).Name("RsTitle").Index(1);
        Map(m => m.CombinedRequirement).Name("CombinedRequirement").Index(2);
        Map(m => m.Motivation).Name("Motivation").Index(3);
        Map(m => m.FileName).Name("FileName").Index(4);
    }
}