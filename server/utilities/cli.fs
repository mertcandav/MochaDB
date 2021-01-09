namespace utilities

open System

/// <summary>
/// CLI(Command Line Interface) utilities.
/// </summary>
[<Class>]
type cli() =
  class
  /// <summary>
  /// Print message to screen with color.
  /// </summary>
  /// <param name="msg">Message.</param>
  /// <param name="color">Color of message.</param>
  static member printc(msg:string, color:ConsoleColor) : unit =
    let realColor:ConsoleColor = Console.ForegroundColor
    Console.ForegroundColor <- color
    printf "%s" msg
    Console.ForegroundColor <- realColor

  /// <summary>
  /// Print message and new line to screen with color.
  /// </summary>
  /// <param name="msg">Message.</param>
  /// <param name="color">Color of message.</param>
  static member printnc(msg:string, color:ConsoleColor) : unit =
    cli.printc(msg, color)
    printfn ""

  /// <summary>
  /// Print error message.
  /// </summary>
  /// <param name="msg">Message.</param>
  static member printError(msg:string) : unit =
    cli.printnc(msg, ConsoleColor.Red)
  end
