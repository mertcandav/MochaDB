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

open MochaDB

open terminal
open utils
open modules

/// <summary>
/// Show help.
/// </summary>
let showHelp() : unit =
  cli.printDictAsTable(dict[
    "cd", "Change directory.";
    "ls", "List directory content.";
    "ver", "Show version.";
    "eng", "Show engine information.";
    "make", "Create new MochaDB database.";
    "connect", "Connect to MochaDB database.";
    "clear", "Clear terminal screen.";
    "sh", "Run mhsh(MochaDB Shell Script) file.";
    "help", "Show help.";
    "exit", "Exit from terminal.";
  ])

/// <summary>
/// Process command and do task.
/// </summary>
/// <param name="ns">Name of modules(namespace).</param>
/// <param name="cmd">Commands(without module name).</param>
let processCommand(ns:string, cmd:string) : unit =
  match ns with
  | "cd" -> cd.proc(cmd)
  | "ls" -> ls.proc(cmd)
  | "ver" -> printfn "%s %s" "MochaDB Terminal --version " terminal.version
  | "eng" -> printfn "%s %s" "MochaDB Engine --version " MochaDatabase.Version
  | "make" -> make.proc(cmd)
  | "sh" -> sh.proc(cmd)
  | "connect" -> connect.proc(cmd)
  | "clear" -> Console.Clear()
  | "help" -> showHelp()
  | "exit" -> exit(0)
  | _ -> terminal.printError("There is no such command!")

/// <summary>
/// Entry point of terminal.
/// </summary>
/// <param name="argv">Arguments.</param>
/// <returns>Exit code.</returns>
[<EntryPoint>]
let main(argv:string[]) : int =
  Console.Title <- "MochaDB Terminal"
  if argv.Length > 0 then
    terminal.argMode <- true
    terminal.startArgs <- argv
    while terminal.argsIndex <= terminal.startArgs.Length - 1 do
      let arg = terminal.startArgs.[terminal.argsIndex]
      terminal.argsIndex <- terminal.argsIndex + 1
      processCommand(commandProcessor.splitNamespace(arg).ToLower(),
        commandProcessor.removeNamespace(arg))
  else
    while true do
      let mutable input:string = terminal.getInput()
      if input <> String.Empty then
        processCommand(commandProcessor.splitNamespace(input).ToLower(),
                       commandProcessor.removeNamespace(input))
  0
