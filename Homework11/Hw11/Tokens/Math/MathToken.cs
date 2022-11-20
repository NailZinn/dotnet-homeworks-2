namespace Hw11.Tokens.Math
{
    public class MathToken
    {
        public string Value { get; }
        public MathTokenType TokenType { get; }

        public bool IsOperator
        {
            get
            {
                return TokenType != MathTokenType.Number &&
                       TokenType != MathTokenType.ParantheseOpen &&
                       TokenType != MathTokenType.ParentheseClose;
            }
        }

        public MathToken(string value, MathTokenType tokenType)
        {
            Value = value;
            TokenType = tokenType;
        }
    }
}
