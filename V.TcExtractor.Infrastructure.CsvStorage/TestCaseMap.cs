using CsvHelper.Configuration;
using V.TcExtractor.Domain.Model;

namespace V.TcExtractor.Infrastructure.CsvStorage;

public class TestCaseMap : ClassMap<TestCase>
{
    public TestCaseMap()
    {
        Map(m => m.TestNo).Name("Test Number").Index(0);
        Map(m => m.ReqId).Name("Requirement ID").Index(1);
        Map(m => m.FileName).Name("File Name").Index(2);
        Map(m => m.Description).Name("Description").Index(3);
    }
}