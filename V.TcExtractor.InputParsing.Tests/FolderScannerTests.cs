using V.TcExtractor.InputParsing.Adapters.FileAdapters;
using V.TcExtractor.InputParsing.Adapters.TableAdapters;
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
            var testCases = sut.GetTestCases(TestDataBasePath);

            // Assert
            Dump(testCases);

            Assert.Equal(4200, testCases.Count());
        }

        private static FolderScanner GetSut()
        {
            var wordDocumentProcessor = GetWordFileProcessor();
            var sut = new FolderScanner([wordDocumentProcessor]);
            return sut;
        }
    }
}