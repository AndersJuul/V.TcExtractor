using V.TcExtractor.InputParsing.Adapters.FileAdapters;
using V.TcExtractor.InputParsing.Adapters.TableAdapters;
using Xunit.Abstractions;
using V.TcExtractor.Model;

namespace V.TcExtractor.InputParsing.Tests.Base;

public abstract class TestCaseTests(ITestOutputHelper testOutputHelper)
{
    protected readonly ITestOutputHelper TestOutputHelper = testOutputHelper;
    protected readonly string TestDataBasePath = "C:\\DATA\\DVPR";

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

    protected static WordFileProcessor GetWordFileProcessor()
    {
        var cellAdapter = new CellAdapter();
        var wordFileProcessor = new WordFileProcessor(
            [
                new TableAdapterId(cellAdapter),
                new TableAdapterTestCaseIdAndDescriptionHeadersInRowZero(cellAdapter),
                new TableAdapterTestCaseInformationHeadersInColZero(cellAdapter),
                new TableAdapterTestCaseInformationHeadersInRowZero(cellAdapter),
                new TableAdapterTestCaseIdInitialConditionsHeadersInRowZero(cellAdapter)
            ],
            cellAdapter);
        return wordFileProcessor;
    }
}