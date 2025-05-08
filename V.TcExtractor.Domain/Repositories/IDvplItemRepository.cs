using V.TcExtractor.Domain.Model;

namespace V.TcExtractor.Domain.Repositories;

public interface IDvplItemRepository
{
    void DeleteAll();
    void AddRange(DvplItem[] dvplItems);
    DvplItem[] GetAll();
}