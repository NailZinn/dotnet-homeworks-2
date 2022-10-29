using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using Hw8.Calculator;
using Microsoft.AspNetCore.Mvc;

namespace Hw8.Controllers;

public class CalculatorController : Controller
{
    public IActionResult Calculate([FromServices] ICalculator calculator,
        string val1,
        string operation,
        string val2)
    {
        double value1 = 0.0;
        Operation parsedOperation = Operation.Invalid;
        double value2 = 0.0;

        double result = 0.0;

        try
        {
            var parsedData = Parser.Parse(val1, operation, val2);

            value1 = parsedData.value1;
            parsedOperation = parsedData.operation;
            value2 = parsedData.value2;
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }

        try
        {
            result = calculator.Calculate(value1, parsedOperation, value2);
        }
        catch (InvalidOperationException ex)
        {
            return Ok(ex.Message);
        }

        return Ok(result);
    }
    
    [ExcludeFromCodeCoverage]
    public IActionResult Index()
    {
        return Content(
            "Заполните val1, operation(plus, minus, multiply, divide) и val2 здесь '/calculator/calculate?val1= &operation= &val2= '\n" +
            "и добавьте её в адресную строку.");
    }
}