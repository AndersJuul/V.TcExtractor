using Xunit.Abstractions;
using V.TcExtractor.InputParsing.Adapters.FileAdapters;
using V.TcExtractor.InputParsing.Adapters.TableAdapters;
using V.TcExtractor.InputParsing.Tests.Base;

namespace V.TcExtractor.InputParsing.Tests
{
    public class WordFileProcessorTests(ITestOutputHelper testOutputHelper) : TestCaseTests(testOutputHelper)
    {
        [Fact]
        public void CanHandle_returns_true_for_word_file_name()
        {
            // Arrange
            var sut = GetSut();

            // Act
            var canHandle = sut.CanHandle("A.docx");

            // Assert
            Assert.True(canHandle);
        }

        [Fact]
        public void CanHandle_returns_false_for_excel_file_name()
        {
            // Arrange
            var sut = GetSut();

            // Act
            var canHandle = sut.CanHandle("A.xlsx");

            // Assert
            Assert.False(canHandle);
        }

        [Fact]
        public void Handle_returns_testcases_for_spc_ves_dvpr()
        {
            // Arrange
            var sut = GetSut();

            // Act
            var testCases = sut
                .Handle(Path.Combine(TestDataBasePath, "SPC", "VES DVPR.docx"));

            // Assert
            Dump(testCases);
            Assert.Equal(38, testCases.Count);
        }

        [Fact]
        public void Handle_returns_testcases_for_psi_dvpr_ves_multithreading()
        {
            // Arrange
            var sut = GetSut();

            // Act
            var testCases = sut
                .Handle(Path.Combine(TestDataBasePath, "PSI", "DVPR VES  Multithreading.docx"))
                .ToArray();

            // Assert
            Dump(testCases);
            Assert.Equal(2, testCases.Count());
            // Original input file modified to have ReqId == REQ-1 for testing purposes (was blank)
            // Assert.Equal("REQ-1", testCases[0].ReqId);
            Assert.All(testCases, x => Assert.False(string.IsNullOrEmpty(x.TestNo)));
            Assert.All(testCases, x => Assert.False(string.IsNullOrEmpty(x.FileName)));
            Assert.All(testCases, x => Assert.False(string.IsNullOrEmpty(x.Description)));
        }

        [Fact]
        public void Handle_returns_testcases_for_psi_dvpr_ves_bess_interface()
        {
            // Arrange
            var sut = GetSut();

            // Act
            var testCases = sut
                .Handle(Path.Combine(TestDataBasePath, "PSI", "DVPR VES BESS Interface.docx"))
                .ToArray();

            // Assert
            Dump(testCases);
            Assert.Equal(2, testCases.Count());
            // Original input file modified to have ReqId == REQ-1 for testing purposes (was blank)
            //Assert.Equal("REQ-1", testCases[0].ReqId);

            Assert.All(testCases, x => Assert.False(string.IsNullOrEmpty(x.TestNo)));
            Assert.All(testCases, x => Assert.False(string.IsNullOrEmpty(x.FileName)));
            Assert.All(testCases, x => Assert.False(string.IsNullOrEmpty(x.Description)));
        }

        private static WordFileProcessor GetSut()
        {
            var cellAdapter = new CellAdapter();
            var sut = new WordFileProcessor(
                [
                    new TableAdapterId(cellAdapter),
                    new TableAdapterTestCaseInformationHeadersInColZero(cellAdapter),
                    new TableAdapterTestCaseInformationHeadersInRowZero(cellAdapter)
                ],
                cellAdapter);
            return sut;
        }
    }
}