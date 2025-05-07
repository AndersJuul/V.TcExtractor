using V.TcExtractor.Domain.Model;

namespace V.TcExtractor.Domain.Repositories;

public interface IDvplRepository
{
    void DeleteAll();
    void AddRange(DvplItem[] dvplItems);
}