module MhqlEngine

//Libraries
open System
open MochaDB
open MochaDB.Mhql

//Read by mhql command.
//db: Target database.
//mhql: MHQL command.
let Reader(db: MochaDatabase, mhql: string) =
    try
        let command = new MochaDbCommand(mhql,db)
        let reader = command.ExecuteReader();
        if reader.Read() then
            Console.WriteLine reader.Value
        else
            printfn "No returned data!"
    with
        | :? MochaException -> 
            printfn "ERROR: Command error!"


//Scalar by mhql command.
//db: Target database.
//mhql: MHQL command.
let Scalar(db: MochaDatabase, mhql: string) =
    try
        let command = new MochaDbCommand(mhql,db)
        let value = command.ExecuteScalar();
        if value = null then
            printfn "Null data"
        else
            Console.WriteLine value
    with
        | :? MochaException ->
            printfn "ERROR: Command error!"
