namespace modules

open System
open System.IO

open MochaDB

open utils
open terminal
open submodules.connection

/// <summary>
/// Connection module.
/// </summary>
type connect() =
  /// <summary>
  /// Process command in module.
  /// </summary>
  /// <param name="cmd">Command.</param>
  static member proc(cmd:string) : unit =
    if cmd <> String.Empty then
      terminal.printError("This module can only be started!")
    else
      let mutable name:string = terminal.getInput("Database name: ", ConsoleColor.White)
      if name = String.Empty then
        terminal.printError("Name is cannot empty!")
      else
        name <- if name.EndsWith(".mhdb") then name else name + ".mhdb"
        if File.Exists(Path.Combine(terminal.pwd, name)) = false then
          terminal.printError("Database file is not found in this name!")
        else
          let password:string = terminal.getInput("Password: ", ConsoleColor.White)
          let mutable logs:string = (terminal.getInput("Logs(default is false): ",
            ConsoleColor.White).ToLower())
          match logs with
          | "" | "true" | "false" -> 
            try
              let db = new MochaDatabase(
                "path=" + Path.Combine(terminal.pwd,name) + "; password=" + password + "; logs=" +
                if logs = String.Empty then "false" else logs)
              db.Connect()
              let mutable break:bool = false
              while break = false do
                let input:string = terminal.getInput(db.Name + " ", ConsoleColor.White)
                if input <> String.Empty then
                let cmd:string = commandProcessor.removeNamespace(input)
                match commandProcessor.splitNamespace(input).ToLower() with
                | "mochaq" -> mochaq.proc(db, cmd)
                | "mhql" -> mhql.proc(db, cmd)
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
            with
            | :? Exception as except ->
              terminal.printError(except.ToString())
          | _ -> terminal.printError("Logs value is not valid!")
