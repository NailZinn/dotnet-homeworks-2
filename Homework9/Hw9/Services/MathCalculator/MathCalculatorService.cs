using Hw9.Dto;
using Hw9.Expressions;
using Hw9.Tokens.Math;
using System.Linq.Expressions;

namespace Hw9.Services.MathCalculator;

public class MathCalculatorService : IMathCalculatorService
{
    public async Task<CalculationMathExpressionResultDto> CalculateMathExpressionAsync(string? expression)
    {
        Expression tree = null;

        try
        {
            tree = await Task.Run(() => Parser.Parse(expression));
        }
        catch (ArgumentException ex)
        {
            return new CalculationMathExpressionResultDto(ex.Message);
        }

        var executeBefore = await Task.Run(() => new MathExpressionConverter().ToDictionary(tree));

        try
        {
            var result = await MathExpressionCalculator.CalculateAsync(executeBefore);
            return new CalculationMathExpressionResultDto(result);
        }
        catch (DivideByZeroException ex)
        {
            return new CalculationMathExpressionResultDto(ex.Message);
        }
    }
}