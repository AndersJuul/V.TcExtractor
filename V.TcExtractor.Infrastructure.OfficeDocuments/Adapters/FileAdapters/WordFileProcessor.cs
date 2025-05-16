using DocumentFormat.OpenXml.Packaging;
using V.TcExtractor.Domain.Model;
using V.TcExtractor.Domain.Processors;

namespace V.TcExtractor.Infrastructure.OfficeDocuments.Adapters.FileAdapters;

public class WordFileProcessor : IFileProcessor
{
    private readonly IDmsNumberAdapter _dmsNumberAdapter;

    public WordFileProcessor(IDmsNumberAdapter dmsNumberAdapter)
    {
        _dmsNumberAdapter = dmsNumberAdapter;
    }

    public bool CanHandle(string file)
    {
        var extension = Path.GetExtension(file);
        return extension.Equals(".docx", StringComparison.InvariantCultureIgnoreCase);
    }

    public FileItem GetFileItem(string file)
    {
        using (var wordDocument = WordprocessingDocument.Open(file, false))
        {
            var dmsNumber = _dmsNumberAdapter.GetDmsNumberFromHeader(wordDocument);

            return new FileItem
            {
                FileName = Path.GetFileName(file),
                DmsNumber = dmsNumber?.Number ?? "?",
                FileType = "Word",
                FilePath = Path.GetFullPath(file)
            };
        }
    }
}