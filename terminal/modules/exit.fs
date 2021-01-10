namespace modules

open System

open terminal

/// <summary>
/// Exit from terminal.
/// </summary>
[<Class>]
type exit() =
  class
  /// <summary>
  /// Process command in module.
  /// </summary>
  /// <param name="cmd">Command.</param>
  static member proc(cmd:string) : unit =
    if cmd <> String.Empty then
      terminal.printError("This module can only be used!")
    else
      Microsoft.FSharp.Core.Operators.exit(0)
  end
