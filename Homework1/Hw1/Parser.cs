namespace Hw1;

public static class Parser
{
    public static void ParseCalcArguments(string[] args, 
        out double val1, 
        out CalculatorOperation operation, 
        out double val2)
    {
        val1 = 0;
        val2 = 0;
        operation = CalculatorOperation.Undefined;

        AssertLength(args);

        try
        {
            val1 = double.Parse(args[0]);
            operation = ParseOperation(args[1]);
            val2 = double.Parse(args[2]);
        }
        catch (FormatException)
        {
            throw new ArgumentException("Could not convert given value to a number");
        }
    }

    private static void AssertLength(string[] args)
    { 
        if (args.Length != 3)
            throw new ArgumentException("Expression must contain 2 values and 1 operation");
    }

    private static CalculatorOperation ParseOperation(string arg)
    {
        return arg switch
        {
            "+" => CalculatorOperation.Plus,
            "-" => CalculatorOperation.Minus,
            "*" => CalculatorOperation.Multiply,
            "/" => CalculatorOperation.Divide,
            _ => throw new InvalidOperationException("Could not convert given value to an operation")
        };
    }
}