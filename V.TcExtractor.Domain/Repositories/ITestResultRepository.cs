using V.TcExtractor.Domain.Model;

namespace V.TcExtractor.Domain.Repositories;

public interface ITestResultRepository
{
    void DeleteAll();
    void AddRange(TestResult[] testResults);
    TestResult[] GetAll();
}