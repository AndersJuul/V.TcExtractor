using V.TcExtractor.InputParsing.Adapters.FileAdapters;
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
                .GetTestCases()
                .ToArray();

            // Assert
            Dump(testCases);

            Assert.Equal(227, testCases.Count());
        }

        [Fact]
        public void Scan_finds_all_module_requirements()
        {
            // This test assumes test data in known place.
            // Arrange
            var sut = GetSut();

            // Act
            var moduleRequirements = sut
                .GetModuleRequirements()
                .ToArray();

            // Assert
            Dump(moduleRequirements);

            Assert.Equal(43, moduleRequirements.Length);
        }

        private FolderScanner GetSut()
        {
            var wordDocumentProcessor = GetWordFileProcessor();
            var moduleRequirementFileProcessor = new ExcelFileProcessor();

            var sut = new FolderScanner([wordDocumentProcessor], [moduleRequirementFileProcessor],
                new InputFolder(TestDataPath));
            return sut;
        }
    }
}