using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using V.TcExtractor.InputParsing.Adapters.TableAdapters;
using V.TcExtractor.Model;

namespace V.TcExtractor.InputParsing.Adapters.FileAdapters
{
    public class WordFileProcessor : IFileProcessor
    {
        private readonly IEnumerable<ITableAdapter> _tableAdapters;
        private readonly ICellAdapter _cellAdapter;

        public WordFileProcessor(IEnumerable<ITableAdapter> tableAdapters, ICellAdapter cellAdapter)
        {
            _tableAdapters = tableAdapters;
            _cellAdapter = cellAdapter;
        }

        public bool CanHandle(string fileName)
        {
            var extension = Path.GetExtension(fileName);
            return extension.Equals(".docx", StringComparison.InvariantCultureIgnoreCase);
        }

        public List<TestCase> Handle(string filePath)
        {
            var testCases = new List<TestCase>();

            using (var wordDocument = WordprocessingDocument.Open(filePath, false))
            {
                var body = wordDocument.MainDocumentPart?.Document.Body;

                // Find all tables in the document
                var tables = body?.Descendants<Table>().ToList() ?? [];

                foreach (var table in tables)
                {
                    var tableAdapter = _tableAdapters.SingleOrDefault(x => x.CanHandle(table));

                    var cases = tableAdapter?.GetTestCases(table, filePath);

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