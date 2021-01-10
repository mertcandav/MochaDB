namespace modules

open System

open utilities
open server

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
      cli.printError("This module can only be used!")
    else
      printfn "%s %s" "MochaDB Terminal --version " server.version
  end
