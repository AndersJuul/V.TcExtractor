namespace V.TcExtractor.Domain.Options;

public class SettingOptions
{
    public bool RefreshTestCases { get; set; } = false; // Default 
    public bool RefreshModuleReq { get; set; } = false; // Default 
    public bool RefreshModuleReqTestCaseMapping { get; set; } = false; // Default 
    public bool RefreshDVPL { get; set; } = false; // Default
    public bool RefreshBigJoin { get; set; } = false; // Default
    public bool RefreshTestResults { get; set; } = false; // default
}