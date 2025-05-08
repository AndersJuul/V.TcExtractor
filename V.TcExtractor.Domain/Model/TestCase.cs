using System.Text.RegularExpressions;

namespace V.TcExtractor.Domain.Model;

public class TestCase
{
    private string _dmsNumber = "";
    private string _testNo = "";
    public required string FileName { get; set; }

    public required string DmsNumber
    {
        get => _dmsNumber;
        set
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("DmsNumber can't be empty.");
            if (!Regex.IsMatch(value, @"^\d{4}-\d{4}$"))
                throw new ArgumentException("DmsNumber must be on form ####-####.");
            _dmsNumber = value;
        }
    }

    public string TestNo
    {
        get => _testNo;
        set
        {
            if (value.Contains(" "))
                throw new ArgumentException("TestNo can't contain spaces.");
            _testNo = value;
        }
    }

    public string Description { get; set; } = "";
    public string ReqId { get; set; } = "";

    public override string ToString()
    {
        return $"Test No: {TestNo}, Req ID: {ReqId}, FileName: {FileName}, Description: {Description}";
    }
}