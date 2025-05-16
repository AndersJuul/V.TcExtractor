using V.TcExtractor.Domain.Model;

namespace V.TcExtractor.Domain.Processors;

public interface IFileProcessor
{
    bool CanHandle(string file);
    FileItem GetFileItem(string file);
}