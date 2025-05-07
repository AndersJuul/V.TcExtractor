using CsvHelper.Configuration;
using V.TcExtractor.Domain.Model;

namespace V.TcExtractor.Infrastructure.CsvStorage.Mappers;

public class DvplItemMap : ClassMap<DvplItem>
{
    public DvplItemMap()
    {
        //Map(m => m.TestNo).Name("Test Number").Index(0);
        //Map(m => m.ReqId).Name("Requirement ID").Index(1);
        Map(m => m.FileName).Name("File Name").Index(0);
        //Map(m => m.Description).Name("Description").Index(3);
        //Map(m => m.DmsNumber).Name("DmsNumber").Index(4);
    }
}