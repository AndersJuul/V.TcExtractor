using CsvHelper.Configuration;
using V.TcExtractor.Domain.Model;

namespace V.TcExtractor.Infrastructure.CsvStorage.Mappers;

public class DvplItemMap : ClassMap<DvplItem>
{
    public DvplItemMap()
    {
        Map(m => m.ProductRsCode).Name("ModuleRsCode").Index(0);
        Map(m => m.ModuleRsCode).Name("ModuleRsCode").Index(1);
        Map(m => m.TestLocation).Name("ModuleRsCode").Index(2);
        Map(m => m.FileName).Name("File Name").Index(3);
    }
}