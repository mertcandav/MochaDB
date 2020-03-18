//Libraries
open System
open DbEngine
open MochaDB
open MochaDB.Connection
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
            if input.Equals("clear",StringComparison.InvariantCultureIgnoreCase) then
                Console.Clear()
                printfn "MochaDB FSharp Test Console"
            elif input.Equals("connectdb",StringComparison.InvariantCultureIgnoreCase) then
                if db.ConnectionState = MochaConnectionState.Connected then
                    printfn "Connection is already open!"
                else
                    db.Connect()
                    printfn "Connected!"
            elif input.Equals("disconnectdb",StringComparison.InvariantCultureIgnoreCase) then
                if db.ConnectionState = MochaConnectionState.Disconnected then
                    printfn "Connection is already closed!"
                else
                    db.Disconnect()
                    printfn "Disconnected!"
            elif input.Equals("cncstate",StringComparison.InvariantCultureIgnoreCase) then
                Console.WriteLine db.ConnectionState
            elif input.Equals("runscript",StringComparison.InvariantCultureIgnoreCase) then
                let debugger = GetScriptDebugger(path + "/testscript.mochascript")
                debugger.Echo.Add(OnEcho)
                printfn "\n\n------ Script Output ------\n\n"
                db.Disconnect()
                debugger.DebugRun()
                printfn "\n\n------ Script Output ------\n\n"
                debugger.Dispose()
                db.Connect()
            else
                db.Query.MochaQ.SetCommand(input);
                ExecuteCommand(db)
        else
            printfn "ERROR: Empty command!"

    0
