module Hw4.Parser

open System
open Hw4.Calculator

type CalcOptions = {
    arg1: float
    arg2: float
    operation: CalculatorOperation
}

let assertLength (args : string[]) =
    match args.Length with
    | 3 -> None
    | _ -> ArgumentException($"Expression {String.Join(' ', args)} must contain 2 values and 1 operation") |> raise

let parseOperation (arg : string) =
    match arg with
    | "+" -> CalculatorOperation.Plus
    | "-" -> CalculatorOperation.Minus
    | "*" -> CalculatorOperation.Multiply
    | "/" -> CalculatorOperation.Divide
    | _ -> InvalidOperationException($"Could not convert {arg} to an operation. Appropriate operations are '+', '-', '*', '/'") |> raise

let parseArgument (arg: string) =
    let couldParseValue, value = Double.TryParse arg
    if couldParseValue then
        value
    else
        ArgumentException($"Could not convert {value} to a number") |> raise
        
let parseCalcArguments(args : string[]) =
    assertLength args |> ignore
    
    let val1 = parseArgument args[0]
    let val2 = parseArgument args[2]
    let operation = parseOperation args[1]
        
    {
        arg1 = val1
        arg2 = val2
        operation = operation
    }