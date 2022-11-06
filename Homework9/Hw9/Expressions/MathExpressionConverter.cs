using System.Linq.Expressions;

namespace Hw9.Expressions
{
    public class MathExpressionConverter
    {
        private readonly Dictionary<Expression, Expression[]> executeBefore = new();

        public Dictionary<Expression, Expression[]> ToDictionary(Expression tree)
        {
            Visit(tree);
            return executeBefore;
        }

        private void Visit(Expression node)
        {
            if (node is BinaryExpression binary)
                VisitBinary(binary);
            else if (node is UnaryExpression unary)
                VisitUnary(unary);
            else
                VisitConstant((ConstantExpression)node);
        }

        private void VisitBinary(BinaryExpression node)
        {
            if (!executeBefore.ContainsKey(node))
            {
                executeBefore.Add(node, new[] { node.Left, node.Right });
            }

            Visit(node.Left);
            Visit(node.Right);
        }

        private void VisitUnary(UnaryExpression node)
        {
            if (!executeBefore.ContainsKey(node))
            {
                executeBefore.Add(node, new[] { node.Operand });
            }

            Visit(node.Operand);
        }

        private void VisitConstant(ConstantExpression node)
        {
            if (!executeBefore.ContainsKey(node))
            {
                executeBefore.Add(node, new Expression[0]);
            }
        }
    }
}
