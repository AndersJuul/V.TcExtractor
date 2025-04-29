using V.TcExtractor.Model;

namespace V.TcExtractor.InputParsing.Adapters.FileAdapters;

public class ExcelFileProcessor : IModuleRequirementFileProcessor
{
    public bool CanHandle(string file)
    {
        throw new NotImplementedException();
    }

    public IEnumerable<ModuleRequirement> GetModuleRequirements(string file)
    {
        throw new NotImplementedException();
    }
}