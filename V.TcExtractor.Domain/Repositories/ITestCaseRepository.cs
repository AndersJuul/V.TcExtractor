using V.TcExtractor.Domain.Model;

namespace V.TcExtractor.Domain.Repositories;

public interface ITestCaseRepository
{
    void DeleteAll();
    void AddRange(TestCase[] testCases);
}