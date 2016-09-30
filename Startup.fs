namespace HelloF

open System
open System.Collections.Generic
open System.Linq
open System.Threading.Tasks
open Microsoft.AspNetCore.Builder
open Microsoft.AspNetCore.Hosting
open Microsoft.AspNetCore.Http
open Microsoft.Extensions.DependencyInjection
open Microsoft.Extensions.Logging

[<AutoOpen>]
module Async =
    // Async<unit> -> Task
    let inline StartAsPlainTask (work : Async<unit>) = work |> Async.StartAsTask :> Task

    //Extension method to Run Async<unit> as RequestDelegate
    type Microsoft.AspNetCore.Builder.IApplicationBuilder with
        member this.Run(handler : HttpContext -> Async<unit>) = 
            this.Run(RequestDelegate(handler >> StartAsPlainTask))

type Startup () =
    //This method gets called by the runtime. Use this method to add services to the container.
    //For more information on how to configure your application, visit https://go.microsoft.com/fwlink/PlatformID
    member this.ConfigureServices(services: IServiceCollection) = ()

    //This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    member this.Configure(app: IApplicationBuilder, env: IHostingEnvironment, loggerFactory: ILoggerFactory) =

        loggerFactory.AddConsole() |> ignore

        if (env.IsDevelopment()) then
            app.UseDeveloperExceptionPage() |> ignore

        let myHandler (context: HttpContext) = async {
            do! context.Response.WriteAsync("Hello World from F#!") |> Async.AwaitTask
        }

        app.Run(myHandler)