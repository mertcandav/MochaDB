namespace submodules.connection

open System

open MochaDB
open MochaDB.Mochaq

open utils
open terminal

/// <summary>
/// MochaQ module.
/// </summary>
type mochaq() =
  /// <summary>
  /// Process command in module.
  /// </summary>
  /// <param name="db">Database connection.</param>
  /// <param name="cmd">Command.</param>
  static member proc(db:MochaDatabase, cmd:string) : unit =
    /// <summary>
    /// Execute MochaQ command.
    /// </summary>
    /// <param name="query">Query.</param>
    let exec(query:string) : unit =
      let mq:MochaQCommand = new MochaQCommand(query)
      if mq.IsGetRunQuery()
      then printfn "GetRun"
      else if mq.IsRunQuery()
      then printfn "Run"
      else terminal.printError("MochaQ command is invalid!")

    if cmd = String.Empty then
      let mutable break:bool = false
      while break = false do
        let input:string = terminal.getInput(db.Name + "\MochaQ ", ConsoleColor.White)
        if input = String.Empty then
          break <- true
        else
          exec(input)
    else
      exec(cmd)
