using Hw1;

public class Program
{
    static void Main(string[] args)
    {
        string[] data = GetArguments(args);

        try
        {
            Parser.ParseCalcArguments(data, out double val1, out CalculatorOperation operation, out double val2);
            Console.Write(Calculator.Calculate(val1, operation, val2));
        }
        catch (ArgumentException ex)
        {
            Console.WriteLine(ex.Message);
        }
        catch (InvalidOperationException ex)
        {
            Console.WriteLine(ex.Message);
        }
    }

    static string[] GetArguments(string[] resource)
    {
        if (resource.Length != 0)
            return resource;

        var str = Console.ReadLine();
        var args = str.Split(' ', StringSplitOptions.RemoveEmptyEntries);

        return args.Length == 3 ? args : throw new ArgumentException("Expression must contain 2 values and 1 operation");
    }
}