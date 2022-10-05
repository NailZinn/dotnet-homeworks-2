open System
open System.Diagnostics.CodeAnalysis
open Hw4

[<ExcludeFromCodeCoverage>]
let getArguments (resource : string[]) : string[] =
    match resource.Length with
    | 0 ->
        let str: string = Console.ReadLine()
        let args: string[] = str.Split(' ', StringSplitOptions.RemoveEmptyEntries)
        args
    | _ -> resource
        
[<ExcludeFromCodeCoverage>]
[<EntryPoint>]
let main (args : string[]) =
    let data = getArguments args
    try
        let options = Parser.parseCalcArguments data
        printfn $"{Calculator.calculate options.arg1 options.operation options.arg2}"
    with
        | :? ArgumentException as ex -> printfn $"{ex.Message}"
    0