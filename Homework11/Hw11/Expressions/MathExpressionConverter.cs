using System.Linq.Expressions;

namespace Hw11.Expressions
{
    public class MathExpressionConverter
    {
        public Dictionary<Expression, Expression[]> ToDictionary(Expression tree)
        {
            var executeBefore = new Dictionary<Expression, Expression[]>();
            Visit((dynamic)tree, executeBefore);
            return executeBefore;
        }

        private void Visit(BinaryExpression node, Dictionary<Expression, Expression[]> executeBefore)
        {
            if (!executeBefore.ContainsKey(node))
            {
                executeBefore.Add(node, new[] { node.Left, node.Right });
            }

            Visit((dynamic)node.Left, executeBefore);
            Visit((dynamic)node.Right, executeBefore);
        }

        private void Visit(UnaryExpression node, Dictionary<Expression, Expression[]> executeBefore)
        {
            if (!executeBefore.ContainsKey(node))
            {
                executeBefore.Add(node, new[] { node.Operand });
            }

            Visit((dynamic)node.Operand, executeBefore);
        }

        private void Visit(ConstantExpression node, Dictionary<Expression, Expression[]> executeBefore)
        {
            if (!executeBefore.ContainsKey(node))
            {
                executeBefore.Add(node, new Expression[0]);
            }
        }
    }
}
