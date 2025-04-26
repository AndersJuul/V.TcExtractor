namespace V.TcExtractor.InputParsing.Model;

public class TestCase
{
    public required string FileName { get; set; }
    public string TestNo { get; set; } = "";
    public string Description { get; set; } = "";
    public string ReqId { get; set; } = "";

    public override string ToString()
    {
        return $"Test No: {TestNo}, Req ID: {ReqId}, FileName: {FileName}, Description: {Description}";
    }
}