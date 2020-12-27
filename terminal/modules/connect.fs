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
  static member proc(args:string[]) : unit =
    /// <summary>
    /// Process command.
    /// </summary>
    /// <param name="db">Database connection.</param>
    /// <param name="command">Command.</param>
    /// <returns>true if disconnected, false if not.</returns>
    let processCommand(db:MochaDatabase, command:string) : bool =
      let mutable break = false
      if command <> String.Empty then
        let cmd:string = commandProcessor.removeNamespace(command)
        match commandProcessor.splitNamespace(command).ToLower() with
        | "mhq" -> mhq.proc(db, cmd)
        | "mhql" -> mhql.proc(db, cmd)
        | "disconnect" -> break <- true
        | "help" ->
          cli.printDictAsTable(dict[
            "mhq", "Execute MochaQ commands.";
            "mhql", "Execute MHQL queries."
            "disconnect", "Disconnect.";
            "help", "Show help."
          ])
        | _ -> terminal.printError("There is no such command!")
      break

    try
      let db = new MochaDatabase(
        path = Path.Combine(terminal.pwd,args.[0]),
        password = args.[1],
        logs = bool.Parse(args.[2]))
      db.Connect()
      if terminal.argMode then
        while terminal.argsIndex <= terminal.startArgs.Length - 1 &&
          processCommand(db, terminal.startArgs.[terminal.argsIndex]) = false do
          terminal.argsIndex <- terminal.argsIndex + 1
      else
        let mutable break:bool = false
        while break = false do
          let input:string = terminal.getInput(db.Name + " ", ConsoleColor.White)
          break <- processCommand(db, input)
      db.Disconnect()
      db.Dispose()
    with
    | :? Exception as except ->
      terminal.printError(except.Message)

  /// <summary>
  /// Process command in module.
  /// </summary>
  /// <param name="cmd">Command.</param>
  static member proc(cmd:string) : unit =
    if cmd <> String.Empty then
      terminal.printError("This module can only be started!")
    else
      if terminal.argMode & terminal.argsIndex < terminal.startArgs.Length then
        let mutable counter:int = 0
        let mutable name:string = String.Empty
        let mutable password:string = String.Empty
        let mutable logs:string = "False"
        while terminal.argsIndex <= terminal.startArgs.Length - 1 do
          let arg = terminal.startArgs.[terminal.argsIndex]
          terminal.argsIndex <- terminal.argsIndex + 1
          match counter with
          | 0 -> name <- arg
          | 1 -> logs <- arg
          | _ -> ()
          counter <- counter + 1
        if name = String.Empty then
          terminal.printError("Name is cannot empty!")
        else
          name <- if name.EndsWith(".mhdb") then name else name + ".mhdb"
          connect.proc([| name; password; logs |])
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
              connect.proc([| name; password; if logs = String.Empty then "False" else logs |])
            | _ -> terminal.printError("Logs value is not valid!")
