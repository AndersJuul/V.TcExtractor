using V.TcExtractor.Domain.Model;
using V.TcExtractor.Domain.Repositories;

namespace V.TcExtractor.Infrastructure.CsvStorage.Repositories
{
    public class DvplRepositoryCsv : RepositoryCsv, IDvplRepository
    {
        protected override string GetFileName()
        {
            throw new NotImplementedException();
        }

        public void AddRange(DvplItem[] dvplItems)
        {
            throw new NotImplementedException();
        }
    }
}