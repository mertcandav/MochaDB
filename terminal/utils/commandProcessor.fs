namespace utils

open System
open System.Collections.Generic
open System.Text.RegularExpressions

open terminal

/// <summary>
/// Command processor for commands.
/// </summary>
type commandProcessor() =
  /// <summary>
  /// Get namespace of command if exists.
  /// </summary>
  /// <param name="cmd">Command.</param>
  /// <returns>Module name(namespace).</returns>
  static member splitNamespace(cmd:string) : string =
    let pos:int = cmd.IndexOf(" ")
    if pos = -1 then cmd else cmd.Substring(0, pos).TrimStart()

  /// <summary>
  /// Remove namespace from command.
  /// </summary>
  /// <param name="cmd">Command.</param>
  /// <returns>Command without namespace.</returns>
  static member removeNamespace(cmd:string) : string =
    let pos:int = cmd.IndexOf(" ")
    if pos = -1 then String.Empty else cmd.Substring(pos + 1).TrimStart()

  /// <summary>
  /// Returns arguments of command.
  /// </summary>
  /// <param name="cmd">Command.</param>
  /// <returns>Arguments.</returns>
  static member getArguments(cmd:string) : List<String> =
    let pattern:Regex = new Regex("(^|\s+)-\w+(?=($|\s+))",RegexOptions.Singleline)
    let mutable args:List<String> = new List<String>()
    for mch:Match in pattern.Matches(cmd) do
      if mch.Success then
        let arg:string = mch.Value.ToLower().Trim()
        if args.Contains(arg) then
          terminal.printError("A parameter cannot be written more than once!")
          args <- null
        else
          if args <> null
          then args.Add(arg)
    args

  /// <summary>
  /// Remove arguments from command.
  /// </summary>
  /// <param name="cmd">Command.</param>
  /// <returns>Command without arguments.</returns>
  static member removeArguments(cmd:string) : string =
    (new Regex("(^|\s+)-\w+(?=($|\s+))",RegexOptions.Singleline)).Replace(cmd, String.Empty).Trim()
