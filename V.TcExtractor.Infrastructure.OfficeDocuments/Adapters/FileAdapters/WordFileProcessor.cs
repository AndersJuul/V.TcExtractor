using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using V.TcExtractor.Domain.Model;
using V.TcExtractor.Infrastructure.OfficeDocuments.Adapters.TableAdapters;

namespace V.TcExtractor.Infrastructure.OfficeDocuments.Adapters.FileAdapters
{
    public class WordFileProcessor : ITestCaseFileProcessor
    {
        private readonly IEnumerable<ITableAdapter> _tableAdapters;

        public WordFileProcessor(IEnumerable<ITableAdapter> tableAdapters)
        {
            _tableAdapters = tableAdapters;
        }

        public bool CanHandle(string fileName)
        {
            var extension = Path.GetExtension(fileName);
            return extension.Equals(".docx", StringComparison.InvariantCultureIgnoreCase);
        }

        public List<TestCase> GetTestCases(string fileName)
        {
            var testCases = new List<TestCase>();

            using (var wordDocument = WordprocessingDocument.Open(fileName, false))
            {
                var body = wordDocument.MainDocumentPart?.Document.Body;

                // Find all tables in the document
                var tables = body?.Descendants<Table>().ToList() ?? [];

                foreach (var table in tables)
                {
                    var tableAdapter = _tableAdapters.SingleOrDefault(x => x.CanHandle(table));

                    var cases = tableAdapter?.GetTestCases(table, fileName);

                    // Add the test case if we found at least some information
                    if (cases != null)
                    {
                        testCases.AddRange(cases);
                    }
                }
            }

            return testCases;
        }
    }
}