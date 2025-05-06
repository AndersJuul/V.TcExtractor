using DocumentFormat.OpenXml.Wordprocessing;

namespace V.TcExtractor.Infrastructure.OfficeDocuments.Adapters.CellAdapters;

public interface ICellAdapter
{
    string GetCellText(TableCell? cell);
}