using V.TcExtractor.Domain.Model;
using V.TcExtractor.Domain.Processors;

namespace V.TcExtractor.Infrastructure.OfficeDocuments.Adapters.FileAdapters;

public class ExcelFileProcessor : IFileProcessor
{
    public bool CanHandle(string file)
    {
        var extension = Path.GetExtension(file);
        return extension.Equals(".xlsx", StringComparison.InvariantCultureIgnoreCase);
    }

    public FileItem GetFileItem(string file)
    {
        return new FileItem
        {
            FileName = Path.GetFileName(file),
            DmsNumber = "?",
            FileType = "Excel",
            FilePath = Path.GetFullPath(file)
        };
    }
}