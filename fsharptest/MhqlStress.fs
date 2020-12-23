module MhqlStress

//Libraries
open System
open System.Diagnostics
open MochaDB
open MochaDB.Mhql
open MochaDB.Streams

/// <summary>
/// Stress mhql commands.
/// </summary>
/// <param name="db">Target database.</param>
let StartMhqlTableGetStress(db:MochaDatabase) : unit =
  let cncstate:MochaConnectionState = db.State
  if cncstate = MochaConnectionState.Disconnected then
    db.Connect()

  while true do
    let timing:Stopwatch = new Stopwatch()
    timing.Start()
    let command:MochaDbCommand = new MochaDbCommand(db)
    let table:MochaTableResult = command.ExecuteScalar("
        USE
            Persons.ID
                Persons.Name
                    Persons.Password
        MUST
            0(Mertcan|Emirhan)
                AND
                    1()
        ORDERBY
            DESC
                3
        GROUPBY
                                 0
    ")
    timing.Stop()
    printfn "Time(Ms): %d" timing.ElapsedMilliseconds
    printfn "Time(Tick): %d" timing.ElapsedTicks
    if table.Any() then
      printfn "Table is empty!"
    else
      printfn "Count of table readed columns: %d" table.Columns.Length

  if cncstate = MochaConnectionState.Disconnected then
    db.Disconnect()

/// <summary>
/// Stress mhql commands with ticks.
/// </summary>
/// <param name="db">Target database.</param>
/// <param name="tick">Count of test.</param>
let StartMhqlTableGetStressWithTick(db:MochaDatabase, tick:int) : unit =
  let cncstate:MochaConnectionState = db.State
  if cncstate = MochaConnectionState.Disconnected then
    db.Connect()

  for _ in 1..tick do
    let timing:Stopwatch = new Stopwatch()
    timing.Start()
    let command:MochaDbCommand = new MochaDbCommand(db)
    let table:MochaTableResult = command.ExecuteScalar("
        USE
            Persons.ID,
                Persons.Name,
                    Persons.Password
        MUST
            0(Mertcan|Emirhan)
                AND
                    1()
        ORDERBY
            DESC
                3
        GROUPBY
                                0
    ")
    timing.Stop()
    printfn "Time(Ms): %d" timing.ElapsedMilliseconds
    printfn "Time(Tick): %d" timing.ElapsedTicks
    if table.Any() then
      printfn "Table is empty!"
    else
      printfn "Count of table readed columns: %d" table.Columns.Length

  if cncstate = MochaConnectionState.Disconnected then
    db.Disconnect()

/// <summary>
/// Stress mhql with command.
/// </summary>
/// <param name="db">Target database.</param>
/// <param name="cmd">Command.</param>
let StartMhqlTableGetStressCmd(db:MochaDatabase, cmd:string) : unit =
  let cncstate:MochaConnectionState = db.State
  if cncstate = MochaConnectionState.Disconnected then
    db.Connect()

  while true do
    let timing:Stopwatch = new Stopwatch()
    timing.Start()
    let command:MochaDbCommand = new MochaDbCommand(db)
    let table:MochaTableResult = command.ExecuteScalar(cmd)
    timing.Stop()
    printfn "Time(Ms): %d" timing.ElapsedMilliseconds
    printfn "Time(Tick): %d" timing.ElapsedTicks
    if table.Any() then
      printfn "Table is empty!"
    else
      printfn "Count of table readed columns: %d" table.Columns.Length

  if cncstate = MochaConnectionState.Disconnected then
    db.Disconnect()

/// <summary>
/// Stress mhql with command and ticks.
/// </summary>
/// <param name="db">Target database.</param>
/// <param name="tick">Count of test.</param>
/// <param name="cmd">Command.</param>
let StartMhqlTableGetStressWithTickCmd(db:MochaDatabase, tick:int, cmd:string) : unit =
  let cncstate:MochaConnectionState = db.State
  if cncstate = MochaConnectionState.Disconnected then
    db.Connect()

  for _ in 1..tick do
    let timing:Stopwatch = new Stopwatch()
    timing.Start()
    let command:MochaDbCommand = new MochaDbCommand(db)
    let table:MochaTableResult = command.ExecuteScalar(cmd)
    timing.Stop()
    printfn "Time(Ms): %d" timing.ElapsedMilliseconds
    printfn "Time(Tick): %d" timing.ElapsedTicks
    if table.Any() = false then
      printfn "Table is empty!"
    else
      printfn "Count of table readed columns: %d:" table.Columns.Length

  if cncstate = MochaConnectionState.Disconnected then
    db.Disconnect()
