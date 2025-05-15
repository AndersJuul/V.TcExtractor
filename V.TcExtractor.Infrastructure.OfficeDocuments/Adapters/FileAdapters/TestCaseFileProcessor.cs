using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using V.TcExtractor.Domain.Model;
using V.TcExtractor.Domain.Processors;
using V.TcExtractor.Infrastructure.OfficeDocuments.Adapters.TestCaseTableAdapters;

namespace V.TcExtractor.Infrastructure.OfficeDocuments.Adapters.FileAdapters
{
    public class TestCaseFileProcessor : ITestCaseFileProcessor
    {
        private readonly IEnumerable<ITestCaseTableAdapter> _tableAdapters;
        private readonly IDmsNumberAdapter _dmsNumberAdapter;

        public TestCaseFileProcessor(IEnumerable<ITestCaseTableAdapter> tableAdapters,
            IDmsNumberAdapter dmsNumberAdapter)
        {
            _tableAdapters = tableAdapters;
            _dmsNumberAdapter = dmsNumberAdapter;
        }

        public bool CanHandle(string fileName)
        {
            if (!fileName.Contains("DVPR")) return false; // Take only dvpr
            var extension = Path.GetExtension(fileName);
            return extension.Equals(".docx", StringComparison.InvariantCultureIgnoreCase);
        }

        public List<TestCase> GetTestCases(string fileName)
        {
            var testCases = new List<TestCase>();

            using (var wordDocument = WordprocessingDocument.Open(fileName, false))
            {
                var dmsNumber = _dmsNumberAdapter.GetDmsNumberFromHeader(wordDocument);
                var body = wordDocument.MainDocumentPart?.Document.Body;

                // Find all tables in the document
                var tables = body?.Descendants<Table>().ToList() ?? [];

                foreach (var table in tables)
                {
                    var tableAdapter = _tableAdapters.SingleOrDefault(x => x.CanHandle(table));

                    var cases = tableAdapter?.GetTestCases(table, fileName, dmsNumber);

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