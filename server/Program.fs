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

open server
open utilities
open config
open terminal

/// <summary>
/// Check dependicies.
/// </summary>
let check() : unit =
  if File.Exists("config.mhcfg") <> true then
    cli.exitError("Config file is not found!")

/// <summary>
/// Ready to use.
/// </summary>
let ready() : unit =
  let mutable _parser:parser = new parser()
  _parser.context <- File.ReadAllLines("config.mhcfg")
  let mutable keys:List<key> = _parser.getKeys()
  for _key:key in keys do
    match _key.name:string with
    | "name" ->
      configs.name <- _key.value
      terminal.pwd <- _key.value
    | "address" -> configs.address <- _key.value
    | "port" -> configs.port <- Int32.Parse(_key.value)
    | "listen" -> configs.listen <- if String.IsNullOrEmpty(_key.value)
                                    then -1
                                    else Int32.Parse(_key.value)

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
    let mutable input:string = terminal.getInput()
    if input <> String.Empty then
      printfn "%s" input
  0
