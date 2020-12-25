module DbEngine

// Libraries
open System
open MochaDB

/// <summary>
/// Returns database by path.
/// </summary>
/// <param name="path">Path of database.</param>
/// <returns>Database.</returns>
let GetDb(path:string) : MochaDatabase =
  let db:MochaDatabase = new MochaDatabase(path, String.Empty)
  db

/// <summary>
/// Returns database by path.
/// </sumamry>
/// <param name="path">Path of database.</param>
/// <param name="connect">Auto connect to database.</param>
/// <returns>Database.</returns>
let GetDbWithConnection(path:string, connect:bool) : MochaDatabase =
  let db:MochaDatabase = GetDb(path)
  if connect
  then db.Connect()
  db

/// <summary>
/// Execıte MochaQ command.
/// </summary>
/// <param name="db">Target database.</param>
let ExecuteCommand(db:MochaDatabase) : unit =
  /// <summary>
  /// Command is run type or not?
  /// </summary>
  /// <param name="command">Command to check.</param>
  /// <returns>true if command is run type, false if not.</returns>
  let CommandIsRunType(command:string) : bool =
    let command = command.ToUpperInvariant()
    String.IsNullOrEmpty(command) = false && (
      command.StartsWith("RESET") ||
      command.StartsWith("SET") ||
      command.StartsWith("ADD") ||
      command.StartsWith("CREATE") ||
      command.StartsWith("CLEAR") ||
      command.StartsWith("REMOVE") ||
      command.StartsWith("RENAME") ||
      command.StartsWith("UPDATE") ||
      command.StartsWith("RESTORE"))

  /// <summary>
  /// Command is getrun type or not?
  /// </summary>
  /// <param name="command">Command to check.</param>
  /// <returns>true if command is getrun type, false if not.</returns>
  let CommandIsGetRunType(command:string) : bool =
    let command = command.ToUpperInvariant()
    String.IsNullOrEmpty(command) = false && (
      command.StartsWith("GET") ||
      command.StartsWith("TABLECOUNT") ||
      command.StartsWith("COLUMNCOUNT") ||
      command.StartsWith("ROWCOUNT") ||
      command.StartsWith("DATACOUNT") ||
      command.StartsWith("EXISTS"))

  try
    if CommandIsGetRunType(db.Query.Command) then
      let result:obj = db.Query.GetRun()
      printfn "%s" (result.ToString())
    elif CommandIsRunType(db.Query.Command) then
      db.Query.Run()
    else
      printfn "ERROR: Command is cannot defined!"
    with
    | :? MochaException as excep ->
      printfn "ERROR: %s" excep.Message
