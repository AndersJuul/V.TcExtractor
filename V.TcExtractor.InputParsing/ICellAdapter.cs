using DocumentFormat.OpenXml.Wordprocessing;

public interface ICellAdapter
{
    string GetCellText(TableCell? cell);
}