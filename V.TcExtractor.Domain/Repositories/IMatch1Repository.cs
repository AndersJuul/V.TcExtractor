using V.TcExtractor.Domain.Model;

namespace V.TcExtractor.Domain.Repositories;

public interface IMatch1Repository
{
    void DeleteAll();
    void AddRange(Match1[] matches);
    Match1[] GetAll();
}