using System.Diagnostics.CodeAnalysis;

namespace Hw11.Tokens.Math
{
    [ExcludeFromCodeCoverage]
    public static class MathTokenHelper
    {
        public static int GetOperatorPrecedence(MathTokenType tokenName)
        {
            return tokenName switch
            {
                MathTokenType.ParantheseOpen or MathTokenType.ParentheseClose => -1,
                MathTokenType.Add or MathTokenType.Substract => 0,
                MathTokenType.Multiply or MathTokenType.Divide => 1,
                MathTokenType.Negate => 2,
                _ => throw new ArgumentException($"{tokenName} is not operator")
            };
        }
    }
}
