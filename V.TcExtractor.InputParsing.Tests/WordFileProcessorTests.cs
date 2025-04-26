using Xunit.Abstractions;
using V.TcExtractor.InputParsing.Adapters.FileAdapters;
using V.TcExtractor.InputParsing.Adapters.TableAdapters;

namespace V.TcExtractor.InputParsing.Tests
{
    public class WordFileProcessorTests(ITestOutputHelper testOutputHelper) : TestCaseTests(testOutputHelper)
    {
        [Fact]
        public void CanHandle_returns_true_for_word_file_name()
        {
            // Arrange
            var sut = new WordFileProcessor([], new CellAdapter());

            // Act
            var canHandle = sut.CanHandle("A.docx");

            // Assert
            Assert.True(canHandle);
        }

        [Fact]
        public void CanHandle_returns_false_for_excel_file_name()
        {
            // Arrange
            var sut = new WordFileProcessor([], new CellAdapter());

            // Act
            var canHandle = sut.CanHandle("A.xlsx");

            // Assert
            Assert.False(canHandle);
        }

        [Fact]
        public void Handle_returns_testcases_for_ves_dvpr()
        {
            // Arrange
            var cellAdapter = new CellAdapter();
            var sut = new WordFileProcessor(
                [
                    new TableAdapterId(cellAdapter),
                    new TableAdapterTestCaseInformation(cellAdapter)
                ],
                cellAdapter);

            // Act
            var testCases = sut.Handle(Path.Combine(TestDataBasePath, "SPC", "VES DVPR.docx"));

            // Assert
            Dump(testCases);
            Assert.Equal(38, testCases.Count);
        }
    }
}