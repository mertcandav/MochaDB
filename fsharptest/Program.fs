//Libraries
open System
open DbEngine
open MochaDB
open MochaDB.MochaScript

//OnEcho event of script debugger.
let OnEcho(e: MochaScriptEchoEventArgs) =
    Console.WriteLine(e.Message)

//Entry function.
[<EntryPoint>]
let main argv =
    printfn "MochaDB FSharp Test Console"
    let path = new MochaPath __SOURCE_DIRECTORY__
    path.ParentDirectory()
    path.ParentDirectory()
    let path = path.Path + "/testdocs"
    let dbprovider = "path=" + (path + "/testdb.mochadb") + "; password=; AutoConnect=true"
    let db = GetDbWithProvider dbprovider
    while true do
        printf "Command: "
        let input = Console.ReadLine()
        if(input.Length > 0) then
            if input.Equals("clear") then
                Console.Clear()
                printfn "MochaDB FSharp Test Console"
            elif input.Equals("runscript") then
                let debugger = GetScriptDebugger(path + "/testscript.mochascript")
                debugger.Echo.Add(OnEcho)
                printfn "\n\n------ Script Output ------\n\n"
                db.Disconnect()
                debugger.DebugRun()
                printfn "\n\n------ Script Output ------\n\n"
                debugger.Dispose()
                db.Connect()
            else
                try
                    let result = db.Query.ExecuteCommand(input.ToString())
                    Console.WriteLine(result.ToString())
                with
                    | :? Exception ->
                       Console.WriteLine "ERROR: Invalid query command!"
        else
            printfn "ERROR: Empty command!"

    0
