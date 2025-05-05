using CsvHelper;
using CsvHelper.Configuration;
using System.Globalization;
using V.TcExtractor.Model;

namespace V.TcExtractor.OutputFormatting
{
    public class TestCaseOutputCsv : ITestCaseOutput
    {
        private readonly OutputFolder _outputFolder;

        public TestCaseOutputCsv(OutputFolder outputFolder)
        {
            _outputFolder = outputFolder;
        }

        public bool CanHandle(string formatId)
        {
            return formatId.Equals("csv", StringComparison.InvariantCultureIgnoreCase);
        }

        public void Write(TestCase[] testCases)
        {
            var filePath = Path.Combine(_outputFolder.Path, "tc_out.csv");

            var config = new QuotedStringCsvConfig();
            using var writer = new StreamWriter(filePath);
            using var csv = new CsvWriter(writer, config);

            // Register the class map
            csv.Context.RegisterClassMap<TestCaseMap>();

            // Write the records
            csv.WriteRecords(testCases);
        }

        public void Write(ModuleRequirement[] moduleRequirements)
        {
            var filePath = Path.Combine(_outputFolder.Path, "rm_out.csv");

            var config = new QuotedStringCsvConfig();
            using var writer = new StreamWriter(filePath);
            using var csv = new CsvWriter(writer, config);

            // Register the class map
            csv.Context.RegisterClassMap<ModuleRequirementMap>();

            // Write the records
            csv.WriteRecords(moduleRequirements);
        }

        public class TestCaseMap : ClassMap<TestCase>
        {
            public TestCaseMap()
            {
                Map(m => m.TestNo).Name("Test Number").Index(0);
                Map(m => m.ReqId).Name("Requirement ID").Index(1);
                Map(m => m.FileName).Name("File Name").Index(2);
                Map(m => m.Description).Name("Description").Index(3);
            }
        }

        public class ModuleRequirementMap : ClassMap<ModuleRequirement>
        {
            public ModuleRequirementMap()
            {
                Map(m => m.Id).Name("Id").Index(0);
                Map(m => m.RsTitle).Name("RsTitle").Index(1);
                Map(m => m.CombinedRequirement).Name("CombinedRequirement").Index(2);
                Map(m => m.Motivation).Name("Motivation").Index(3);
                Map(m => m.FileName).Name("FileName").Index(4);
            }
        }

        public record QuotedStringCsvConfig : CsvConfiguration
        {
            public QuotedStringCsvConfig() : base(CultureInfo.InvariantCulture)
            {
                ShouldQuote = args => true; // Quote all fields
            }
        }
    }
}