using V.TcExtractor.InputParsing.Tests.Base;
using Xunit.Abstractions;

namespace V.TcExtractor.InputParsing.Tests
{
    public class FolderScannerTests : TestCaseTests
    {
        public FolderScannerTests(ITestOutputHelper testOutputHelper) : base(testOutputHelper)
        {
        }

        [Fact]
        public void Scan_finds_all_test_cases()
        {
            // This test assumes test data in known place.
            // Arrange
            var sut = GetSut();

            // Act
            var testCases = sut
                .GetTestCases(TestDataBasePath)
                .ToArray();

            // Assert
            Dump(testCases);

            Assert.Equal(227, testCases.Count());
        }

        private static FolderScanner GetSut()
        {
            var wordDocumentProcessor = GetWordFileProcessor();
            var sut = new FolderScanner([wordDocumentProcessor]);
            return sut;
        }
    }
}