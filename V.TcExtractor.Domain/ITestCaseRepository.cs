using V.TcExtractor.Domain.Model;

namespace V.TcExtractor.Domain;

public interface ITestCaseRepository
{
    void DeleteAll();
    void AddRange(TestCase[] testCases);
}