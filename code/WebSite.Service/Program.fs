open Suave
open Suave.Http.Successful
open Suave.Web
open Suave.Http
open Suave.Http.Applicatives
open Suave.Http.Successful
open Topshelf
open System
open System.Threading
open Suave.Types

[<EntryPoint>]
let main argv =
    let cancellationTokenSource = ref None

    let start hc = 
        let cts = new CancellationTokenSource()
        let token = cts.Token
        let port = 8083  // make this a config
        let config = WebSite.Routing.config token port

        startWebServerAsync config WebSite.Routing.app
        |> snd
        |> Async.StartAsTask 
        |> ignore

        cancellationTokenSource := Some cts

        WebSite.Tasks.createTasks()

        true

    let stop hc = 
        match !cancellationTokenSource with
        | Some cts -> cts.Cancel()
        | None -> ()

        WebSite.Tasks.stopTasks()

        true

    Service.Default 
    |> display_name "Website"
    |> instance_name "Website"
    |> with_start start
    |> with_stop stop
    |> run