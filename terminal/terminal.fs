namespace terminal

open System

// Terminal declare
type terminal() =
  // Print error message
  static member printError(msg:string) =
    Console.ForegroundColor <- ConsoleColor.Red
    Console.WriteLine(msg)
    Console.ResetColor()

  // Return input
  static member getInput() =
    Console.ForegroundColor <- ConsoleColor.White
    Console.Write(terminal.pwd + " ")
    Console.ResetColor()
    let input = Console.ReadLine().TrimStart()
    input

  // Working directory.
  static member val pwd = Environment.CurrentDirectory with get, set
