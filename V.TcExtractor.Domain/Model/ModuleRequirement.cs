namespace V.TcExtractor.Domain.Model;

public class ModuleRequirement
{
    private string _id = "";

    public string Id
    {
        get => _id;
        set => _id = value.Replace(",", ".");
    }

    public string RsTitle { get; set; } = "";
    public string CombinedRequirement { get; set; } = "";
    public string Motivation { get; set; } = "";
    public string FileName { get; set; } = "";
    public RequirementSource Source { get; set; }

    public override string ToString()
    {
        return $"{Id}    {RsTitle}   {CombinedRequirement}     {Motivation}   {FileName} {Source}";
    }
}