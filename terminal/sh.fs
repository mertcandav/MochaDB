namespace modules

open System
open System.IO

open mhsh
open terminal

/// <summary>
/// Execute MochaDB Shell Script.
/// </summary>
type sh() =
  /// <summary>
  /// Process command in module.
  /// </summary>
  /// <param name="cmd">Command.</param>
  static member proc(cmd:string) : unit =
    if cmd = String.Empty then
      terminal.printError("Script file path is not defined!")
    else
      let mutable path = Path.Combine(terminal.pwd, cmd)
      path <- if path.EndsWith(".mhsh") then path else path + ".mhsh"
      match File.Exists(path) with
      | true ->
        let sh = new interpreter()
        sh.path <- path
        sh.interpret()
      | false -> terminal.printError("Shell script file is not found in this path!")