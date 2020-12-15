namespace terminal

open System

// Terminal declare
type terminal() =
  // Return input
  member this.getInput() =
    Console.ForegroundColor <- ConsoleColor.White
    Console.Write(this.pwd + " ")
    Console.ResetColor()
    let input = Console.ReadLine().TrimStart()
    input

  // Working directory.
  member val pwd = "" with get, set
