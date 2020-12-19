module DbEngine

// Libraries
open System
open MochaDB
open MochaDB.Querying

/// <summary>
/// Returns database by path.
/// </summary>
/// <param name="path">Path of database.</param>
/// <returns>Database.</returns>
let GetDb(path:string) : MochaDatabase =
  let db:MochaDatabase = new MochaDatabase(path, String.Empty)
  db

/// <summary>
/// Returns database by path.
/// </sumamry>
/// <param name="path">Path of database.</param>
/// <param name="connect">Auto connect to database.</param>
/// <returns>Database.</returns>
let GetDbWithConnection(path:string, connect:bool) : MochaDatabase =
  let db:MochaDatabase = GetDb(path)
  if connect
  then db.Connect()
  db

/// <summary>
/// Returns database by provider.
/// </summary>
/// <param name="provider">Database connection provider.</param>
/// <returns>Database.</returns>
let GetDbWithProvider(provider:string) : MochaDatabase =
  let db:MochaDatabase = new MochaDatabase(provider)
  db

/// <summary>
/// Execıte MochaQ command.
/// </summary>
/// <param name="db">Target database.</param>
let ExecuteCommand(db:MochaDatabase) : unit =
  try
    if db.Query.MochaQ.IsGetRunQuery() then
      let result:obj = db.Query.GetRun()
      printfn "%s" (result.ToString())
    elif db.Query.MochaQ.IsRunQuery() then
      db.Query.Run()
    else
      printfn "ERROR: Command is cannot defined!"
    with
    | :? MochaException as excep ->
      printfn "ERROR: %s" excep.Message
