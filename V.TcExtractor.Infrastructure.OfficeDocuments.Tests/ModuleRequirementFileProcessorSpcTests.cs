using V.TcExtractor.Infrastructure.OfficeDocuments.Adapters.FileAdapters;
using V.TcExtractor.Infrastructure.OfficeDocuments.Tests.Base;
using Xunit.Abstractions;

namespace V.TcExtractor.Infrastructure.OfficeDocuments.Tests
{
    public class ModuleRequirementFileProcessorSpcTests(ITestOutputHelper testOutputHelper)
        : TestCaseTests(testOutputHelper)
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
            var canHandle = sut.CanHandle(Path.Combine(TestDataPath, "DVPL", "SPC Requirements.xlsx"));

            // Assert
            Assert.True(canHandle);
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
            Assert.Equal(81, moduleRequirements.Count());
        }

        private static ModuleRequirementFileProcessorSpc GetSut()
        {
            return new ModuleRequirementFileProcessorSpc();
        }
    }
}