using Xunit.Abstractions;

namespace V.TcExtractor.InputParsing.Tests
{
    public class WordFileProcessorTests
    {
        private readonly ITestOutputHelper _testOutputHelper;

        public WordFileProcessorTests(ITestOutputHelper testOutputHelper)
        {
            _testOutputHelper = testOutputHelper;
        }

        [Fact]
        public void CanHandle_returns_true_for_word_file_name()
        {
            // Arrange
            var sut = new WordFileProcessor();

            // Act
            var canHandle = sut.CanHandle("A.docx");

            // Assert
            Assert.True(canHandle);
        }

        [Fact]
        public void CanHandle_returns_false_for_excel_file_name()
        {
            // Arrange
            var sut = new WordFileProcessor();

            // Act
            var canHandle = sut.CanHandle("A.xlsx");

            // Assert
            Assert.False(canHandle);
        }
    }
}