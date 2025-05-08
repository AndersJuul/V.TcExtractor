using System.Diagnostics.CodeAnalysis;

namespace V.TcExtractor.Domain.Model;

public class DvplItem
{
    private string _fileName;
    private string _moduleRsCode;
    private string _testLocation;
    private string _productRsCode;

    public required string FileName
    {
        get => _fileName;
        set
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("FileName can't be empty.");
            _fileName = value;
        }
    }

    public required string ModuleRsCode
    {
        get => _moduleRsCode;
        set
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("ModuleRsCode can't be empty.");
            _moduleRsCode = value;
        }
    }

    public required string TestLocation
    {
        get => _testLocation;
        set
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("TestLocation can't be empty.");
            _testLocation = value;
        }
    }

    public required string ProductRsCode
    {
        get => _productRsCode;
        set
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("ProductRsCode can't be empty.");
            _productRsCode = value;
        }
    }

    public override string ToString()
    {
        return $"{ProductRsCode} || {TestLocation} || {ModuleRsCode} || {FileName} ";
    }
}