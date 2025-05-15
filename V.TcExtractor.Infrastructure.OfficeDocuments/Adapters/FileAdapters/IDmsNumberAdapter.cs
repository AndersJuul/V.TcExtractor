using DocumentFormat.OpenXml.Packaging;

namespace V.TcExtractor.Infrastructure.OfficeDocuments.Adapters.FileAdapters;

public interface IDmsNumberAdapter
{
    string GetDmsNumberFromHeader(WordprocessingDocument wordDocument);
}