using Hw1;
using System.Text.RegularExpressions;

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
        return Regex.Split(str, @"([*()\^\/]|[\+\-])");
    }
}