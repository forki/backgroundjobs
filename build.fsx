#r "packages/FAKE/tools/FakeLib.dll"
open Fake

open System
open System.IO
open System.Diagnostics

let solutionFile  = "WebSite.sln"

Target "Clean" (fun _ ->
    CleanDirs ["bin"]
)

let build () = 
    !! solutionFile
    |> MSBuildRelease "" "Build"
    |> ignore

Target "Build" (fun _ ->
    build()
)

let rec runWebsite() =
    let codeFolder = FullName "code"
    use watcher = new FileSystemWatcher(codeFolder, "*.fs")
    watcher.EnableRaisingEvents <- true
    watcher.IncludeSubdirectories <- true
    watcher.Changed.Add(handleWatcherEvents)
    watcher.Created.Add(handleWatcherEvents)
    watcher.Renamed.Add(handleWatcherEvents)
    
    build()

    let app = Path.Combine("bin","Release","Website.Service.exe")
    let ok = 
        execProcess (fun info -> 
            info.FileName <- app
            info.Arguments <- "") TimeSpan.MaxValue
    if not ok then tracefn "Website shut down."
    watcher.Dispose()

and handleWatcherEvents (e:IO.FileSystemEventArgs) =
    tracefn "Rebuilding website...."

    let runningWebsites = 
        Process.GetProcessesByName("Website.Service")
        |> Seq.iter (fun p -> p.Kill())

    runWebsite()

Target "Run" (fun _ ->
    async {
        Threading.Thread.Sleep(3000)
        Process.Start(sprintf "http://localhost:%d" 8083) |> ignore }
    |> Async.Start

    runWebsite()
)

Target "Default" DoNothing

"Clean"
  ==> "Build"
  ==> "Run"
  ==> "Default"

RunTargetOrDefault "Default"