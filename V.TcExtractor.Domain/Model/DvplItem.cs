namespace V.TcExtractor.Domain.Model;

public class DvplItem
{
    public required string FileName { get; set; }
    public required string ModuleRsCode { get; set; }
    public required string TestLocation { get; set; }

    public override string ToString()
    {
        return $"{FileName} || {TestLocation} || {ModuleRsCode}";
    }
}