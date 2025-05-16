using V.TcExtractor.Domain.Model;

namespace V.TcExtractor.Domain.Repositories;

public interface IFileItemRepository
{
    void DeleteAll();
    void AddRange(FileItem[] fileItems);
}