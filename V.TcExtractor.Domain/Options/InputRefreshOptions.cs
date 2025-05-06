namespace V.TcExtractor.Domain.Options;

public class InputRefreshOptions
{
    public bool ShouldRefreshTestCases { get; set; } = false; // Default 
    public bool ShouldRefreshModuleReq { get; set; } = false; // Default 
}