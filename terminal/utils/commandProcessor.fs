namespace utils

open System
open System.Collections.Generic
open System.Text.RegularExpressions

open terminal

// Command processor for commands.
type commandProcessor() =
  // Get namespace of command if exists.
  static member splitNamespace(cmd:string) : string =
    cmd.Split(' ').[0]

  // Remove namespace from command.
  static member removeNamespace(cmd:string) : string =
    cmd.Substring(commandProcessor.splitNamespace(cmd).Length).TrimStart()

  // Returns arguments of command.
  static member getArguments(cmd:string) : List<String> =
    let pattern = new Regex("(^|\s+)-\w+(?=($|\s+))",RegexOptions.Singleline)
    let mutable args = new List<String>()
    for mch in pattern.Matches(cmd) do
      if mch.Success then
        let arg = mch.Value.ToLower().Trim()
        if args.Contains(arg) then
          terminal.printError("A parameter cannot be written more than once!")
          args <- null
        else
          if args <> null
          then args.Add(arg)
    args

  // Remove arguments from command.
  static member removeArguments(cmd:string) : string =
    (new Regex("(^|\s+)-\w+(?=($|\s+))",RegexOptions.Singleline)).Replace(cmd, String.Empty).Trim()
