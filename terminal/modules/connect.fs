namespace modules

open System
open System.IO

open MochaDB

open terminal

// Connection module.
type connect() =
  // Process command in module.
  static member proc(cmd:string) : unit =
    if cmd <> String.Empty then
      terminal.printError("This module can only be started!")
    else
      let mutable name = terminal.getInput("Database name: ")
      if name = String.Empty then
        terminal.printError("Name is cannot empty!")
      else
        name <- if name.EndsWith(".mhdb") then name else name + ".mhdb"
        if File.Exists(Path.Combine(terminal.pwd, name)) = false then
          terminal.printError("Database file is not found in this name!")
        else
          let password = terminal.getInput("Password: ")
          let mutable logs = terminal.getInput("Logs(default is false): ").ToLower()
          match logs with
          | "" | "true" | "false" -> 
            let db = new MochaDatabase(
              "path=" + name + "; password=" + password + "; logs=" +
              if logs = String.Empty then "false" else logs)
            db.Connect()
            let mutable break = false
            while break = false do
              let input = terminal.getInput(db.Name + " ")
              printf ""
            db.Disconnect()
            db.Dispose()
          | _ -> terminal.printError("Logs value is not valid!")