namespace V.TcExtractor.Domain.Model;

public class FileItem
{
    public required string FileName { get; set; }
    public required string DmsNumber { get; set; }
    public required string FileType { get; set; }
    public required string FilePath { get; set; }
}