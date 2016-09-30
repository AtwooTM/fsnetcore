// Learn more about F# at http://fsharp.org

open Microsoft.Extensions.Configuration
open Microsoft.AspNetCore.Hosting
open System.IO

open HelloF

[<EntryPoint>]
let main (args: string array) =

    let config = 
        (new ConfigurationBuilder())
            .AddCommandLine(args)
            .AddEnvironmentVariables("ASPNETCORE_")
            .Build()

    let host =
        (new WebHostBuilder())
            .UseConfiguration(config)
            .UseKestrel()
            .UseContentRoot(Directory.GetCurrentDirectory())
            .UseIISIntegration()
            .UseStartup<Startup>()
            .Build()

    host.Run()
    
    0 // return an integer exit code
