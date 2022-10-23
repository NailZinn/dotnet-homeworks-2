open System
open System.Net.Http
open System.Diagnostics.CodeAnalysis

[<ExcludeFromCodeCoverage>]
let convertOperation input = 
    match input with
    | "+" -> "Plus"
    | "-" -> "Minus"
    | "*" -> "Multiply"
    | "/" -> "Divide"
    | _ -> input

[<ExcludeFromCodeCoverage>]
let checkInput (input : string) =
    let query = input.Split(' ', StringSplitOptions.RemoveEmptyEntries)
    match query.Length with
    | 3 ->
        let operation = query[1] |> convertOperation
        Ok (query, operation)
    | _ -> Error $"wrong length found. Expected 3 (2 values and 1 operation) but was {query.Length}"

[<ExcludeFromCodeCoverage>]
let getResult (client : HttpClient) (uri : Uri) =
    async {
        try
            do! Async.Sleep 2000
            let! response = client.GetStringAsync(uri) |> Async.AwaitTask
            return response
        with
            :? AggregateException -> return "could not compute the expression. Try again"
    }

[<ExcludeFromCodeCoverage>]
[<EntryPoint>]
let main _ =
    use client = new HttpClient()
    while true do
        let input = Console.ReadLine() |> checkInput
        match input with
        | Ok parsedData ->
            let query = parsedData |> fst
            let operation = parsedData |> snd
            let uri = Uri($"https://localhost:51638/calculate?value1={query[0]}&operation={operation}&value2={query[2]}")
            let res = getResult client uri
            printfn $"{res |> Async.RunSynchronously}"
        | Error message -> printfn $"{message}"
    0