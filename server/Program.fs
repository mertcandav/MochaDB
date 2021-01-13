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
open System.IO
open System.Collections.Generic

open MochaDB

open server
open modules
open utilities
open config

/// <summary>
/// Check dependicies.
/// </summary>
let check() : unit =
  cli.printnc("Checking...", ConsoleColor.Yellow)
  if File.Exists("config.mhcfg") = false then
    cli.exitError("Config file is not found!")

/// <summary>
/// Ready to use.
/// </summary>
let ready() : unit =
  cli.printnc("Reading configurations...", ConsoleColor.Yellow)
  let mutable _parser:parser = new parser()
  _parser.context <- File.ReadAllLines("config.mhcfg")
  let mutable keys:List<key> = _parser.getKeys()
  if _parser.checkKeys(keys |> ref) = false then
    cli.exitError("Config file is not contains all keys!")
  for _key:key in keys do
    match _key.name:string with
    | "name" ->
      configs.name <- _key.value
      server.pwd <- _key.value + ">"
    | "address" -> configs.address <- _key.value
    | "port" -> configs.port <- Int32.Parse(_key.value)
    | "title" -> Console.Title <- _key.value
    | "listen" -> configs.listen <- if String.IsNullOrEmpty(_key.value)
                                    then -1
                                    else Int32.Parse(_key.value)

/// <summary>
/// Show help.
/// </summary>
let showHelp() : unit =
  cli.printDictAsTable(dict[
    "ver", "Show version.";
    "eng", "Show engine information.";
    "clear", "Clear terminal screen.";
    "help", "Show help.";
    "exit", "Exit from terminal.";
  ])

/// <summary>
/// Process command and do task.
/// </summary>
/// <param name="ns">Name of modules(namespace).</param>
/// <param name="cmd">Commands(without module name).</param>
let processCommand(ns:string, cmd:string) : unit =
  match ns:string with
  | "ver" -> ver.proc(cmd)
  | "eng" -> eng.proc(cmd)
  | "clear" -> clear.proc(cmd)
  | "help" -> showHelp()
  | "exit" -> exit.proc(cmd)
  | _ -> cli.printError("There is no such command!")

/// <summary>
/// Entry point of terminal.
/// </summary>
/// <param name="argv">Arguments.</param>
/// <returns>Exit code.</returns>
[<EntryPoint>]
let main (argv:string[]) : int =
  Console.Title <- "MochaDB Server"
  check()
  ready()
  while true do
    let mutable input:string = server.getInput()
    if input <> String.Empty then
      let nspace:string = commandProcessor.splitNamespace(input |> ref).ToLower()
      let command:string = commandProcessor.removeNamespace(input |> ref)
      processCommand(nspace, command)
  0
