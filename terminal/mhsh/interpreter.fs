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

namespace mhsh

open System
open System.Collections.Generic
open System.Linq
open System.IO

open MochaDB

open modules
open mhsh.parser
open mhsh.objects
open utils
open terminal

/// <summary>
/// Interpreter for mhsh(MochaDB Shell Script).
/// </summary>
[<Class>]
type interpreter() =
  class
  /// <summary>
  /// Show help.
  /// </summary>
  static member showHelp() : unit =
    cli.printDictAsTable(dict[
      "cd", "Change directory.";
      "ls", "List directory content.";
      "ver", "Show version.";
      "eng", "Show engine information.";
      "make", "Create new MochaDB database.";
      "connect", "Connect to MochaDB database.";
      "clear", "Clear terminal screen.";
      //"sh", "Run mhsh(MochaDB Shell Script) file.";
      "help", "Show help.";
      "exit", "Exit from terminal.";
    ])

  /// <summary>
  /// Process command in special interpreter commands.
  /// </summary>
  /// <param name="ns">Name of modules(namespace).</param>
  /// <param name="cmd">Commands(without module name).</param>
  /// <returns>true if namespace is internal command, false if not.</returns>
  member this.internalProcessCommand(ns:string, cmd:string) : bool =
    match ns with
    | "pause" ->
      printf "%s" (if String.IsNullOrWhiteSpace(cmd) then "Press any key to continue..." else cmd)
      Console.ReadKey() |> ignore
      printfn ""
      true
    | "echo" ->
      printfn "%s" cmd
      true
    | _ -> false

  /// <summary>
  /// Process command and do task.
  /// </summary>
  /// <param name="ns">Name of modules(namespace).</param>
  /// <param name="cmd">Commands(without module name).</param>
  static member processCommand(ns:string, cmd:string) : unit =
    match ns with
    | "cd" -> cd.proc(cmd)
    | "ls" -> ls.proc(cmd)
    | "ver" -> printfn "%s %s" "MochaDB Terminal --version " terminal.version
    | "eng" -> printfn "%s %s" "MochaDB Engine --version " MochaDatabase.Version
    | "make" -> make.proc(cmd)
    //| "sh" ->
    //  if cmd = String.Empty then
    //    terminal.printError("Script file path is not defined!")
    //  else
    //    let mutable path:string = Path.Combine(terminal.pwd, cmd)
    //    path <- if path.EndsWith(".mhsh") then path else path + ".mhsh"
    //    match File.Exists(path) with
    //    | true ->
    //      let sh:interpreter = new interpreter()
    //      sh.path <- path
    //      sh.interpret()
    //    | false -> terminal.printError("Shell script file is not found in this path!")
    | "connect" -> connect.proc(cmd)
    | "clear" -> Console.Clear()
    | "help" -> interpreter.showHelp()
    | "exit" -> exit(0)
    | _ -> terminal.printError("There is no such command!")

  /// <summary>
  /// Interpret script codes.
  /// </summary>
  member this.interpret() : unit =
    terminal.startArgs <- File.ReadAllLines(this.path)
    terminal.argMode <- true
    for index in 0..terminal.startArgs.Length - 1 do
      terminal.startArgs.[index] <- _LEXER_.removeComments(terminal.startArgs.[index])
    //terminal.startArgs <-
    //  terminal.startArgs.Where(fun(x:string) -> _LEXER_.isSkippableStatement(x) = false)
    //    .ToArray()
    while _TOKENS_.FAILED <> true && terminal.argsIndex < terminal.startArgs.Length do
      let line:string = terminal.startArgs.[terminal.argsIndex]
      terminal.argsIndex <- terminal.argsIndex + 1
      let nspace:string = commandProcessor.splitNamespace(line).ToLower()
      let command:string = commandProcessor.removeNamespace(line)
      if this.internalProcessCommand(nspace, command) = false
      then interpreter.processCommand(nspace, command)
    terminal.argMode <- false
    _TOKENS_.FAILED <- false

  /// <summary>
  /// Path of MochaDB Shell Script file.
  /// </summary>
  member val path:string = String.Empty with get, set

  /// <summary>
  /// Variables.
  /// </summary>
  member val variables:List<_VARIABLE_> = new List<_VARIABLE_>() with get, set
  end
