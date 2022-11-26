using Hw11.Dto;
using Hw11.Expressions;
using System.Linq.Expressions;

namespace Hw11.Services.MathCalculator;

public class MathCalculatorService : IMathCalculatorService
{
    public async Task<double> CalculateMathExpressionAsync(string? expression)
    {
        Expression tree;

        tree = await Task.Run(() => Parser.Parse(expression));

        var executeBefore = await Task.Run(() => new MathExpressionConverter().ToDictionary(tree));

        var result = await MathExpressionCalculator.CalculateAsync(executeBefore);
        return result;
    }
}