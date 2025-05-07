using V.TcExtractor.Domain.Model;

namespace V.TcExtractor.Infrastructure.OfficeDocuments.Adapters.FileAdapters;

public interface IDvplFileProcessor
{
    bool CanHandle(string fileName);
    IEnumerable<DvplItem> GetDvplItems(string fileName);
}