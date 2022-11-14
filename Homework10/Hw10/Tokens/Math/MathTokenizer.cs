using Hw10.ErrorMessages;

namespace Hw10.Tokens.Math
{
    public static class MathTokenizer
    {
        public static IEnumerable<MathToken> Tokenize(string expression)
        {
            var parts = expression.Split(' ');
            var digits = new List<char>();

            foreach (var part in parts)
            {
                var curPos = 0;
                while (curPos < part.Length)
                {
                    while (curPos < part.Length && char.IsDigit(part[curPos]))
                    {
                        digits.Add(part[curPos]);
                        curPos++;
                    }

                    if (digits.Count != 0)
                    {
                        yield return TokenizeNumber(digits);
                        digits.Clear();
                        continue;
                    }

                    MathToken token = null;

                    try
                    {
                        token = TokenizeOperator(part, curPos);
                    }
                    catch (ArgumentException ex)
                    {
                        if (part.Length == 1)
                            throw new ArgumentException(ex.Message);
                        throw new ArgumentException($"{MathErrorMessager.NotNumberMessage(part)}");
                    }

                    yield return token;
                    curPos++;
                }
            }
        }

        private static MathToken TokenizeOperator(string part, int position)
        {
            return part[position] switch
            {
                '(' => new MathToken(part[position].ToString(), MathTokenType.ParantheseOpen),
                ')' => new MathToken(part[position].ToString(), MathTokenType.ParentheseClose),
                '+' => new MathToken(part[position].ToString(), MathTokenType.Add),
                '-' => TokenizeMinus(part, position),
                '*' => new MathToken(part[position].ToString(), MathTokenType.Multiply),
                '/' => new MathToken(part[position].ToString(), MathTokenType.Divide),
                _ => throw new ArgumentException($"{MathErrorMessager.UnknownCharacterMessage(part[position])}")
            };
        }

        private static MathToken TokenizeNumber(List<char> digits)
            => new MathToken(string.Join("", digits), MathTokenType.Number);

        private static MathToken TokenizeMinus(string part, int position)
        {
            var tokenType =
                part.Length == 1 ? MathTokenType.Substract : MathTokenType.Negate;
            return new MathToken(part[position].ToString(), tokenType);
        }
    }
}
