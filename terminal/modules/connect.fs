namespace modules

open System
open System.IO

open MochaDB

open utils
open terminal
open submodules.connection


// Connection module.
type connect() =
  // Process command in module.
  static member proc(cmd:string) : unit =
    if cmd <> String.Empty then
      terminal.printError("This module can only be started!")
    else
      let mutable name = terminal.getInput("Database name: ", ConsoleColor.White)
      if name = String.Empty then
        terminal.printError("Name is cannot empty!")
      else
        name <- if name.EndsWith(".mhdb") then name else name + ".mhdb"
        if File.Exists(Path.Combine(terminal.pwd, name)) = false then
          terminal.printError("Database file is not found in this name!")
        else
          let password = terminal.getInput("Password: ", ConsoleColor.White)
          let mutable logs = terminal.getInput("Logs(default is false): ",
            ConsoleColor.White).ToLower()
          match logs with
          | "" | "true" | "false" -> 
            let db = new MochaDatabase(
              "path=" + name + "; password=" + password + "; logs=" +
              if logs = String.Empty then "false" else logs)
            db.Connect()
            let mutable break = false
            while break = false do
              let input = terminal.getInput(db.Name + " ", ConsoleColor.White)
              if input <> String.Empty then
              let cmd = commandProcessor.removeNamespace(input)
              match commandProcessor.splitNamespace(input).ToLower() with
              | "mochaq" -> mochaq.proc(db, cmd)
              | "disconnect" -> break <- true
              | "help" ->
                cli.printDictAsTable(dict[
                  "mochaq", "Execute MochaQ queries.";
                  "disconnect", "Disconnect.";
                  "help", "Show help."
                ])
              | _ -> terminal.printError("There is no such command!")
            db.Disconnect()
            db.Dispose()
          | _ -> terminal.printError("Logs value is not valid!")
