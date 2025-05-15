using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using V.TcExtractor.Domain.Model;
using V.TcExtractor.Domain.Processors;
using V.TcExtractor.Infrastructure.OfficeDocuments.Adapters.TestResultTableAdapters;

namespace V.TcExtractor.Infrastructure.OfficeDocuments.Adapters.FileAdapters
{
    public class TestResultNonScadaFileProcessor : ITestResultFileProcessor
    {
        private readonly IEnumerable<ITestResultTableAdapter> _tableAdapters;
        private readonly IDmsNumberAdapter _dmsNumberAdapter;

        public TestResultNonScadaFileProcessor(IEnumerable<ITestResultTableAdapter> tableAdapters,
            IDmsNumberAdapter dmsNumberAdapter)
        {
            if (tableAdapters == null || !tableAdapters.Any())
                throw new ArgumentNullException(nameof(tableAdapters), "No table adapters provided.");
            _tableAdapters = tableAdapters;
            _dmsNumberAdapter = dmsNumberAdapter;
        }

        public bool CanHandle(string fileName)
        {
            if (!fileName.Contains("DVRE")) return false; // Take only DVRE
            if (fileName.Contains("SCADA")) return false; // SCADA is special

            var extension = Path.GetExtension(fileName);
            return extension.Equals(".docx", StringComparison.InvariantCultureIgnoreCase);
        }

        public List<TestResult> GetTestResults(string fileName)
        {
            var testResults = new List<TestResult>();

            using (var wordDocument = WordprocessingDocument.Open(fileName, false))
            {
                var dmsNumber = _dmsNumberAdapter.GetDmsNumberFromHeader(wordDocument);
                var body = wordDocument.MainDocumentPart?.Document.Body;

                // Find all tables in the document
                var tables = body?.Descendants<Table>().ToList() ?? [];

                foreach (var table in tables)
                {
                    var tableAdapter = _tableAdapters.SingleOrDefault(x => x.CanHandle(table));

                    var results = tableAdapter?.GetTestResults(table, fileName, dmsNumber);

                    // Add the test case if we found at least some information
                    if (results != null)
                    {
                        testResults.AddRange(results);
                    }
                }
            }

            return testResults;
        }
    }
}