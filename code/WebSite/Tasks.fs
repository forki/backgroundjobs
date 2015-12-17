module WebSite.Tasks

open WebSite.Trigger
open System
open System.IO

let refreshRate = TimeSpan.FromMilliseconds 500.

let private generatedReports = ref 0

let GetGeneratedReportCount() = !generatedReports

type TaskMessage = 
    | CreateExcelReport of string

let taskAgent = 
    Agent.Start(fun agent -> 
        let rec loop() = 
            async { 
                let! msg = agent.Receive()
                match msg with
                | CreateExcelReport fileName ->
                    printfn "Generating Excel report %d at %s." !generatedReports fileName
                    
                    // TODO: Create a real excel report

                    generatedReports := !generatedReports + 1
                return! loop ()
            }
        loop())

let trigger = createTrigger refreshRate taskAgent

let createTasks () =
    RecurringTask(TimeSpan.FromSeconds 5., CreateExcelReport "output/Report1.xlsx")
    |> trigger.Post

let stopTasks () = trigger.Post Stop