namespace modules

open System

open MochaDB

open terminal

/// <summary>
/// Create a new MochaDB database.
/// </summary>
type make() =
  /// <summary>
  /// Process command in module.
  /// </summary>
  /// <param name="cmd">Command.</param>
  static member proc(cmd:string) : unit =
    if cmd <> String.Empty then
      terminal.printError("This module can only be started!")
    else
      let name:string = terminal.getInput("Name: ")
      if name = String.Empty then
        terminal.printError("Name is cannot empty!")
      else
        let password:string = terminal.getInput("Password: ")
        let description:string = terminal.getInput("Description: ")
        try
          MochaDatabase.CreateMochaDB(
            System.IO.Path.Combine(terminal.pwd, name),description,password)
          terminal.printnc("Created successfully!", ConsoleColor.Green)
        with
        | :? Exception as except -> terminal.printError(except.ToString())
