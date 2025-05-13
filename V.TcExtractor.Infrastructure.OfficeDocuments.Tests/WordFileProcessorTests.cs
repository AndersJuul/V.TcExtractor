using V.TcExtractor.Infrastructure.OfficeDocuments.Adapters.FileAdapters;
using V.TcExtractor.Infrastructure.OfficeDocuments.Tests.Base;
using Xunit.Abstractions;

namespace V.TcExtractor.Infrastructure.OfficeDocuments.Tests
{
    public class WordFileProcessorTests(ITestOutputHelper testOutputHelper) : TestCaseTests(testOutputHelper)
    {
        [Fact]
        public void CanHandle_returns_true_for_word_file_name()
        {
            // Arrange
            var sut = GetSut();

            // Act
            var canHandle = sut.CanHandle("A DVPR.docx");

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

        [Fact]
        public void Handle_returns_testcases_for_spc_ves_dvpr()
        {
            // Arrange
            var sut = GetSut();

            // Act
            var testCases = sut
                .GetTestCases(Path.Combine(TestDataPath, "DVPR", "SPC", "VES DVPR.docx"));

            // Assert
            Dump(testCases);
            Assert.Equal(38, testCases.Count);
        }

        [Fact]
        public void Handle_returns_testcases_for_psi_dvpr_ves_multithreading()
        {
            // Arrange
            var sut = GetSut();

            // Act
            var testCases = sut
                .GetTestCases(Path.Combine(TestDataPath, "DVPR", "PSI", "DVPR VES  Multithreading.docx"))
                .ToArray();

            // Assert
            Dump(testCases);
            Assert.Equal(2, testCases.Count());
            // Original input file modified to have ReqId == REQ-1 for testing purposes (was blank)
            // Assert.Equal("REQ-1", testCases[0].ReqId);
            Assert.All(testCases, x => Assert.False(string.IsNullOrEmpty(x.TestNo)));
            Assert.All(testCases, x => Assert.False(string.IsNullOrEmpty(x.FileName)));
            Assert.All(testCases, x => Assert.False(string.IsNullOrEmpty(x.Description)));
        }

        [Fact]
        public void Handle_returns_testcases_for_psi_dvpr_ves_bess_interface()
        {
            // Arrange
            var sut = GetSut();

            // Act
            var testCases = sut
                .GetTestCases(Path.Combine(TestDataPath, "DVPR", "PSI", "DVPR VES BESS Interface.docx"))
                .ToArray();

            // Assert
            Dump(testCases);
            Assert.Equal(2, testCases.Count());

            Assert.All(testCases, x => Assert.False(string.IsNullOrEmpty(x.TestNo)));
            Assert.All(testCases, x => Assert.False(string.IsNullOrEmpty(x.FileName)));
            Assert.All(testCases, x => Assert.False(string.IsNullOrEmpty(x.Description)));
        }

        [Fact]
        public void Handle_returns_testcases_for_psi_dvpr_ves_data_logging()
        {
            // Arrange
            var sut = GetSut();

            // Act
            var testCases = sut
                .GetTestCases(Path.Combine(TestDataPath, "DVPR", "PSI", "DVPR VES Data Logging.docx"))
                .ToArray();

            // Assert
            Dump(testCases);
            Assert.Equal(6, testCases.Count());

            Assert.All(testCases, x => Assert.False(string.IsNullOrEmpty(x.TestNo)));
            Assert.All(testCases, x => Assert.False(string.IsNullOrEmpty(x.FileName)));
            Assert.All(testCases, x => Assert.False(string.IsNullOrEmpty(x.Description)));
        }

        [Fact]
        public void Handle_returns_testcases_for_psi_dvpr_ves_data_tools()
        {
            // Arrange
            var sut = GetSut();

            // Act
            var testCases = sut
                .GetTestCases(Path.Combine(TestDataPath, "DVPR", "PSI", "DVPR VES Data Tools.docx"))
                .ToArray();

            // Assert
            Dump(testCases);
            Assert.Equal(3, testCases.Count());

            Assert.All(testCases, x => Assert.False(string.IsNullOrEmpty(x.TestNo)));
            Assert.All(testCases, x => Assert.False(string.IsNullOrEmpty(x.FileName)));
            Assert.All(testCases, x => Assert.False(string.IsNullOrEmpty(x.Description)));
        }

        [Fact]
        public void Handle_returns_testcases_for_psi_dvpr_ves_environmental_control()
        {
            // Arrange
            var sut = GetSut();

            // Act
            var testCases = sut
                .GetTestCases(Path.Combine(TestDataPath, "DVPR", "PSI", "DVPR VES Environmental Control.docx"))
                .ToArray();

            // Assert
            Dump(testCases);
            Assert.Single(testCases);

            Assert.All(testCases, x => Assert.False(string.IsNullOrEmpty(x.TestNo)));
            Assert.All(testCases, x => Assert.False(string.IsNullOrEmpty(x.FileName)));
            Assert.All(testCases, x => Assert.False(string.IsNullOrEmpty(x.Description)));
        }

        [Fact]
        public void Handle_returns_testcases_for_psi_dvpr_ves_opc_ua_interface()
        {
            // Arrange
            var sut = GetSut();

            // Act
            var testCases = sut
                .GetTestCases(Path.Combine(TestDataPath, "DVPR", "PSI", "DVPR VES OPC-UA Interface.docx"))
                .ToArray();

            // Assert
            Dump(testCases);
            Assert.Equal(3, testCases.Count());

            Assert.All(testCases, x => Assert.False(string.IsNullOrEmpty(x.TestNo)));
            Assert.All(testCases, x => Assert.False(string.IsNullOrEmpty(x.FileName)));
            Assert.All(testCases, x => Assert.False(string.IsNullOrEmpty(x.Description)));
        }

        [Fact]
        public void Handle_returns_testcases_for_psi_dvpr_ves_power_meter()
        {
            // Arrange
            var sut = GetSut();

            // Act
            var testCases = sut
                .GetTestCases(Path.Combine(TestDataPath, "DVPR", "PSI", "DVPR VES Power Meter.docx"))
                .ToArray();

            // Assert
            Dump(testCases);
            Assert.Equal(11, testCases.Count());

            Assert.All(testCases, x => Assert.False(string.IsNullOrEmpty(x.TestNo)));
            Assert.All(testCases, x => Assert.False(string.IsNullOrEmpty(x.FileName)));
            Assert.All(testCases, x => Assert.False(string.IsNullOrEmpty(x.Description)));
        }

        [Fact]
        public void Handle_returns_testcases_for_psi_dvpr_ves_ppc_resources_and_scaling()
        {
            // Arrange
            var sut = GetSut();

            // Act
            var testCases = sut
                .GetTestCases(Path.Combine(TestDataPath, "DVPR", "PSI", "DVPR VES PPC Resources and Scaling.docx"))
                .ToArray();

            // Assert
            Dump(testCases);
            Assert.Equal(4, testCases.Count());

            // Special case: TC_ ReSc _001 covers multiple requirements
            var testCaseTC_ReSc_001 = testCases.Single(x => x.TestNo == "TC_ReSc_001");
            Assert.Contains("PSI_1.1.2", testCaseTC_ReSc_001.ReqId);
            Assert.Contains("PSI_1.1.4", testCaseTC_ReSc_001.ReqId);
            Assert.Contains("PSI_57.1", testCaseTC_ReSc_001.ReqId);
            Assert.Contains("PSI_57.2", testCaseTC_ReSc_001.ReqId);

            Assert.All(testCases, x => Assert.False(string.IsNullOrEmpty(x.TestNo)));
            Assert.All(testCases, x => Assert.False(string.IsNullOrEmpty(x.FileName)));
            Assert.All(testCases, x => Assert.False(string.IsNullOrEmpty(x.Description)));
        }

        [Fact]
        public void Handle_returns_testcases_for_psi_dvpr_ves_pv_interface()
        {
            // Arrange
            var sut = GetSut();

            // Act
            var testCases = sut
                .GetTestCases(Path.Combine(TestDataPath, "DVPR", "PSI", "DVPR VES PV Interface.docx"))
                .ToArray();

            // Assert
            Dump(testCases);
            Assert.Equal(2, testCases.Count());

            Assert.All(testCases, x => Assert.False(string.IsNullOrEmpty(x.TestNo)));
            Assert.All(testCases, x => Assert.False(string.IsNullOrEmpty(x.FileName)));
            Assert.All(testCases, x => Assert.False(string.IsNullOrEmpty(x.Description)));
        }

        [Fact]
        public void Handle_returns_testcases_for_psi_dvpr_ves_ssl()
        {
            // Arrange
            var sut = GetSut();

            // Act
            var testCases = sut
                .GetTestCases(Path.Combine(TestDataPath, "DVPR", "PSI", "DVPR VES SSL.docx"))
                .ToArray();

            // Assert
            Dump(testCases);
            Assert.Equal(10, testCases.Count());

            Assert.All(testCases, x => Assert.False(string.IsNullOrEmpty(x.TestNo)));
            Assert.All(testCases, x => Assert.False(string.IsNullOrEmpty(x.FileName)));
            // TODO -- document is clearly not done. Assert.All(testCases, x => Assert.False(string.IsNullOrEmpty(x.Description)));
        }

        [Fact]
        public void Handle_returns_testcases_for_psi_dvpr_ves_statcom_interface()
        {
            // Arrange
            var sut = GetSut();

            // Act
            var testCases = sut
                .GetTestCases(Path.Combine(TestDataPath, "DVPR", "PSI", "DVPR VES STATCOM Interface.docx"))
                .ToArray();

            // Assert
            Dump(testCases);
            Assert.Equal(3, testCases.Count());

            Assert.All(testCases, x => Assert.False(string.IsNullOrEmpty(x.TestNo)));
            Assert.All(testCases, x => Assert.False(string.IsNullOrEmpty(x.FileName)));
            Assert.All(testCases, x => Assert.False(string.IsNullOrEmpty(x.Description)));
        }

        [Fact]
        public void Handle_returns_testcases_for_psi_dvpr_ves_supervisions_and_monitoring()
        {
            // Arrange
            var sut = GetSut();

            // Act
            var testCases = sut
                .GetTestCases(Path.Combine(TestDataPath, "DVPR", "PSI", "DVPR VES Supervisions and Monitoring.docx"))
                .ToArray();

            // Assert
            Dump(testCases);
            Assert.Equal(39, testCases.Count());

            Assert.All(testCases, x => Assert.False(string.IsNullOrEmpty(x.TestNo)));
            Assert.All(testCases, x => Assert.False(string.IsNullOrEmpty(x.FileName)));
            Assert.All(testCases, x => Assert.False(string.IsNullOrEmpty(x.Description)));
        }

        [Fact]
        public void Handle_returns_testcases_for_psi_dvpr_ves_wtg_interface()
        {
            // Arrange
            var sut = GetSut();

            // Act
            var testCases = sut
                .GetTestCases(Path.Combine(TestDataPath, "DVPR", "PSI", "DVPR VES WTG Interface.docx"))
                .ToArray();

            // Assert
            Dump(testCases);
            Assert.Equal(3, testCases.Count());

            Assert.All(testCases, x => Assert.False(string.IsNullOrEmpty(x.TestNo)));
            Assert.All(testCases, x => Assert.False(string.IsNullOrEmpty(x.FileName)));
            Assert.All(testCases, x => Assert.False(string.IsNullOrEmpty(x.Description)));
        }

        [Fact]
        public void Handle_returns_testcases_for_psi_dvpr_re_veppc()
        {
            // Arrange
            var sut = GetSut();

            // Act
            var testCases = sut
                .GetTestCases(Path.Combine(TestDataPath, "DVPR", "PSI", "DVPR-RE VEPPC Lab.docx"))
                .ToArray();

            // Assert
            Dump(testCases);
            Assert.Equal(20, testCases.Count());

            Assert.All(testCases, x => Assert.False(string.IsNullOrEmpty(x.TestNo)));
            Assert.All(testCases, x => Assert.False(string.IsNullOrEmpty(x.FileName)));
            Assert.All(testCases, x => Assert.False(string.IsNullOrEmpty(x.Description)));
        }

        [Fact]
        public void Handle_returns_testcases_for_spc_dvpr_ves_framework()
        {
            // Arrange
            var sut = GetSut();

            // Act
            var testCases = sut
                .GetTestCases(Path.Combine(TestDataPath, "DVPR", "SPC", "DVPR VES - Framework.docx"))
                .ToArray();

            // Assert
            Dump(testCases);
            Assert.Equal(14, testCases.Count());

            Assert.All(testCases, x => Assert.False(string.IsNullOrEmpty(x.TestNo)));
            Assert.All(testCases, x => Assert.False(string.IsNullOrEmpty(x.FileName)));
            Assert.All(testCases, x => Assert.False(string.IsNullOrEmpty(x.Description)));
        }

        [Fact]
        public void Handle_returns_testcases_for_spc_dvpr_ves_io()
        {
            // Arrange
            var sut = GetSut();

            // Act
            var testCases = sut
                .GetTestCases(Path.Combine(TestDataPath, "DVPR", "SPC", "DVPR VES - IO.docx"))
                .ToArray();

            // Assert
            Dump(testCases);
            Assert.Equal(4, testCases.Count());

            Assert.All(testCases, x => Assert.False(string.IsNullOrEmpty(x.TestNo)));
            Assert.All(testCases, x => Assert.False(string.IsNullOrEmpty(x.FileName)));
            Assert.All(testCases, x => Assert.False(string.IsNullOrEmpty(x.Description)));
        }

        [Fact]
        public void Handle_returns_testcases_for_spc_dvpr_ves_vot()
        {
            // Arrange
            var sut = GetSut();

            // Act
            var testCases = sut
                .GetTestCases(Path.Combine(TestDataPath, "DVPR", "SPC", "DVPR VES - VOT.docx"))
                .ToArray();

            // Assert
            Dump(testCases);
            Assert.Equal(36, testCases.Count());

            Assert.All(testCases, x => Assert.False(string.IsNullOrEmpty(x.TestNo)));
            Assert.All(testCases, x => Assert.False(string.IsNullOrEmpty(x.FileName)));
            Assert.All(testCases, x => Assert.False(string.IsNullOrEmpty(x.Description)));
        }

        [Fact]
        public void Handle_returns_testcases_for_spc_test_batch_ves_dvpr_for_base_configuration()
        {
            // Arrange
            var sut = GetSut();

            // Act
            var testCases = sut
                .GetTestCases(Path.Combine(TestDataPath, "DVPR", "SPC",
                    "Tests_Batch - VES DVPR for Base Configuration.docx"))
                .ToArray();

            // Assert
            Dump(testCases);
            Assert.Equal(12, testCases.Count());

            Assert.All(testCases, x => Assert.False(string.IsNullOrEmpty(x.TestNo)));
            Assert.All(testCases, x => Assert.False(string.IsNullOrEmpty(x.FileName)));
            Assert.All(testCases, x => Assert.False(string.IsNullOrEmpty(x.Description)));
        }

        [Fact]
        public void Handle_returns_testcases_for_spc_test_batch_ves_dvpr_for_security_zone_options()
        {
            // Arrange
            var sut = GetSut();

            // Act
            var testCases = sut
                .GetTestCases(Path.Combine(TestDataPath, "DVPR", "SPC",
                    "Tests_Batch - VES DVPR for Security Zone Options.docx"))
                .ToArray();

            // Assert
            Dump(testCases);
            Assert.Equal(12, testCases.Count());

            Assert.All(testCases, x => Assert.False(string.IsNullOrEmpty(x.TestNo)));
            Assert.All(testCases, x => Assert.False(string.IsNullOrEmpty(x.FileName)));
            Assert.All(testCases, x => Assert.False(string.IsNullOrEmpty(x.Description)));
        }

        private static WordFileProcessor GetSut()
        {
            var sut = GetWordFileProcessor();
            return sut;
        }
    }
}