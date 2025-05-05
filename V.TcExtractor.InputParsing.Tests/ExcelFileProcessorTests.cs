using Xunit.Abstractions;
using V.TcExtractor.InputParsing.Adapters.FileAdapters;
using V.TcExtractor.InputParsing.Tests.Base;

namespace V.TcExtractor.InputParsing.Tests
{
    public class ExcelFileProcessorTests(ITestOutputHelper testOutputHelper) : TestCaseTests(testOutputHelper)
    {
        [Fact]
        public void CanHandle_returns_false_for_word_file_name()
        {
            // Arrange
            var sut = GetSut();

            // Act
            var canHandle = sut.CanHandle("A.docx");

            // Assert
            Assert.False(canHandle);
        }

        [Fact]
        public void CanHandle_returns_true_for_excel_file_name()
        {
            // Arrange
            var sut = GetSut();

            // Act
            var canHandle = sut.CanHandle(Path.Combine(TestDataPath, "DVPL", "PSI Requirements.xlsx"));

            // Assert
            Assert.True(canHandle);
        }

        [Fact]
        public void GetModuleRequirements_returns_module_requirements_for_psi_requirements()
        {
            // Arrange
            var sut = GetSut();

            // Act
            var moduleRequirements = sut
                .GetModuleRequirements(Path.Combine(TestDataPath, "DVPL", "PSI Requirements.xlsx"));

            // Assert
            Dump(moduleRequirements);
            Assert.Equal(43, moduleRequirements.Count());
        }

        [Fact]
        public void GetModuleRequirements_returns_module_requirements_for_spc_requirements()
        {
            // Arrange
            var sut = GetSut();

            // Act
            var moduleRequirements = sut
                .GetModuleRequirements(Path.Combine(TestDataPath, "DVPL", "SPC Requirements.xlsx"));

            // Assert
            Dump(moduleRequirements);
            Assert.Equal(85, moduleRequirements.Count());
        }

        private static ExcelFileProcessor GetSut()
        {
            var sut = GetExcelFileProcessor();
            return sut;
        }
    }
}