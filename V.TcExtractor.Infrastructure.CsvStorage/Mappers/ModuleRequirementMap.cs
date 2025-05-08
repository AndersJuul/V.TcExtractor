using CsvHelper.Configuration;
using V.TcExtractor.Domain.Model;

namespace V.TcExtractor.Infrastructure.CsvStorage.Mappers;

public class ModuleRequirementMap : ClassMap<ModuleRequirement>
{
    public ModuleRequirementMap()
    {
        Map(m => m.Id).Name("Id").Index(0);
        Map(m => m.ProductRequirement).Name("ProductRequirement").Index(1);
        Map(m => m.RsTitle).Name("RsTitle").Index(2);
        Map(m => m.CombinedRequirement).Name("CombinedRequirement").Index(3);
        Map(m => m.Motivation).Name("Motivation").Index(4);
        Map(m => m.FileName).Name("FileName").Index(5);
        Map(m => m.Source).Name("Source").Index(6);
    }
}