using DocumentFormat.OpenXml.Packaging;
using V.TcExtractor.Domain.Model;

namespace V.TcExtractor.Infrastructure.OfficeDocuments.Adapters.FileAdapters;

public class DmsNumberAdapter : IDmsNumberAdapter
{
    public DmsInfo? GetDmsNumberFromHeader(WordprocessingDocument wordDocument)
    {
        var headerParts = wordDocument.MainDocumentPart?.HeaderParts.ToArray() ??
                          throw new NullReferenceException(
                              "Not able to get MainDocumentPart?.HeaderParts from document.");

        foreach (var headerPart in headerParts)
        {
            var text = headerPart.Header.InnerText;
            var dmsNumberFromHeader = DmsNumberFromHeader(text, "Document: INFO  Title ");
            if (dmsNumberFromHeader != null) return dmsNumberFromHeader;
        }

        foreach (var headerPart in headerParts)
        {
            var text = headerPart.Header.InnerText;
            var dmsNumberFromHeader = DmsNumberFromHeader(text, "Document no. ");
            if (dmsNumberFromHeader != null) return dmsNumberFromHeader;
        }

        foreach (var headerPart in headerParts)
        {
            var text = headerPart.Header.InnerText;
            var dmsNumberFromHeader = DmsNumberFromHeader(text, "Document:");
            if (dmsNumberFromHeader != null) return dmsNumberFromHeader;
        }

        foreach (var headerPart in headerParts)
        {
            var text = headerPart.Header.InnerText;
            var dmsNumberFromHeader = DmsNumberFromHeader(text, "DMS no.: ");
            if (dmsNumberFromHeader != null) return dmsNumberFromHeader;
        }

        return null;
    }

    private static DmsInfo? DmsNumberFromHeader(string text, string indicator)
    {
        if (text.Contains(indicator))
        {
            var subPart = text.Substring(text.IndexOf(indicator) + indicator.Length).Substring(0, 9);
            return new DmsInfo { Number = subPart };
        }

        return null;
    }
}