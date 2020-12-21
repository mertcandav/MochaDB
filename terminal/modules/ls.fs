namespace modules

open System
open System.Collections.Generic
open System.IO

open terminal
open utils

/// <summary>
/// List directory.
/// </summary>
type ls() =
  /// <summary>
  /// Process command in module.
  /// </summary>
  /// <param name="cmd">Command.</param>
  static member proc(cmd:string) : unit =
    let mutable directories:bool = false
    let mutable files:bool = false
    if cmd = String.Empty then
      directories <- true
      files <- true
    else
      if cmd.Equals("@databases", StringComparison.InvariantCultureIgnoreCase) then
        for db in Directory.GetFiles(terminal.pwd) do
          if db.EndsWith(".mhdb") then
            terminal.printc("[DATABASE] ", ConsoleColor.DarkYellow)
            printfn "%s" (db.Substring(db.LastIndexOf(Path.DirectorySeparatorChar) + 1))
      else if commandProcessor.removeArguments(cmd) <> String.Empty then
        terminal.printError("This module can only be used with parameters.")
      else
        let args:List<String> = commandProcessor.getArguments(cmd)
        if args <> null then
          for arg in args do
            match arg with
            | "-f" -> files <- true
            | "-d" -> directories <- true
            | _ ->
              files <- false
              directories <- false
              terminal.printError("Argument is not recognized!")
    if directories then
      for dir in Directory.GetDirectories(terminal.pwd) do
        terminal.printc("[DIR] ", ConsoleColor.Blue)
        printfn "%s" (dir.Substring(dir.LastIndexOf(Path.DirectorySeparatorChar) + 1))
    if files then
      for file in Directory.GetFiles(terminal.pwd) do
        terminal.printc("[FILE] ", ConsoleColor.Cyan)
        printfn "%s" (file.Substring(file.LastIndexOf(Path.DirectorySeparatorChar) + 1))
