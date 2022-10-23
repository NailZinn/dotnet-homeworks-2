open System
open System.Diagnostics.CodeAnalysis
open Hw5
open Hw5.Parser
open Hw5.Calculator

[<ExcludeFromCodeCoverage>]
let getArguments (resource : string[]) : string[] =
    match resource.Length with
    | 0 ->
        let str: string = Console.ReadLine()
        let args: string[] = str.Split(' ', StringSplitOptions.RemoveEmptyEntries)
        args
    | _ -> resource
    
let showMessage message =
    match message with
    | Message.SuccessfulExecution -> "Вычисление успешно выполнено"
    | Message.WrongArgLength -> "Ожидаемая длина входного выражения ожидалась 3: 2 числа и 1 операция"
    | Message.WrongArgFormatForValue1 -> "Не удалось сконвертировать первое значение в число"
    | Message.WrongArgFormatForValue2 -> "Не удалось сконвертировать второе значение в число"
    | Message.WrongArgFormatOperation -> "Не удалось распознать разрешённую операцию. Среди разрешённых: '+', '-', '*', '/'"
    | Message.DivideByZero -> "Деление на 0!"
    
[<ExcludeFromCodeCoverage>]
[<EntryPoint>]
let main (args: string[]) =
    let data = getArguments args
    match parseCalcArguments data with
    | Ok parsedData -> printfn $"{parsedData |||> calculate}"
    | Error message -> printfn $"{message |> showMessage}"
    0