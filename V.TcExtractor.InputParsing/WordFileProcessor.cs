using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using V.TcExtractor.InputParsing.Model;

namespace V.TcExtractor.InputParsing
{
    public class WordFileProcessor : IFileProcessor
    {
        public bool CanHandle(string fileName)
        {
            var extension = Path.GetExtension(fileName);
            return extension.Equals(".docx", StringComparison.InvariantCultureIgnoreCase);
        }

        public List<TestCase> Handle(string filePath)
        {
            List<TestCase> testCases = new List<TestCase>();

            try
            {
                using (WordprocessingDocument wordDocument = WordprocessingDocument.Open(filePath, false))
                {
                    Body body = wordDocument.MainDocumentPart.Document.Body;

                    // Find all tables in the document
                    var tables = body.Descendants<Table>().ToList();

                    foreach (var table in tables)
                    {
                        // Check if this is a test case table by looking for "Test Case Information" text
                        bool isTestCaseTable = false;
                        var firstRow = table.Elements<TableRow>().FirstOrDefault();

                        if (firstRow != null)
                        {
                            var cellText = GetCellText(firstRow.Elements<TableCell>().FirstOrDefault());
                            if (cellText.Contains("Test Case Information"))
                            {
                                isTestCaseTable = true;
                            }
                        }

                        if (isTestCaseTable)
                        {
                            TestCase testCase = new TestCase { FileName = filePath };

                            // Process rows to find Test No, Description, and Req ID
                            foreach (var row in table.Elements<TableRow>())
                            {
                                var cells = row.Elements<TableCell>().ToList();
                                if (cells.Count >= 2)
                                {
                                    string headerCell = GetCellText(cells[0]);
                                    string valueCell = GetCellText(cells[1]);

                                    // Extract Test No
                                    if (headerCell.Contains("Test No"))
                                    {
                                        testCase.TestNo = valueCell.Trim();
                                    }
                                    // Extract Description
                                    else if (headerCell.Contains("Description"))
                                    {
                                        testCase.Description = valueCell.Trim();
                                    }
                                    // Extract Req ID
                                    else if (cells.Count >= 5 && headerCell.Contains("Test Case Information"))
                                    {
                                        // Req ID is typically in the 5th column of the header row
                                        if (cells.Count >= 5)
                                        {
                                            testCase.ReqId = GetCellText(cells[4]).Trim();
                                        }
                                    }
                                }
                            }

                            // Add the test case if we found at least some information
                            if (!string.IsNullOrWhiteSpace(testCase.TestNo) ||
                                !string.IsNullOrWhiteSpace(testCase.Description))
                            {
                                testCases.Add(testCase);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error processing document: {ex.Message}");
            }

            return testCases;
        }

        private string GetCellText(TableCell cell)
        {
            if (cell == null)
                return string.Empty;

            return string.Join(" ", cell.Descendants<Text>().Select(t => t.Text));
        }
    }
}