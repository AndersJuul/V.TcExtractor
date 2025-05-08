using V.TcExtractor.Application.Tests;

namespace V.TcExtractor.Domain.Repositories;

public interface IBigJoinRepository
{
    void DeleteAll();
    void AddRange(BigJoin[] bigJoins);
}