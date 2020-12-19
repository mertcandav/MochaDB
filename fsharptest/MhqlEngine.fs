module MhqlEngine

// Libraries
open System
open MochaDB
open MochaDB.Mhql
open MochaDB.Streams

/// <summary>
/// Read by mhql command.
/// </summary>
/// <param name="db">Target database.</param>
/// <param name="mhql">MHQL command.</param>
let Reader(db:MochaDatabase, mhql:string) : unit =
  try
    let command:MochaDbCommand = new MochaDbCommand(mhql, db)
    let reader:MochaReader<obj> = command.ExecuteReader()
    if reader.Read() then
      printfn "%A" reader.Value
    else
      printfn "No returned data!"
  with
  | :? MochaException -> 
    printfn "ERROR: Command error!"

/// <summary>
/// Scalar by mhql command.
/// </summary>
/// <param name="db">Target database.</param>
/// <param name="mhql">MHQL command.</param>
let Scalar(db:MochaDatabase, mhql:string) : unit =
  try
    let command:MochaDbCommand = new MochaDbCommand(mhql, db)
    let value:obj = command.ExecuteScalar()
    if value = null then
      printfn "Null data"
    else
      printfn "%A" value
  with
  | :? MochaException ->
    printfn "ERROR: Command error!"
