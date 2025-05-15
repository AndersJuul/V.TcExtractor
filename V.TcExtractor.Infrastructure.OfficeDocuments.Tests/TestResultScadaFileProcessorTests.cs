using V.TcExtractor.Infrastructure.OfficeDocuments.Adapters.FileAdapters;
using V.TcExtractor.Infrastructure.OfficeDocuments.Tests.Base;
using Xunit.Abstractions;

namespace V.TcExtractor.Infrastructure.OfficeDocuments.Tests
{
    public class TestResultScadaFileProcessorTests(ITestOutputHelper testOutputHelper)
        : TestCaseTests(testOutputHelper)
    {
        [Fact]
        public void CanHandle_returns_true_for_word_file_name()
        {
            // Arrange
            var sut = GetSut();

            // Act
            var canHandle = sut.CanHandle(Path.Combine("DVRE", "DVRE VES - SCADA.docx"));

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

        public static IEnumerable<object[]> TestData()
        {
            yield return [new[] { "DVRE", "DVRE VES - SCADA.docx" }, 38];
        }

        [Theory]
        [MemberData(nameof(TestData))]
        public void Handle_returns_testresults_for_file(string[] a, int expectedCount)
        {
            // Arrange
            var path = new[] { TestDataPath }.Concat(a).ToArray();
            var sut = GetSut();

            // Act
            var testResults = sut
                .GetTestResults(Path.Combine(path))
                .ToArray();

            // Assert
            Dump(testResults);
            Assert.Equal(expectedCount, testResults.Length);
        }

        private TestResultScadaFileProcessor GetSut()
        {
            var sut = GetTestResultScadaFileProcessor();
            return sut;
        }
    }
}