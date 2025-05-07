using V.TcExtractor.Infrastructure.OfficeDocuments;

namespace V.TcExtractor.Domain.Repositories;

public interface IDvplRepository
{
    void DeleteAll();
    void AddRange(DvplItem[] dvplItems);
}