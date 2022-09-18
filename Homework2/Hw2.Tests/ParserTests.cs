using Hw2;
using Xunit;

namespace Hw2Tests
{
    public class ParserTests
    {
        [Theory]
        [InlineData("+", CalculatorOperation.Plus)]
        [InlineData("-", CalculatorOperation.Minus)]
        [InlineData("*", CalculatorOperation.Multiply)]
        [InlineData("/", CalculatorOperation.Divide)]
        public void TestCorrectOperations(string operation, CalculatorOperation operationExpected)
        {
            string[] args = { "15", operation, "10" };
            
            Parser.ParseCalcArguments(args, out var val1, out var operationResult, out var val2);
            
            Assert.Equal(15, val1);
            Assert.Equal(operationExpected, operationResult);
            Assert.Equal(10, val2);
        }
        
        [Theory]
        [InlineData("f", "+", "3")]
        [InlineData("3", "+", "f")]
        [InlineData("a", "+", "f")]
        public void TestParserWrongValues(string val1, string operation, string val2)
        {
            string[] args = { val1, operation, val2 };

            Assert.Throws<ArgumentException>(() => Parser.ParseCalcArguments(args, out _, out _, out _));
        }
        
        [Fact]
        public void TestParserWrongOperation()
        {
            var args = new[] { "3", ".", "4" };
            
            Assert.Throws<InvalidOperationException>(() => Parser.ParseCalcArguments(args, out _, out _, out _));
        }

        [Fact]
        public void TestParserWrongLength()
        {
            var args = new[] { "3", ".", "4", "5" };
            
            Assert.Throws<ArgumentException>(() => Parser.ParseCalcArguments(args, out _, out _, out _));
        }
    }
}
