using Xunit.Abstractions;
using V.TcExtractor.InputParsing.Model;

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
}