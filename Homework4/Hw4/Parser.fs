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
    | _ -> ArgumentException() |> raise

let parseOperation (arg : string) =
    match arg with
    | "+" -> CalculatorOperation.Plus
    | "-" -> CalculatorOperation.Minus
    | "*" -> CalculatorOperation.Multiply
    | "/" -> CalculatorOperation.Divide
    | _ -> InvalidOperationException("Could not convert given value to an operation") |> raise
    
let parseCalcArguments(args : string[]) =
    assertLength args |> ignore
    
    let couldParseVal1, val1 = Double.TryParse args[0]
    let couldParseVal2, val2 = Double.TryParse args[2]
    let operation = parseOperation args[1]
    
    if (couldParseVal1 |> not || couldParseVal2 |> not) then
        ArgumentException("Could not convert given value to a number") |> raise
        
    { arg1 = val1; arg2 = val2; operation = operation }