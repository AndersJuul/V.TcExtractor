using Microsoft.Extensions.Options;
using V.TcExtractor.Domain.Options;
using V.TcExtractor.Infrastructure.OfficeDocuments.Adapters.FileAdapters;
using V.TcExtractor.Infrastructure.OfficeDocuments.Tests.Base;
using Xunit.Abstractions;

namespace V.TcExtractor.Infrastructure.OfficeDocuments.Tests
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

            Assert.Equal(225, testCases.Count());
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

            Assert.Equal(122, moduleRequirements.Length);
        }

        [Fact]
        public void Scan_finds_all_dvpl_items()
        {
            // This test assumes test data in known place.
            // Arrange
            var sut = GetSut();

            // Act
            var dvplItems = sut
                .GetDvplItems()
                .ToArray();

            // Assert
            Dump(dvplItems);

            Assert.Equal(120, dvplItems.Length);
        }

        private FolderScanner GetSut()
        {
            var wordDocumentProcessor = GetWordFileProcessor();

            var psiDvplFileProcessor = new PsiDvplFileProcessor();
            var spcDvplFileProcessor = new SpcDvplFileProcessor();
            var moduleRequirementFileProcessors = GetExcelFileProcessors();

            var sut = new FolderScanner(
                [wordDocumentProcessor],
                moduleRequirementFileProcessors,
                [psiDvplFileProcessor, spcDvplFileProcessor],
                new OptionsWrapper<FileLocationOptions>(new FileLocationOptions { Path = TestDataPath }));
            return sut;
        }
    }
}