namespace modules

open System
open System.IO

open terminal

/// <summary>
/// Change directory module.
/// </summary>
type cd() =
  /// <summary>
  /// Process command in module.
  /// </summary>
  /// <param name="cmd">Command.</param>
  static member proc(cmd:string) : unit =
    if cmd = String.Empty then
      printfn "%s" terminal.pwd
    else
      let parts:string[] = cmd.Split(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar)
      let mutable path:string = terminal.pwd
      for part:string in parts do
        if part = ".." then
          let index:int = path.LastIndexOf(Path.DirectorySeparatorChar)
          if index <> -1 then
            path <- path.Substring(0, index)
        else
          path <- Path.Combine(path, part)
      match Directory.Exists(path) with
      | true -> terminal.pwd <- path
      | false -> terminal.printError("Directory is not found in this path!")
