namespace modules

open System

open terminal

/// <summary>
/// Show version.
/// </summary>
[<Class>]
type ver() =
  class
  /// <summary>
  /// Process command in module.
  /// </summary>
  /// <param name="cmd">Command.</param>
  static member proc(cmd:string) : unit =
    if cmd <> String.Empty then
      terminal.printError("This module can only be used!")
    else
      printfn "%s %s" "MochaDB Terminal --version " terminal.version
  end
