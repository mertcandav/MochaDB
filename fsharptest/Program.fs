// Copyright 2020 The MochaDB Authors.
// Use of this source code is governed by a MIT
// license that can be found in the LICENSE file.

// Libraries
open System
open System.IO
open DbEngine
open MhqlEngine
open MhqlStress
open FileEngine
open MochaDB

/// <summary>
/// Go previous directory.
/// </summary>
/// <param name="path">Path.</param>
/// <returns>Previous path.</returns>
let parentPath(path:string) : string =
  let dex:int = path.LastIndexOf(Path.DirectorySeparatorChar)
  if dex <> -1 then path.Substring(0, dex) else path

/// <summary>
/// Entry point.
/// </summary>
/// <param name="argv">Arguments.</param>
/// <returns>Exit code.</returns>
[<EntryPoint>]
let main(argv:string[]) : int =
  printfn "MochaDB FSharp Test Console"
  let mutable path:string = __SOURCE_DIRECTORY__
  path <- parentPath(path)
  path <- parentPath(path)
  path <- path + "/tests"
  let db:MochaDatabase = new MochaDatabase(path = path + "/testdb.mhdb", autoConnect = true)
  while true do
    printf "Command: "
    let input:string = Console.ReadLine()
    if input.Length > 0 then
      if input.Equals("clear", StringComparison.InvariantCultureIgnoreCase) then
        Console.Clear()
        printfn "MochaDB FSharp Test Console"
      elif input.Equals("exit", StringComparison.InvariantCultureIgnoreCase) then
        exit(0)
      elif input.Equals("connectdb", StringComparison.InvariantCultureIgnoreCase) then
        if db.State = MochaConnectionState.Connected then
          printfn "Connection is already open!"
        else
          db.Connect()
          printfn "Connected!"
      elif input.Equals("disconnectdb", StringComparison.InvariantCultureIgnoreCase) then
        if db.State = MochaConnectionState.Disconnected then
          printfn "Connection is already closed!"
        else
          db.Disconnect()
          printfn "Disconnected!"
      elif input.Equals("getscript", StringComparison.InvariantCultureIgnoreCase) then
        printfn "\n\n------ Script Content ------\n\n"
        let content = GetFileContent(path + "/testscript.mochascript")
        printf "%s" content
        printfn "\n\n------ Script Content ------\n\n"
      elif input.StartsWith("scalar ", StringComparison.InvariantCultureIgnoreCase) then
        Scalar(db, input.[6..])
      elif input.Equals("cncstate", StringComparison.InvariantCultureIgnoreCase) then
        printfn "%A" db.State
      elif input.StartsWith("mhqlstresscmd",StringComparison.InvariantCultureIgnoreCase) then
        printfn "\n\n------ MHQL Stress Test -----\n\n"
        StartMhqlTableGetStressCmd(db, input.Split(' ').[1])
      elif input.StartsWith("mhqlstresst", StringComparison.InvariantCultureIgnoreCase) then
        printfn "\n\n------ MHQL Stress Test -----\n\n"
        let parts = input.Split(' ')
        StartMhqlTableGetStressWithTickCmd(db, Int32.Parse(parts.[1]), input.Substring(13 + parts.[1].Length))
        printfn "\n\n------ MHQL Stress Test -----\n\n"
      elif input.Equals("mhqlstress", StringComparison.InvariantCultureIgnoreCase) then
        printfn "\n\n------ MHQL Stress Test -----\n\n"
        StartMhqlTableGetStress(db)
      elif input.StartsWith("mhqlstress", StringComparison.InvariantCultureIgnoreCase) then
        printfn "\n\n------ MHQL Stress Test -----\n\n"
        StartMhqlTableGetStressWithTick(db, Int32.Parse(input.Split(' ').[1]))
        printfn "\n\n------ MHQL Stress Test -----\n\n"
      else
        db.Query.Command <- input
        ExecuteCommand(db)
    else
      printfn "ERROR: Empty command!"

  0
