using Hw10.ErrorMessages;
using System.Linq.Expressions;

namespace Hw10.Expressions
{
    public class MathExpressionCalculator
    {
        public static async Task<double> CalculateAsync(Dictionary<Expression, Expression[]> executeBefore)
        {
            var lazy = new Dictionary<Expression, Lazy<Task<double>>>();
            var mainExpression = executeBefore.Keys.First();

            foreach (var (after, before) in executeBefore)
            {
                lazy[after] = new Lazy<Task<double>>(async () =>
                {
                    if (before.Any())
                    {
                        await Task.WhenAll(before.Select(b => lazy[b].Value));
                        await Task.Yield();

                        await Task.Delay(1000);
                    }

                    if (after is BinaryExpression binary)
                    {
                        return Calculate(
                            binary,
                            await lazy[binary.Left].Value,
                            await lazy[binary.Right].Value);
                    }
                    else if (after is UnaryExpression unary)
                    {
                        return Calculate(
                            unary,
                            await lazy[unary.Operand].Value);
                    }
                    else
                    {
                        return Calculate(after);
                    }
                });
            }
            
            return await lazy[mainExpression].Value;
        }

        private static double Calculate(Expression expression, params double[] operands)
        {
            return expression.NodeType switch
            {
                ExpressionType.Add => operands[0] + operands[1],
                ExpressionType.Subtract => operands[0] - operands[1],
                ExpressionType.Multiply => operands[0] * operands[1],
                ExpressionType.Divide =>
                    operands[1] != 0.0 
                        ? operands[0] / operands[1] 
                        : throw new DivideByZeroException(MathErrorMessager.DivisionByZero),
                ExpressionType.Negate => -operands[0],
                ExpressionType.Constant => (double)((ConstantExpression)expression).Value
            };
        }
    }
}
