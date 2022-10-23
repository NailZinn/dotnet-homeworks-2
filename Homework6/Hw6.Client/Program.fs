open System
open System.Net.Http
open System.Threading.Tasks
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
    | _ -> Error "wrong length found"

[<ExcludeFromCodeCoverage>]
let getQuery (input : Result<(string[] * string), string>) =
    match input with
    | Ok parsed -> parsed

[<ExcludeFromCodeCoverage>]
let getErrorMessage (input : Result<(string[] * string), string>) =
    match input with
    | Error message -> message

[<ExcludeFromCodeCoverage>]
let getResult (client : HttpClient) (uri : Uri) =
    Task.Delay(1000) |> ignore
    async {
        try
            let! response = client.GetStringAsync(uri) |> Async.AwaitTask
            return response
        with
            :? AggregateException -> return "could not compute the expression. Try again"
    }

[<ExcludeFromCodeCoverage>]
[<EntryPoint>]
let main _ =
    let client = new HttpClient()
    while true do
        let input = Console.ReadLine() |> checkInput
        if input = Error "wrong length found" then
            printfn $"{input |> getErrorMessage}"
        else
            let parsedData = input |> getQuery
            let query = parsedData |> fst
            let operation = parsedData |> snd
            let uri = Uri($"https://localhost:51638/calculate?value1={query[0]}&operation={operation}&value2={query[2]}")
            let res = getResult client uri
            printfn $"{res |> Async.RunSynchronously}"
    0