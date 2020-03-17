open System
open DbEngine
open MochaDB

[<EntryPoint>]
let main argv =
    printfn "MochaDB FSharp Test Console"
    let path = new MochaPath __SOURCE_DIRECTORY__
    path.ParentDirectory()
    path.ParentDirectory()
    let path = path.Path + "/testdocs"
    let db = GetDb(path + "/testdb.mochadb")
    db.Connect()
    while true do
        printf "Command: "
        let input = Console.ReadLine()
        if(input > null) then
            let result = db.Query.ExecuteCommand(input.ToString())
            Console.WriteLine(result.ToString())
    0
