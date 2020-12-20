namespace submodules.connection

open System
open System.Collections

open MochaDB
open MochaDB.Mhql
open MochaDB.Mochaq
open MochaDB.Streams

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
      try
        let mq:MochaQCommand = new MochaQCommand(query)
        if mq.IsGetRunQuery() then
          let result:obj = db.Query.GetRun(mq.Command)
          match result with
          | null -> printfn "NULL"
          | :? MochaTable -> cli.printTable(result :?> MochaTable)
          | :? MochaTableResult -> cli.printTable(result :?> MochaTableResult)
          | :? MochaReader<'a> -> cli.printReader(result :?> MochaReader<'a>)
          | :? IEnumerable -> cli.printEnumerable(result :?> IEnumerable)
          | _ -> printfn "%s" (result.ToString())
        else if mq.IsRunQuery()
        then db.Query.Run(mq.Command)
        else terminal.printError("MochaQ command is invalid!")
      with
      | :? Exception as except ->
        terminal.printError(except.Message)

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
