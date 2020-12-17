//
// MIT License
//
// Copyright (c) 2020 Mertcan Davulcu
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
// IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
// DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR
// OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE
// OR OTHER DEALINGS IN THE SOFTWARE.

namespace terminal

open System

// Terminal declare.
type terminal() =
  // Print message to screen with color.
  static member printc(msg:string, color:ConsoleColor) : unit =
    let realColor = Console.ForegroundColor
    Console.ForegroundColor <- color
    printf "%s" msg
    Console.ForegroundColor <- realColor

  // Print message and new line to screen with color.
  static member printnc(msg:string, color:ConsoleColor) : unit =
    terminal.printc(msg, color)
    printfn ""

  // Print error message.
  static member printError(msg:string) : unit =
    terminal.printnc(msg, ConsoleColor.Red)

  // Returns input with msg.
  static member getInput(msg:string) : string =
    printf "%s" msg
    Console.ReadLine().TrimStart()

  // Returns input with msg and color.
  static member getInput(msg:string, color:ConsoleColor) : string =
    terminal.printc(msg, color)
    Console.ReadLine().TrimStart()

  // Returns input with pwd.
  static member getInput() : string =
    terminal.getInput(terminal.pwd + " ", ConsoleColor.White)

  // Working directory.
  static member val pwd = Environment.CurrentDirectory with get, set

  // Version of terminal.
  static member val version = "0.0.1" with get
