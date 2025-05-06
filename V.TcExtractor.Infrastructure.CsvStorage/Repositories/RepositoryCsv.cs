namespace V.TcExtractor.Infrastructure.CsvStorage.Repositories;

public abstract class RepositoryCsv
{
    protected abstract string GetFileName();

    public void DeleteAll()
    {
        var fileName = GetFileName();
        if (!File.Exists(fileName))
            return;

        File.Delete(fileName);
    }
}