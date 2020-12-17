﻿module MhqlStress

//Libraries
open System
open System.Diagnostics
open MochaDB
open MochaDB.Connection
open MochaDB.Mhql

//Stress mhql commands.
//db: Target database.
let StartMhqlTableGetStress(db:MochaDatabase) : unit =
  let cncstate = db.State
  if cncstate = MochaConnectionState.Disconnected then
    db.Connect()

  while true do
    let timing = new Stopwatch()
    timing.Start()
    let command = new MochaDbCommand(db)
    let reader = command.ExecuteReader("
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
    let readed = reader.Read()
    timing.Stop()
    printfn "Time(Ms): %d" timing.ElapsedMilliseconds
    printfn "Time(Tick): %d" timing.ElapsedTicks
    if readed = false then
      printfn "Reader is not read any data!"
    else
      printfn "Reader data as ToString(): %s" (reader.Value.ToString())

  if cncstate = MochaConnectionState.Disconnected then
    db.Disconnect()

//Stress mhql commands with ticks.
//db: Target database.
//tick: Test count.
let StartMhqlTableGetStressWithTick(db:MochaDatabase, tick:int) : unit =
  let cncstate = db.State
  if cncstate = MochaConnectionState.Disconnected then
    db.Connect()

  for _ in 1..tick do
    let timing = new Stopwatch()
    timing.Start()
    let command = new MochaDbCommand(db)
    let reader = command.ExecuteReader("
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
    let readed = reader.Read()
    timing.Stop()
    printfn "Time(Ms): %d" timing.ElapsedMilliseconds
    printfn "Time(Tick): %d" timing.ElapsedTicks
    if readed = false then
      printfn "Reader is not read any data!"
    else
      printfn "Reader data as ToString(): %s" (reader.Value.ToString())

  if cncstate = MochaConnectionState.Disconnected then
    db.Disconnect()

//Stress mhql with command.
//db: Target database.
//cmd: Command.
let StartMhqlTableGetStressCmd(db:MochaDatabase, cmd:string) : unit =
  let cncstate = db.State
  if cncstate = MochaConnectionState.Disconnected then
    db.Connect()

  while true do
    let timing = new Stopwatch()
    timing.Start()
    let command = new MochaDbCommand(db)
    let reader = command.ExecuteReader(cmd)
    let readed = reader.Read()
    timing.Stop()
    printfn "Time(Ms): %d" timing.ElapsedMilliseconds
    printfn "Time(Tick): %d" timing.ElapsedTicks
    if readed = false then
      printfn "Reader is not read any data!"
    else
      printfn "Reader data as ToString(): %s" (reader.Value.ToString())

  if cncstate = MochaConnectionState.Disconnected then
    db.Disconnect()

//Stress mhql with command and ticks.
//db: Target database.
//tick: Test count.
//cmd: Command.
let StartMhqlTableGetStressWithTickCmd(db:MochaDatabase, tick:int, cmd:string) : unit =
  let cncstate = db.State
  if cncstate = MochaConnectionState.Disconnected then
    db.Connect()

  for _ in 1..tick do
    let timing = new Stopwatch()
    timing.Start()
    let command = new MochaDbCommand(db)
    let reader = command.ExecuteReader(cmd)
    let readed = reader.Read()
    timing.Stop()
    printfn "Time(Ms): %d" timing.ElapsedMilliseconds
    printfn "Time(Tick): %d" timing.ElapsedTicks
    if readed = false then
      printfn "Reader is not read any data!"
    else
      printfn "Reader data as ToString(): %s" (reader.Value.ToString())

  if cncstate = MochaConnectionState.Disconnected then
    db.Disconnect()
