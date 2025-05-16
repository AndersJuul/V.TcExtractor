using V.TcExtractor.Infrastructure.OfficeDocuments.Adapters.FileAdapters;
using V.TcExtractor.Infrastructure.OfficeDocuments.Tests.Base;
using Xunit.Abstractions;

namespace V.TcExtractor.Infrastructure.OfficeDocuments.Tests
{
    public class WordFileProcessorTests : TestCaseTests
    {
        private readonly WordFileProcessor _processor;
        private string _validExistingFile;

        public WordFileProcessorTests(ITestOutputHelper testOutputHelper) : base(testOutputHelper)
        {
            _processor = new WordFileProcessor(new DmsNumberAdapter());
            _validExistingFile = Path.Combine(TestDataPath, "DVRE", "DVRE VES - Framework.docx");
        }

        [Fact]
        public void CanHandle_ReturnsTrueForWordDocument()
        {
            // Arrange

            // Act 
            var canHandle = _processor.CanHandle(_validExistingFile);

            // Assert
            Assert.True(canHandle);
        }

        [Fact]
        public void GetFileItem_ReturnsCorrectFileForValidExistingFile()
        {
            // Arrange

            // Act 
            var fileItem = _processor.GetFileItem(_validExistingFile);

            // Assert
            Assert.Equal("DVRE VES - Framework.docx", fileItem.FileName);
            Assert.Equal("0164-6839", fileItem.DmsNumber);
        }
    }
}