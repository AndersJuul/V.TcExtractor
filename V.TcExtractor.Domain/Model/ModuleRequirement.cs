namespace V.TcExtractor.Domain.Model;

public class ModuleRequirement
{
    private string _id = "";
    private string _productRequirement;

    public string Id
    {
        get => _id;
        set
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("Id can't be empty.");

            _id = value.Replace(",", ".");
        }
    }

    public string RsTitle { get; set; } = "";
    public string CombinedRequirement { get; set; } = "";
    public string Motivation { get; set; } = "";
    public string FileName { get; set; } = "";
    public RequirementSource Source { get; set; }

    public string ProductRequirement
    {
        get => _productRequirement;
        set
        {
            if (!value.Contains("PRO"))
                throw new ArgumentException("Product requirement must contain 'PRO'.");
            _productRequirement = value;
        }
    }

    public override string ToString()
    {
        return $"{Id}  {ProductRequirement}  {RsTitle}   {CombinedRequirement}     {Motivation}   {FileName} {Source}";
    }
}