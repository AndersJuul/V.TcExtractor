using DocumentFormat.OpenXml.Packaging;
using V.TcExtractor.Domain.Model;

namespace V.TcExtractor.Infrastructure.OfficeDocuments.Adapters.FileAdapters;

public interface IDmsNumberAdapter
{
    DmsInfo? GetDmsNumberFromHeader(WordprocessingDocument wordDocument);
}