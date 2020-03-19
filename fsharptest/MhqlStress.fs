module MhqlStress

//Libraries
open System
open System.Diagnostics
open MochaDB
open MochaDB.Connection
open MochaDB.Mhql

//Stress mhql commands.
//db: Target database.
let StartMhqlTableGetStress(db: MochaDatabase) =
    let cncstate = db.ConnectionState
    if cncstate = MochaConnectionState.Disconnected then
        db.Connect()

    while true do
        let timing = new Stopwatch()
        timing.Start()
        let command = new MochaDbCommand(db)
        let reader = command.ExecuteReader("
        USE
            TableOne,
                TableOne.Name,
                    TableOne.Gender
                        RETURN
        ")
        let readed = reader.Read()
        timing.Stop()
        Console.WriteLine("Time(Ms): " + timing.ElapsedMilliseconds.ToString())
        Console.WriteLine("Time(Tick): " + timing.ElapsedTicks.ToString())
        if readed = false then
            printfn "Reader is not read any data!"
        else
            Console.WriteLine("Reader data as ToString(): " + reader.Value.ToString())

    if cncstate = MochaConnectionState.Disconnected then
        db.Disconnect()

//Stress mhql commands with ticks.
//db: Target database.
//tick: Test count.
let StartMhqlTableGetStressWithTick(db: MochaDatabase, tick: int) =
    let cncstate = db.ConnectionState
    if cncstate = MochaConnectionState.Disconnected then
        db.Connect()

    for _ in 1..tick do
        let timing = new Stopwatch()
        timing.Start()
        let command = new MochaDbCommand(db)
        let reader = command.ExecuteReader("
        USE
            TableOne,
                TableOne.Name,
                    TableOne.Gender
                        RETURN
        ")
        let readed = reader.Read()
        timing.Stop()
        Console.WriteLine("Time(Ms): " + timing.ElapsedMilliseconds.ToString())
        Console.WriteLine("Time(Tick): " + timing.ElapsedTicks.ToString())
        if readed = false then
            printfn "Reader is not read any data!"
        else
            Console.WriteLine("Reader data as ToString(): " + reader.Value.ToString())

    if cncstate = MochaConnectionState.Disconnected then
        db.Disconnect()