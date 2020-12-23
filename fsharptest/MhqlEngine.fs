module MhqlEngine

// Libraries
open System
open MochaDB
open MochaDB.Mhql

/// <summary>
/// Scalar by mhql command.
/// </summary>
/// <param name="db">Target database.</param>
/// <param name="mhql">MHQL command.</param>
let Scalar(db:MochaDatabase, mhql:string) : unit =
  try
    let command:MochaDbCommand = new MochaDbCommand(mhql, db)
    let value:MochaTableResult = command.ExecuteScalar()
    if value = null then
      printfn "Null data"
    else
      printfn "%A" value
  with
  | :? MochaException ->
    printfn "ERROR: Command error!"
