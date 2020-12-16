namespace modules

open System

open MochaDB

open terminal

// Create a new MochaDB database.
type make() =
  // Process command in module.
  static member proc(cmd:string) : unit =
    if cmd <> String.Empty then
      terminal.printError("This module can only be started!")
    else
      let name = terminal.getInput("Name: ")
      if name = String.Empty then
        terminal.printError("Name is cannot empty!")
      else
        let password = terminal.getInput("Password: ")
        let description = terminal.getInput("Description: ")
        try
          MochaDatabase.CreateMochaDB(
            System.IO.Path.Combine(terminal.pwd, name),description,password)
          terminal.printnc("Created successfully!", ConsoleColor.Green)
        with
        | :? Exception as except -> terminal.printError(except.ToString())
