using CsvHelper.Configuration;
using V.TcExtractor.Domain.Model;

namespace V.TcExtractor.Infrastructure.CsvStorage.Mappers;

public class BigJoinMap : ClassMap<BigJoin>
{
    public BigJoinMap()
    {
        Map(m => m.ProductRsCode).Name("ProductRsCode").Index(0);
        Map(m => m.TestLocation).Name("TestLocation").Index(1);
        Map(m => m.ModuleRequirementId).Name("ModuleRequirementId").Index(2);
        Map(m => m.TestNo).Name("TestNo").Index(3);
        Map(m => m.TestCaseFileName).Name("TestCaseFileName").Index(4);
        Map(m => m.TestCaseDmsNumber).Name("TestCaseDmsNumber").Index(5);
    }
}