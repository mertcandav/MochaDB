namespace modules

open System

open utilities

/// <summary>
/// Clear terminal screen.
/// </summary>
[<Class>]
type clear() =
  class
  /// <summary>
  /// Process command in module.
  /// </summary>
  /// <param name="cmd">Command.</param>
  static member proc(cmd:string) : unit =
    if cmd <> String.Empty then
      cli.printError("This module can only be used!")
    else
      Console.Clear();
  end
