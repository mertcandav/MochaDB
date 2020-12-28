namespace submodules.connection

open System
open System.Xml.Linq
open System.Collections

open MochaDB
open MochaDB.Mhql
open MochaDB.Mhq

open utils
open terminal

/// <summary>
/// MochaQ module.
/// </summary>
type mhq() =
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
    let execute(query:string) : unit =
      try
        let xml:bool = query.[0] = '$'
        let query = if xml then query.Substring(1) else query
        if utils.mhq.CommandIsGetRunType(query) then
          let result:obj = db.Query.GetRun(query)
          match result with
          | null -> printfn "NULL"
          | :? MochaTable ->
            if xml then printfn "%s" (parser.parseTableToXmlString(result :?> MochaTable))
            else cli.printTable(result :?> MochaTable)
          | :? MochaTableResult ->
            if xml then printfn "%s" (parser.parseTableToXmlString(result :?> MochaTableResult))
            else cli.printTable(result :?> MochaTableResult)
          | :? IEnumerable ->
            if xml then
              let xdoc:XDocument = XDocument.Parse("<?Collection?></Collection>")
              for element in result :?> IEnumerable do
                xdoc.Root.Add(new XElement(XName.Get("Element"),
                  if element = null then "" else element.ToString()))
              printfn "%s" (xdoc.ToString())
            else cli.printEnumerable(result :?> IEnumerable)
          | _ ->
            if xml then printfn "<Result>%s</Result>" (result.ToString())
            else printfn "%s" (result.ToString())
        else if utils.mhq.CommandIsRunType(query)
        then db.Query.Run(query)
        else terminal.printError("MochaQ command is invalid!")
      with
      | :? Exception as except ->
        terminal.printError(except.Message)

    if terminal.argMode then
      terminal.argsIndex <- terminal.argsIndex + 1
      while terminal.argsIndex < terminal.startArgs.Length do
        let command = terminal.startArgs.[terminal.argsIndex]
        terminal.argsIndex <- terminal.argsIndex + 1
        if command = String.Empty then
          ()
        else
          execute(command)
    else
      if cmd = String.Empty then
        let mutable break:bool = false
        while break = false do
          let input:string = terminal.getInput(db.Name + "\MHQ ", ConsoleColor.White)
          if input = String.Empty then
            break <- true
          else
            execute(input)
      else
        execute(cmd)
