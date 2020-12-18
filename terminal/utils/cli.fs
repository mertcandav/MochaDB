namespace utils

open System
open System.Linq
open System.Collections.Generic

open MochaDB

open terminal

// CLI module.
type cli() =
  // Print dictionary as table.
  static member printDictAsTable(dict:IDictionary<'a,'b>) : unit =
    let maxlen = dict.Keys.Max(fun(x:'a) -> x.ToString().Length) + 5
    for key in dict.Keys do
      terminal.printc(key, ConsoleColor.Yellow)
      printf "%s" (new String(' ', maxlen - key.Length))
      printfn "%s" dict.[key]

  // Print table.
  static member printTable(table:MochaTable) : unit =
    if table.IsEmpty() then
      printfn "Table is empty!"
    else
      let mutable tx = table.Columns.Sum(fun(x:MochaColumn) -> x.Name.Length)
      let ctx = table.Columns.Max(
        fun(x:MochaColumn) -> x.Datas.Sum(fun(y:MochaData) -> y.Data.ToString().Length))
      tx <- if tx < ctx then ctx else tx

      // Centre content.
      let alignCentre(text:string, x:int) : string =
        if String.IsNullOrEmpty(text) then
          new String(' ', x)
        else
          text.PadRight(x - (x - text.Length) / 2).PadLeft(x)

      // Print row.
      let printRow(values:MochaCollection<'a>) : unit =
        let x = (tx - values.Count) / values.Count
        printf "|"
        let mutable finalLine = "|"
        for value in values do
          let mutable value = (alignCentre(value.ToString(), x))
          value <- value.Replace("\n", " ")
          printf "%s|" value
          finalLine <- finalLine + (new String('-', value.Length)) + "|"
        printfn "\n%s" finalLine

      printRow(table.Columns)
      for row in table.Rows do
        printRow(row.Datas)
