namespace submodules.connection

open System
open System.Xml.Linq
open System.Collections

open MochaDB
open MochaDB.Mhql
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
      try
        let xml:bool = query.[0] = '$'
        let mq:MochaQCommand = new MochaQCommand(
          if xml then query.Substring(1) else query)
        if mq.IsGetRunQuery() then
          let result:obj = db.Query.GetRun(mq.Command)
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
        else if mq.IsRunQuery()
        then db.Query.Run(mq.Command)
        else terminal.printError("MochaQ command is invalid!")
      with
      | :? Exception as except ->
        terminal.printError(except.ToString())

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
