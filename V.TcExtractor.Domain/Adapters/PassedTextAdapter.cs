namespace V.TcExtractor.Domain.Adapters;

public class PassedTextAdapter : IPassedTextAdapter
{
    private readonly string[] _wordPointingToFailure =
    [
        "failed",
        "not pass",
        "not passed",
        "Partial passed",
        "Partial  p assed"
    ];

    private readonly string[] _wordPointingToSuccess =
    [
        "pass",
        "success",
        "s uccess",
        "Partial passed",
        "Partial  p assed"
    ];

    public string GetPassedFromTestResult(string result)
    {
        if (_wordPointingToFailure.Any(x => result.Contains(x, StringComparison.InvariantCultureIgnoreCase)))
            return "Not Passed";
        if (_wordPointingToSuccess.Any(x => result.Contains(x, StringComparison.InvariantCultureIgnoreCase)))
            return "Passed";

        // Default to not passed if no match found
        return "Not Passed";
    }
}