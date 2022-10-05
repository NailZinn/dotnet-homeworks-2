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
    | 3 -> printf "ok"
    | _ -> ArgumentException() |> raise

let parseOperation (arg : string) =
    match arg with
    | "+" -> CalculatorOperation.Plus
    | "-" -> CalculatorOperation.Minus
    | "*" -> CalculatorOperation.Multiply
    | "/" -> CalculatorOperation.Divide
    | _ -> ArgumentException("Could not convert given value to an operation") |> raise
    
let parseCalcArguments(args : string[]) =
    assertLength args
    try
        let val1 = Double.Parse(args[0])
        let val2 = Double.Parse(args[2])
        let operation = parseOperation args[1]
        
        { arg1 = val1; arg2 = val2; operation = operation }
    with
        | :? FormatException -> ArgumentException("Could not convert given value to a number") |> raise