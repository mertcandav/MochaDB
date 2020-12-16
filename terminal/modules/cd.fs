namespace modules

open System
open System.IO

open terminal

// Change directory module.
type cd() =
  // Process command in module.
  static member proc(cmd:string) =
    if cmd = String.Empty then
      printfn "%s" terminal.pwd
    else
      let parts = cmd.Split(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar)
      let mutable path = terminal.pwd
      for part in parts do
        if part = ".." then
          let index = path.LastIndexOf(Path.DirectorySeparatorChar)
          if index <> -1 then
            path <- path.Substring(0, index)
        else
          path <- Path.Combine(path, part)
      match Directory.Exists(path) with
      | true -> terminal.pwd <- path
      | false -> terminal.printError("Directory is not found in this path!")
