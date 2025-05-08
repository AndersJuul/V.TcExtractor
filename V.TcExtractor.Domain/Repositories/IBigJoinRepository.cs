using V.TcExtractor.Domain.Model;

namespace V.TcExtractor.Domain.Repositories;

public interface IBigJoinRepository
{
    void DeleteAll();
    void AddRange(BigJoin[] bigJoins);
    BigJoin[] GetAll();
}