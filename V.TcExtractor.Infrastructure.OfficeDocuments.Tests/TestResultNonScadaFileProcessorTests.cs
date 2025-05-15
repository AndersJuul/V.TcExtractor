using V.TcExtractor.Domain.Model;
using V.TcExtractor.Infrastructure.OfficeDocuments.Adapters.FileAdapters;
using V.TcExtractor.Infrastructure.OfficeDocuments.Tests.Base;
using Xunit.Abstractions;

namespace V.TcExtractor.Infrastructure.OfficeDocuments.Tests
{
    public class TestResultNonScadaFileProcessorTests(ITestOutputHelper testOutputHelper)
        : TestCaseTests(testOutputHelper)
    {
        [Fact]
        public void CanHandle_returns_true_for_word_file_name()
        {
            // Arrange
            var sut = GetSut();

            // Act
            var canHandle = sut.CanHandle("A DVRE.docx");

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
            //yield return [new[] { "DVRE", "DVRE VES  Multithreading.docx" }, 38];
            yield return [new[] { "DVRE", "DVRE VES - Framework.docx" }, 14];
            yield return [new[] { "DVRE", "DVRE VES - IO.docx" }, 5];
            yield return [new[] { "DVRE", "DVRE VES - L7FW Failover with Containers.docx" }, 11];
            yield return [new[] { "DVRE", "DVRE VES - OSV 1.docx" }, 76];
            yield return [new[] { "DVRE", "DVRE VES - VOT.docx" }, 37];
            yield return [new[] { "DVRE", "Tests_Batch - VES DVRE for Base Configuration.docx" }, 12];
        }

        [Theory]
        [MemberData(nameof(TestData))]
        public void Handle_returns_testresults_for_file(string[] a, int expectedCount)
        {
            // Arrange
            var path = new string[] { TestDataPath }.Concat(a).ToArray();
            var sut = GetSut();

            // Act
            var testResults = sut
                .GetTestResults(Path.Combine(path))
                .ToArray();

            // Assert
            Dump(testResults);
            Assert.Equal(expectedCount, testResults.Length);
        }

        private TestResultNonScadaFileProcessor GetSut()
        {
            var sut = GetTestResultNonScadaFileProcessor();
            return sut;
        }
    }
}