namespace modules

open System

open MochaDB

open terminal

/// <summary>
/// Create a new MochaDB database.
/// </summary>
type make() =
  /// <summary>
  /// Process commands in module.
  /// </summary>
  /// <param name="name">Arguments.</param>
  static member proc(args:string[]) : unit =
    try
      MochaDatabase.CreateMochaDB(
        System.IO.Path.Combine(terminal.pwd, args.[0]),args.[1],args.[2])
      terminal.printnc("Created successfully!", ConsoleColor.Green)
    with
    | :? Exception as except -> terminal.printError(except.Message)

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
        let mutable description:string = String.Empty
        let mutable password:string = String.Empty
        while terminal.argsIndex <= terminal.startArgs.Length - 1 do
          let arg = terminal.startArgs.[terminal.argsIndex]
          terminal.argsIndex <- terminal.argsIndex + 1
          match counter with
          | 0 -> name <- arg
          | 1 -> description <- arg
          | 2 -> password <- arg
          | _ -> ()
          counter <- counter + 1
        make.proc([| name; description; password |])
      else
        let name:string = terminal.getInput("Name: ")
        if name = String.Empty then
          terminal.printError("Name is cannot empty!")
        else
          let password:string = terminal.getInput("Password: ")
          let description:string = terminal.getInput("Description: ")
          make.proc([| name; description; password |])
