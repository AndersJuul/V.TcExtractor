using CsvHelper.Configuration;
using V.TcExtractor.Domain.Model;

namespace V.TcExtractor.Infrastructure.CsvStorage.Mappers;

public class FileItemMap : ClassMap<FileItem>
{
    public FileItemMap()
    {
        Map(m => m.DmsNumber).Name("DmsNumber").Index(0);
        Map(m => m.FileType).Name("FileType").Index(1);
        Map(m => m.FileName).Name("FileName").Index(2);
        Map(m => m.FilePath).Name("FilePath").Index(3);
    }
}