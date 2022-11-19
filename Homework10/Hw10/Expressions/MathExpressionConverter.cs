using System.Linq.Expressions;

namespace Hw10.Expressions
{
    public class MathExpressionConverter
    {
        public Dictionary<Expression, Expression[]> ToDictionary(Expression tree)
        {
            var executeBefore = new Dictionary<Expression, Expression[]>();
            Visit(tree, executeBefore);
            return executeBefore;
        }

        private void Visit(Expression node, Dictionary<Expression, Expression[]> executeBefore)
        {
            if (node is BinaryExpression binary)
                VisitBinary(binary, executeBefore);
            else if (node is UnaryExpression unary)
                VisitUnary(unary, executeBefore);
            else
                VisitConstant((ConstantExpression)node, executeBefore);
        }

        private void VisitBinary(BinaryExpression node, Dictionary<Expression, Expression[]> executeBefore)
        {
            if (!executeBefore.ContainsKey(node))
            {
                executeBefore.Add(node, new[] { node.Left, node.Right });
            }

            Visit(node.Left, executeBefore);
            Visit(node.Right, executeBefore);
        }

        private void VisitUnary(UnaryExpression node, Dictionary<Expression, Expression[]> executeBefore)
        {
            if (!executeBefore.ContainsKey(node))
            {
                executeBefore.Add(node, new[] { node.Operand });
            }

            Visit(node.Operand, executeBefore);
        }

        private void VisitConstant(ConstantExpression node, Dictionary<Expression, Expression[]> executeBefore)
        {
            if (!executeBefore.ContainsKey(node))
            {
                executeBefore.Add(node, new Expression[0]);
            }
        }
    }
}
