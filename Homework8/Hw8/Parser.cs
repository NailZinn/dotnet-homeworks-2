using Hw8.Calculator;
using System.Globalization;

namespace Hw8
{
    public class Parser
    {
        public static (double value1, Operation operation, double value2) Parse(string val1, string operation, string val2)
        {
            var value1 = ParseValue(val1);
            var value2 = ParseValue(val2);
            var parsedOperation = ParseOperation(operation);

            return (value1, parsedOperation, value2);
        }

        private static double ParseValue(string value)
        {
            var isValueParsed = double.TryParse(value, NumberStyles.AllowDecimalPoint, 
                CultureInfo.InvariantCulture, out double result);

            return isValueParsed ? result : throw new ArgumentException(Messages.InvalidNumberMessage);
        }

        private static Operation ParseOperation(string operation)
        {
            return operation switch
            {
                "Plus" => Operation.Plus,
                "Minus" => Operation.Minus,
                "Multiply" => Operation.Multiply,
                "Divide" => Operation.Divide,
                _ => throw new InvalidOperationException(Messages.InvalidOperationMessage)
            };
        }
    }
}
