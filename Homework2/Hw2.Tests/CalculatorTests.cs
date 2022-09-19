using Hw2;
using Xunit;

namespace Hw2Tests
{
    public class CalculatorTests
    {
        [Theory]
        [InlineData(15, 5, CalculatorOperation.Plus, 20)]
        [InlineData(15, 5, CalculatorOperation.Minus, 10)]
        [InlineData(15, 5, CalculatorOperation.Multiply, 75)]
        [InlineData(15, 5, CalculatorOperation.Divide, 3)]
        public void Calculate_TwoNumbers_ReturnsRightAnswer(int value1, int value2, CalculatorOperation operation, int expectedValue)
        {
            var actual = Calculator.Calculate(value1, operation, value2);

            Assert.Equal(expectedValue, actual);
        }
        
        [Fact]
        public void Calculate_WithUndefinedOperation_ThrowsException()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => Calculator.Calculate(0, CalculatorOperation.Undefined, 10));
        }

        [Fact]
        public void Calculate_DivideZeroByNonZero_ReturnsZero()
        {
            var actual = Calculator.Calculate(0, CalculatorOperation.Divide, 10);

            Assert.Equal(0, actual);
        }

        [Fact]
        public void Calculate_DivideNonZeroByZero_ReturnsPositiveInfinity()
        {
            var actual = Calculator.Calculate(10, CalculatorOperation.Divide, 0);

            Assert.Equal(double.PositiveInfinity, actual);
        }
        
        [Fact]
        public void Calculate_DivideZeroByZero_ReturnsNaN()
        {
            var actual = Calculator.Calculate(0, CalculatorOperation.Divide, 0);

            Assert.Equal(double.NaN, actual);
        }
    }
}
