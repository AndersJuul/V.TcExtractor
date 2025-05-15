using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using V.TcExtractor.Domain.Model;
using V.TcExtractor.Domain.Processors;
using V.TcExtractor.Infrastructure.OfficeDocuments.Adapters.TestResultTableAdapters;

namespace V.TcExtractor.Infrastructure.OfficeDocuments.Adapters.FileAdapters
{
    public class TestResultFileProcessor : ITestResultFileProcessor
    {
        private readonly IEnumerable<ITestResultTableAdapter> _tableAdapters;

        public TestResultFileProcessor(IEnumerable<ITestResultTableAdapter> tableAdapters)
        {
            if (tableAdapters == null || !tableAdapters.Any())
                throw new ArgumentNullException(nameof(tableAdapters), "No table adapters provided.");
            _tableAdapters = tableAdapters;
        }

        public bool CanHandle(string fileName)
        {
            if (!fileName.Contains("DVPR")) return false; // Take only DVRE
            if (fileName.Contains("SCADA")) return false; // SCADA is special
            var extension = Path.GetExtension(fileName);
            return extension.Equals(".docx", StringComparison.InvariantCultureIgnoreCase);
        }

        public List<TestResult> GetTestResults(string fileName)
        {
            var testResults = new List<TestResult>();

            using (var wordDocument = WordprocessingDocument.Open(fileName, false))
            {
                var dmsNumber = GetDmsNumberFromHeader(wordDocument);
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

        private static string GetDmsNumberFromHeader(WordprocessingDocument wordDocument)
        {
            var headerParts = wordDocument.MainDocumentPart?.HeaderParts.ToArray() ??
                              throw new NullReferenceException(
                                  "Not able to get MainDocumentPart?.HeaderParts from document.");

            foreach (var headerPart in headerParts)
            {
                var text = headerPart.Header.InnerText;
                var dmsNumberFromHeader = DmsNumberFromHeader(text, "Document: INFO  Title ");
                if (dmsNumberFromHeader != "") return dmsNumberFromHeader;
            }

            foreach (var headerPart in headerParts)
            {
                var text = headerPart.Header.InnerText;
                var dmsNumberFromHeader = DmsNumberFromHeader(text, "Document no. ");
                if (dmsNumberFromHeader != "") return dmsNumberFromHeader;
            }

            foreach (var headerPart in headerParts)
            {
                var text = headerPart.Header.InnerText;
                var dmsNumberFromHeader = DmsNumberFromHeader(text, "Document:");
                if (dmsNumberFromHeader != "") return dmsNumberFromHeader;
            }

            foreach (var headerPart in headerParts)
            {
                var text = headerPart.Header.InnerText;
                var dmsNumberFromHeader = DmsNumberFromHeader(text, "DMS no.: ");
                if (dmsNumberFromHeader != "") return dmsNumberFromHeader;
            }

            return "";
        }

        private static string DmsNumberFromHeader(string text, string indicator)
        {
            if (text.Contains(indicator))
            {
                var subPart = text.Substring(text.IndexOf(indicator) + indicator.Length).Substring(0, 9);
                return subPart;
            }

            return "";
        }
    }
}