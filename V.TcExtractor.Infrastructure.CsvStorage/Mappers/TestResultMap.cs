using CsvHelper.Configuration;
using V.TcExtractor.Domain.Model;

namespace V.TcExtractor.Infrastructure.CsvStorage.Mappers;

public class TestResultMap : ClassMap<TestResult>
{
    public TestResultMap()
    {
        Map(m => m.TestId).Name("TestId").Index(0);
        Map(m => m.Passed).Name("Passed").Index(1);
        Map(m => m.Subject).Name("Subject").Index(2);
        Map(m => m.Result).Name("Result").Index(3);
        Map(m => m.FileName).Name("FileName").Index(4);
        Map(m => m.DmsNumber).Name("DmsNumber").Index(5);
    }
}