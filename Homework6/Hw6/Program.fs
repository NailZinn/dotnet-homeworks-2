module Hw6.App

open Microsoft.AspNetCore.Hosting
open Microsoft.AspNetCore.Builder
open Microsoft.Extensions.Hosting
open Microsoft.Extensions.Logging
open Microsoft.Extensions.DependencyInjection
open Giraffe
open Hw5
open Hw5.Parser
open Hw5.Calculator
open Microsoft.AspNetCore.Http
open System.Diagnostics.CodeAnalysis

let convertOperation input =
    match input with
    | "Plus" -> "+"
    | "Minus" -> "-"
    | "Multiply" -> "*"
    | "Divide" -> "/"
    | _ -> input

let parseQuery (query : IQueryCollection) = 
    [|
        query["value1"] |> string;
        query["operation"] |> string |> convertOperation;
        query["value2"] |> string;
    |]

let convertMessage (message : Result<float, Message>) (queryData : string[]) = 
    match message with
    | Ok result -> Ok (result |> string)
    | Error err ->
        match err with
        | Message.WrongArgFormatForValue1 -> Error $"Could not parse value '{queryData[0]}'"
        | Message.WrongArgFormatForValue2 -> Error $"Could not parse value '{queryData[2]}'"
        | Message.WrongArgFormatOperation -> Error $"Could not parse value '{queryData[1]}'"
        | Message.DivideByZero -> Ok "DivideByZero"

let calculate (input : Result<(float * CalculatorOperation * float), Message>) = 
    match input with
    | Ok parsedData -> Ok (parsedData |||> Calculator.calculate)
    | Error message -> Error message

let calculatorHandler: HttpHandler =
    fun next ctx ->
        let queryData =
            ctx.Request.Query
            |> parseQuery
        let result = 
            queryData
            |> parseCalcArguments
            |> calculate 
            |> convertMessage <| queryData

        match result with
        | Ok ok -> (setStatusCode 200 >=> text (ok.ToString())) next ctx
        | Error error -> (setStatusCode 400 >=> text error) next ctx

let webApp =
    choose [
        GET >=> choose [
             route "/" >=> text "Use /calculate?value1=<VAL1>&operation=<OPERATION>&value2=<VAL2>"
             route "/calculate" >=> calculatorHandler
        ]
        setStatusCode 404 >=> text "Not Found"
    ]
    
type Startup() =
    member _.ConfigureServices (services : IServiceCollection) =
        services.AddGiraffe() |> ignore

    member _.Configure (app : IApplicationBuilder) (_ : IHostEnvironment) (_ : ILoggerFactory) =
        app.UseGiraffe webApp

[<ExcludeFromCodeCoverage>]
[<EntryPoint>]
let main _ =
    Host
        .CreateDefaultBuilder()
        .ConfigureWebHostDefaults(fun whBuilder -> whBuilder.UseStartup<Startup>() |> ignore)
        .Build()
        .Run()
    0