using CsvHelper.Configuration;
using V.TcExtractor.Domain.Model;

namespace V.TcExtractor.Infrastructure.CsvStorage.Mappers;

public class DvplItemMap : ClassMap<DvplItem>
{
    public DvplItemMap()
    {
        Map(m => m.FileName).Name("File Name").Index(0);
        Map(m => m.ModuleRsCode).Name("ModuleRsCode").Index(1);
    }
}