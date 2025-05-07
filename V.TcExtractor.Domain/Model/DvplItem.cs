namespace V.TcExtractor.Domain.Model;

public class DvplItem
{
    public string FileName { get; set; }

    public override string ToString()
    {
        return $"DVPL Item: {FileName}";
    }
}