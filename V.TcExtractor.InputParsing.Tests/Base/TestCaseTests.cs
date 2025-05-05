using V.TcExtractor.InputParsing.Adapters.FileAdapters;
using V.TcExtractor.InputParsing.Adapters.TableAdapters;
using Xunit.Abstractions;
using V.TcExtractor.Model;

namespace V.TcExtractor.InputParsing.Tests.Base;

public abstract class TestCaseTests(ITestOutputHelper testOutputHelper)
{
    protected readonly ITestOutputHelper TestOutputHelper = testOutputHelper;
    protected readonly string TestDataPath = "C:\\DATA\\v";

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

    protected static WordFileProcessor GetWordFileProcessor()
    {
        var cellAdapter = new CellAdapter();
        var wordFileProcessor = new WordFileProcessor(
        [
            new TableAdapterId(cellAdapter),
            new TableAdapterTestCaseIdAndDescriptionHeadersInRowZero(cellAdapter),
            new TableAdapterTestCaseIdSubjectHeadersInColZero(cellAdapter),
            new TableAdapterTestCaseInformationHeadersInRowZero(cellAdapter),
            new TableAdapterTestCaseIdInitialConditionsHeadersInRowZero(cellAdapter)
        ]);
        return wordFileProcessor;
    }

    protected static ExcelFileProcessor GetExcelFileProcessor()
    {
        return new ExcelFileProcessor();
    }
}