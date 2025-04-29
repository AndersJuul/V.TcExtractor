using V.TcExtractor.Model;

namespace V.TcExtractor.OutputFormatting
{
    public class TestCaseOutputConsole : ITestCaseOutput
    {
        public bool CanHandle(string formatId)
        {
            return formatId.Equals("console", StringComparison.InvariantCultureIgnoreCase);
        }

        public void Write(TestCase[] testCases)
        {
            System.Console.WriteLine(testCases.Length);
            foreach (var testCase in testCases)
            {
                System.Console.WriteLine(testCase);
            }
        }

        public void Write(ModuleRequirement[] moduleRequirements)
        {
            throw new NotImplementedException();
        }
    }
}