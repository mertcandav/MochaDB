namespace modules

open System

open MochaDB

open terminal

/// <summary>
/// Show version of engine.
/// </summary>
[<Class>]
type eng() =
  class
  /// <summary>
  /// Process command in module.
  /// </summary>
  /// <param name="cmd">Command.</param>
  static member proc(cmd:string) : unit =
    if cmd <> String.Empty then
      terminal.printError("This module can only be used!")
    else
      printfn "%s %s" "MochaDB Engine --version " MochaDatabase.Version
  end
