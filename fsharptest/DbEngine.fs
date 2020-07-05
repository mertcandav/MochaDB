module DbEngine

//Libraries
open System
open MochaDB

//Returns database by path.
//path: Path of database.
let GetDb(path :string) : MochaDatabase =
    let db = new MochaDatabase(path,String.Empty)
    db

//Returns database by path.
//path: Path of database.
//connect: Auto connect to database.
let GetDbWithConnection(path: string,connect: bool) : MochaDatabase =
    let db = GetDb path
    if connect then db.Connect()
    db

//Returns database by provider.
//provider: Database connection provider.
let GetDbWithProvider(provider: string) : MochaDatabase =
    let db = new MochaDatabase(provider)
    db

//Execute MochaQ command.
//db: Database.
let ExecuteCommand(db: MochaDatabase) =
    try
        if db.Query.MochaQ.IsGetRunQuery() = true then
            let result = db.Query.GetRun()
            Console.WriteLine(result.ToString())
        elif db.Query.MochaQ.IsDynamicQuery() = true then
            let result = db.Query.Dynamic()
            Console.WriteLine(result.ToString())
        elif db.Query.MochaQ.IsRunQuery() = true then
            db.Query.Run()
        else
            Console.WriteLine "ERROR: Command is cannot defined!"
    with
        | :? MochaException as excep ->
            Console.WriteLine("ERROR: " + excep.Message)
