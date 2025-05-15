namespace V.TcExtractor.Domain.Model;

public class TestResult
{
    public required string TestId { get; set; } = "";
    public required string Passed { get; set; } = "";
    public required string Subject { get; set; } = "";
    public required string Result { get; set; } = "";
    public required string FileName { get; set; } = "";

    public override string ToString()
    {
        return $"TestId: {TestId}  Passed: {Passed}  Subject: {Subject} Result: {Result} FileName: {FileName}";
    }
}