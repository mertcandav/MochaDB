module DbEngine

//Libraries
open System
open MochaDB
open MochaDB.MochaScript

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

//Returns script debugger by path.
//path: Path of MochaScript command file.
let GetScriptDebugger(path: string) : MochaScriptDebugger =
    let debugger = new MochaScriptDebugger(path)
    debugger
