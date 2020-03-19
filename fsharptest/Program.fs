//Libraries
open System
open DbEngine
open MhqlEngine
open MhqlStress
open FileEngine
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
            elif input.Equals("exit",StringComparison.InvariantCultureIgnoreCase) then
                exit 0
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
            elif input.Equals("getscript",StringComparison.InvariantCultureIgnoreCase) then
                printfn "\n\n------ Script Content ------\n\n"
                let content = GetFileContent(path + "/testscript.mochascript")
                Console.Write content
                printfn "\n\n------ Script Content ------\n\n"
            elif input.StartsWith("reader ",StringComparison.InvariantCultureIgnoreCase) then
                Reader(db,input.[6..])
            elif input.StartsWith("scalar ",StringComparison.InvariantCultureIgnoreCase) then
                Scalar(db,input.[6..])
            elif input.Equals("cncstate",StringComparison.InvariantCultureIgnoreCase) then
                Console.WriteLine db.ConnectionState
            elif input.Equals("mhqlstress",StringComparison.InvariantCultureIgnoreCase) then
                printfn "\n\n------ MHQL Stress Test -----\n\n"
                StartMhqlTableGetStress db
            elif input.StartsWith("mhqlstress",StringComparison.InvariantCultureIgnoreCase) then
                printfn "\n\n------ MHQL Stress Test -----\n\n"
                StartMhqlTableGetStressWithTick(db,Int32.Parse(input.Split(' ').[1]))
                printfn "\n\n------ MHQL Stress Test -----\n\n"
            elif input.Equals("runscript",StringComparison.InvariantCultureIgnoreCase) then
                let debugger = GetScriptDebugger(path + "/testscript.mochascript")
                debugger.Echo.Add(OnEcho)
                printfn "\n\n------ Script Debug Output ------\n\n"
                db.Disconnect()
                debugger.DebugRun()
                printfn "\n\n------ Script Debug Output ------\n\n"
                debugger.Dispose()
                db.Connect()
            else
                db.Query.MochaQ.SetCommand(input);
                ExecuteCommand(db)
        else
            printfn "ERROR: Empty command!"

    0
