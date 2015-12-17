module WebSite.Routing

open System
open System.IO
open WebSite
open System.Web
open System.Collections
open System.Linq

// Suave
open Suave
open Suave.Web
open Suave.Http
open Suave.Types
open Suave.Http.Applicatives
open Suave.Http.Successful
open Suave.Http.Writers

// -------------------------------------------------------------------------------------------------
// Server entry-point and routing
// -------------------------------------------------------------------------------------------------


// Handles routing for the server
let app =
  choose
    [ path "/" >>= request(fun r ->  OK (sprintf "Generated reports %d" (Tasks.GetGeneratedReportCount()))) ]

let config token port = 
    { defaultConfig with
        homeFolder = Some __SOURCE_DIRECTORY__
        logger = Logging.Loggers.saneDefaultsFor Logging.LogLevel.Debug
        bindings = [ HttpBinding.mk' HTTP  "127.0.0.1" port ] 
        cancellationToken = token}