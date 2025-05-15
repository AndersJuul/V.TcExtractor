using V.TcExtractor.Domain.Adapters;

namespace V.TcExtractor.Domain.Tests
{
    public class PassedTextAdapterTests
    {
        private readonly PassedTextAdapter _adapter;

        public PassedTextAdapterTests()
        {
            _adapter = new PassedTextAdapter();
        }

        [Theory]
        [InlineData("Test failed", "Not Passed")]
        [InlineData("FAILED result", "Not Passed")]
        [InlineData("Something not pass", "Not Passed")]
        [InlineData("This is not pass", "Not Passed")]
        [InlineData("Partial passed but incomplete", "Not Passed")]
        [InlineData("Result: Partial  p assed", "Not Passed")]
        [InlineData("Test pass", "Passed")]
        [InlineData("PASSING case", "Passed")]
        [InlineData("Upload success", "Passed")]
        [InlineData("Great s uccess today", "Passed")]
        [InlineData("Completely irrelevant input", "Not Passed")]
        [InlineData("", "Not Passed")]
        public void GetPassedFromTestResult_ShouldReturnExpectedResult(string input, string expected)
        {
            var result = _adapter.GetPassedFromTestResult(input);
            Assert.Equal(expected, result);
        }
    }
}