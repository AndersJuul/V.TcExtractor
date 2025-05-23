﻿using Microsoft.Extensions.Options;
using V.TcExtractor.Domain.Model;
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
        public void Scan_finds_all_test_results()
        {
            // This test assumes test data in known place.
            // Arrange
            var sut = GetSut();

            // Act
            var testResults = sut
                .GetTestResults()
                .ToArray();

            // Assert
            Dump(testResults);

            Assert.Equal(155, testResults.Count());
        }

        [Fact]
        public void Scan_finds_all_files()
        {
            // This test assumes test data in known place.
            // Arrange
            var sut = GetSut();

            // Act
            var fileItems = sut
                .GetFileItems()
                .ToArray();

            // Assert
            Dump(fileItems);

            Assert.Equal(32, fileItems.Count());
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

            Assert.Equal(235, dvplItems.Length);
        }

        private FolderScanner GetSut()
        {
            var wordDocumentProcessor = GetWordFileProcessor();

            var psiDvplFileProcessor = new PsiDvplFileProcessor();
            var spcDvplFileProcessor = new SpcDvplFileProcessor();
            var moduleRequirementFileProcessors = GetExcelFileProcessors();
            var testResultFileProcessor = GetTestResultNonScadaFileProcessor();

            var sut = new FolderScanner(
                [wordDocumentProcessor],
                moduleRequirementFileProcessors,
                [psiDvplFileProcessor, spcDvplFileProcessor],
                [testResultFileProcessor],
                new OptionsWrapper<FileLocationOptions>(new FileLocationOptions { Path = TestDataPath }),
                [new WordFileProcessor(new DmsNumberAdapter()), new ExcelFileProcessor()]);
            return sut;
        }
    }
}