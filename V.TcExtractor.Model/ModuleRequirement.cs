namespace V.TcExtractor.Model;

public class ModuleRequirement
{
    public string Id { get; set; } = "";
    public string RsTitle { get; set; } = "";
    public string CombinedRequirement { get; set; } = "";
    public string Motivation { get; set; } = "";
    public string FileName { get; set; } = "";

    public override string ToString()
    {
        return $"{Id}    {RsTitle}   {CombinedRequirement}     {Motivation}   {FileName}";
    }
}