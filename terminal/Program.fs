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

open System
open System.Linq

open terminal
open utils
open modules

// Help content.
let _help_ = dict[
  "cd", "Change directory.";
  "help", "Show help.";
  "exit", "Exit from terminal.";
]

// Show help.
let showHelp() =
  let getWS(count:int) =
    let sb = new System.Text.StringBuilder(String.Empty)
    for _ in 1..count do
      sb.Append(" ") |> ignore
    sb.ToString()
  let maxlen = _help_.Keys.Max(fun(x) -> x.Length) + 5
  for key in _help_.Keys do
    Console.Write(key)
    Console.Write(getWS(maxlen - key.Length))
    Console.WriteLine(_help_.[key])

let processCommand(ns:string, cmd:string) =
  match ns with
  | "cd" -> cd.proc(cmd)
  | "help" -> showHelp()
  | "exit" -> exit(0x0)
  | _ -> terminal.printError("There is no such command!")

[<EntryPoint>]
let main(argv:string[]) =
  while true do
    let mutable input = terminal.getInput()
    if input <> String.Empty then
      processCommand(commandProcessor.splitNamespace(input).ToLower(),
                     commandProcessor.removeNamespace(input))
  0
