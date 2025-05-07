namespace V.TcExtractor.Domain.Model;

public class DvplItem
{
    public string FileName { get; set; }
    public string ModuleRsCode { get; set; }
    public string TestLocation { get; set; }

    public override string ToString()
    {
        return $"{FileName} {TestLocation} {ModuleRsCode}";
    }
}