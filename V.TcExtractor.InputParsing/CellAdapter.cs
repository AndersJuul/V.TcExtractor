using DocumentFormat.OpenXml.Wordprocessing;

public class CellAdapter : ICellAdapter
{
    public string GetCellText(TableCell cell)
    {
        if (cell == null)
            return string.Empty;
        // or use cell.InnerText?
        return string.Join(" ", cell.Descendants<Text>().Select(t => t.Text));
    }
}