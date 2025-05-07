using CsvHelper.Configuration;
using V.TcExtractor.Domain.Model;

namespace V.TcExtractor.Infrastructure.CsvStorage.Mappers;

public class Match1Map : ClassMap<Match1>
{
    public Match1Map()
    {
        Map(m => m.ModuleRequirement.Id).Name("ModuleReqId").Index(0);
    }
}