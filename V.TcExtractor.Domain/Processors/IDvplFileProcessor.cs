using V.TcExtractor.Domain.Model;

namespace V.TcExtractor.Domain.Processors;

public interface IDvplFileProcessor
{
    bool CanHandle(string fileName);
    IEnumerable<DvplItem> GetDvplItems(string fileName);
}