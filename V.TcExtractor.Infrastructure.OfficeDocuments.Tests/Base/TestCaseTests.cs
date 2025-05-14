using V.TcExtractor.Domain.Model;
using V.TcExtractor.Domain.Processors;
using V.TcExtractor.Infrastructure.OfficeDocuments.Adapters.CellAdapters;
using V.TcExtractor.Infrastructure.OfficeDocuments.Adapters.FileAdapters;
using V.TcExtractor.Infrastructure.OfficeDocuments.Adapters.TableAdapters;
using Xunit.Abstractions;

namespace V.TcExtractor.Infrastructure.OfficeDocuments.Tests.Base;

public abstract class TestCaseTests(ITestOutputHelper testOutputHelper)
{
    protected readonly ITestOutputHelper TestOutputHelper = testOutputHelper;
    protected readonly string TestDataPath = "C:\\Users\\aju\\Mit drev\\V";

    protected void Dump(IEnumerable<TestCase> testCases)
    {
        TestOutputHelper.WriteLine("TestCases :");
        TestOutputHelper.WriteLine($"---- {testCases.Count()} cases");
        foreach (var testCase in testCases)
        {
            TestOutputHelper.WriteLine($"Test : {testCase}");
        }

        TestOutputHelper.WriteLine("----");
    }

    protected void Dump(IEnumerable<ModuleRequirement> moduleRequirements)
    {
        TestOutputHelper.WriteLine("Module Requirements:");
        TestOutputHelper.WriteLine($"---- {moduleRequirements.Count()} module requirements");
        foreach (var moduleRequirement in moduleRequirements)
        {
            TestOutputHelper.WriteLine($"Module Requirement : {moduleRequirement}");
        }

        TestOutputHelper.WriteLine("----");
    }

    protected void Dump(DvplItem[] dvplItems)
    {
        TestOutputHelper.WriteLine("DVPL Items:");
        TestOutputHelper.WriteLine($"---- {dvplItems.Count()} DVPL Items");
        foreach (var dvplItem in dvplItems)
        {
            TestOutputHelper.WriteLine($"DVPL Item : {dvplItem}");
        }

        TestOutputHelper.WriteLine("----");
    }

    protected static TestCaseFileProcessor GetWordFileProcessor()
    {
        var cellAdapter = new CellAdapter();
        var wordFileProcessor = new TestCaseFileProcessor(
        [
            new TableAdapterId(cellAdapter),
            new TableAdapterTestCaseIdAndDescriptionHeadersInRowZero(cellAdapter),
            new TableAdapterTestCaseIdSubjectHeadersInColZero(cellAdapter),
            new TableAdapterTestCaseInformationHeadersInRowZero(cellAdapter),
            new TableAdapterTestCaseIdInitialConditionsHeadersInRowZero(cellAdapter)
        ]);
        return wordFileProcessor;
    }

    protected static IModuleRequirementFileProcessor[] GetExcelFileProcessors()
    {
        return
        [
            new ModuleRequirementFileProcessorPsi(),
            new ModuleRequirementFileProcessorSpc()
        ];
    }

    protected ITestResultFileProcessor GetTestResultFileProcessor()
    {
        var cellAdapter = new CellAdapter();
        var wordFileProcessor = new TestResultFileProcessor(
        [
            new TableAdapterId(cellAdapter),
            new TableAdapterTestCaseIdAndDescriptionHeadersInRowZero(cellAdapter),
            new TableAdapterTestCaseIdSubjectHeadersInColZero(cellAdapter),
            new TableAdapterTestCaseInformationHeadersInRowZero(cellAdapter),
            new TableAdapterTestCaseIdInitialConditionsHeadersInRowZero(cellAdapter)
        ]);
        return wordFileProcessor;
    }
}