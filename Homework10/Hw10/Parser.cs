using Hw10.ErrorMessages;
using Hw10.Tokens.Math;
using System.Linq.Expressions;

namespace Hw10
{
    public class Parser
    {
        public static Expression Parse(string? expression)
        {
            if (string.IsNullOrEmpty(expression))
                throw new ArgumentException(MathErrorMessager.EmptyString);

            var tokenize = MathTokenizer.Tokenize(expression).ToList();

            if (tokenize[0].IsOperator)
                throw new ArgumentException(MathErrorMessager.StartingWithOperation);

            if (tokenize[^1].IsOperator)
                throw new ArgumentException(MathErrorMessager.EndingWithOperation);

            var output = new Stack<Expression>();
            var operators = new Stack<MathToken>();

            for (int i = 0; i < tokenize.Count; i++)
            {
                CheckParentheseOpen(tokenize, i);
                CheckParentheseClose(tokenize, i);
                CheckForNeighborOperators(tokenize, i);

                if (tokenize[i].TokenType == MathTokenType.Number)
                    PushExpression(output, tokenize[i]);
                else if (tokenize[i].IsOperator)
                {
                    HandleLowerPrecedenceOperatorCase(operators, tokenize[i], output);
                }
                else if (tokenize[i].TokenType == MathTokenType.ParentheseClose)
                {
                    HandleParenthesisCloseCase(operators, output);
                }
                else
                    operators.Push(tokenize[i]);
            }

            if (operators.Any(oper => oper.TokenType == MathTokenType.ParantheseOpen))
                throw new ArgumentException(MathErrorMessager.IncorrectBracketsNumber);

            foreach (var oper in operators)
                PushExpression(output, oper);

            return output.Pop();
        }

        private static void HandleLowerPrecedenceOperatorCase(Stack<MathToken> operators, MathToken token, Stack<Expression> output)
        {
            while (operators.Count > 0 &&
                   MathTokenHelper.GetOperatorPrecedence(token.TokenType) <=
                   MathTokenHelper.GetOperatorPrecedence(operators.Peek().TokenType))
                PushExpression(output, operators.Pop());
            operators.Push(token);
        }

        private static void HandleParenthesisCloseCase(Stack<MathToken> operators, Stack<Expression> output)
        {
            if (operators.Count == 0)
                throw new ArgumentException(MathErrorMessager.IncorrectBracketsNumber);
            while (operators.Peek().TokenType != MathTokenType.ParantheseOpen)
            {
                if (operators.Count == 0)
                    throw new ArgumentException(MathErrorMessager.IncorrectBracketsNumber);

                PushExpression(output, operators.Pop());
            }

            operators.Pop();
        }

        private static void CheckForNeighborOperators(List<MathToken> tokenize, int i)
        {
            if (tokenize[i].IsOperator && tokenize[i + 1].IsOperator)
                throw new ArgumentException(
                    MathErrorMessager.TwoOperationInRowMessage(tokenize[i].Value, tokenize[i + 1].Value));
        }

        private static void CheckParentheseClose(List<MathToken> tokenize, int i)
        {
            if (tokenize[i].TokenType == MathTokenType.ParentheseClose &&
                tokenize[i - 1].IsOperator)
                throw new ArgumentException(
                    MathErrorMessager.OperationBeforeParenthesisMessage(tokenize[i - 1].Value));
        }

        private static void CheckParentheseOpen(List<MathToken> tokenize, int i)
        {
            if (tokenize[i].TokenType == MathTokenType.ParantheseOpen &&
                tokenize[i + 1].IsOperator &&
                tokenize[i + 1].TokenType != MathTokenType.Negate)
                throw new ArgumentException(
                    MathErrorMessager.InvalidOperatorAfterParenthesisMessage(tokenize[i + 1].Value));
        }

        private static void PushExpression(Stack<Expression> stack, MathToken token)
        {
            if (!token.IsOperator)
            {
                stack.Push(
                    Expression.Constant(double.Parse(token.Value)));
            }
            else
            {
                switch (token.TokenType)
                {
                    case MathTokenType.Add:
                        stack.Push(
                            Expression.Add(stack.Pop(), stack.Pop()));
                        break;
                    case MathTokenType.Substract:
                        var second = stack.Pop();
                        var first = stack.Pop();
                        stack.Push(
                            Expression.Subtract(first, second));
                        break;
                    case MathTokenType.Multiply:
                        stack.Push(
                            Expression.Multiply(stack.Pop(), stack.Pop()));
                        break;
                    case MathTokenType.Divide:
                        var denominator = stack.Pop();
                        var numerator = stack.Pop();
                        stack.Push(
                            Expression.Divide(numerator, denominator));
                        break;
                    case MathTokenType.Negate:
                        stack.Push(
                            Expression.Negate(stack.Pop()));
                        break;
                }
            }
        }
    }
}
