module Hw5.Parser

open System
open Hw5.Calculator
open Hw5.MaybeBuilder
open System.Globalization

let (|CalculatorOperation|_|) arg =
    match arg with
    | Plus -> Some CalculatorOperation.Plus
    | Minus -> Some CalculatorOperation.Minus
    | Multiply -> Some CalculatorOperation.Multiply
    | Divide -> Some CalculatorOperation.Divide
    | _ -> None
    
let (|Double|_|) (arg: string)=
    match Double.TryParse(arg, NumberStyles.AllowDecimalPoint, Globalization.CultureInfo.InvariantCulture) with
    | true, double -> Some double
    | _ -> None

let isArgLengthSupported (args:string[]): Result<'a,'b> =
    match args.Length with
    | 3 -> Ok args
    | _ -> Error Message.WrongArgLength
    
[<System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage>]
let inline isOperationSupported (arg1, operation, arg2): Result<('a * CalculatorOperation * 'b), Message> =
    match operation with
    | CalculatorOperation operation -> Ok (arg1, operation, arg2)
    | _ -> Error Message.WrongArgFormatOperation

let parseArgs (args: string[]): Result<('a * string * 'b), Message> =
    match args[0] with
    | Double val1 ->
        match args[2] with
        | Double val2 -> Ok (val1, args[1], val2)
        | _ -> Error Message.WrongArgFormatForValue2
    | _ -> Error Message.WrongArgFormatForValue1

[<System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage>]
let inline isDividingByZero (arg1, operation, arg2): Result<('a * CalculatorOperation * 'b), Message> =
    match arg2 = 0.0 && operation = CalculatorOperation.Divide with
    | true -> Error Message.DivideByZero
    | false -> Ok (arg1, operation, arg2)
    
let parseCalcArguments (args: string[]): Result<'a, 'b> =
    maybe {
        let! argsWithCorrectLength = isArgLengthSupported args
        let! argsWithCorrectValues = parseArgs argsWithCorrectLength
        let! argsWithCorrectValuesAndOperation = isOperationSupported argsWithCorrectValues
        let! noDivisionWithVal2EqualsToZero = isDividingByZero argsWithCorrectValuesAndOperation
        return noDivisionWithVal2EqualsToZero
    }