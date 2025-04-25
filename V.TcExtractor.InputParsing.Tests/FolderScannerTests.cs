using Xunit.Abstractions;

namespace V.TcExtractor.InputParsing.Tests
{
    public class FolderScannerTests
    {
        private readonly ITestOutputHelper _testOutputHelper;

        public FolderScannerTests(ITestOutputHelper testOutputHelper)
        {
            _testOutputHelper = testOutputHelper;
        }

        [Fact]
        public void Guard_rejects_empty_arg_for_path()
        {
            // Arrange
            var sut = new FolderScanner([]);

            // Act
            // Assert
            Assert.Throws<ArgumentException>(() => sut.Scan([]));
        }

        [Fact]
        public void Guard_rejects_two_args_for_path()
        {
            // Arrange
            var sut = new FolderScanner([]);

            // Act
            // Assert
            Assert.Throws<ArgumentException>(() => sut.Scan([]));
        }

        [Fact]
        public void Guard_accepts_single_arg_for_path()
        {
            // This test assumes test data in known place.
            // Arrange
            var sut = new FolderScanner([]);

            // Act
            var testCases = sut.Scan(["C:\\DATA\\DVPR"]);

            // Assert
            Assert.Empty(testCases);
        }

        [Fact]
        public void Scan_finds_all_test_cases()
        {
            // This test assumes test data in known place.
            // Arrange
            var wordDocumentProcessor = new WordFileProcessor();
            var sut = new FolderScanner([wordDocumentProcessor]);

            // Act
            var testCases = sut.Scan(["C:\\DATA\\DVPR"]);

            // Assert
            foreach (var testCase in testCases)
            {
                _testOutputHelper.WriteLine($"Test : {testCase}");
            }

            Assert.Equal(4200, testCases.Count());
        }
    }
}