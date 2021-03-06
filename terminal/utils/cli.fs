﻿namespace utils

open System
open System.Linq
open System.Collections
open System.Collections.Generic

open MochaDB
open MochaDB.Mhql

open terminal

/// <summary>
/// CLI module.
/// </summary>
[<Class>]
type cli() =
  class
  /// <summary>
  /// Print dictionary as table.
  /// </summary>
  /// <param name="dict">Dictionary to print.<param>
  static member printDictAsTable(dict:IDictionary<'a,'b>) : unit =
    let maxlen:int = dict.Keys.Max(fun(x:'a) -> x.ToString().Length) + 5
    for key:'a in dict.Keys do
      terminal.printc(key, ConsoleColor.Yellow)
      printf "%s" (new String(' ', maxlen - key.Length))
      printfn "%s" dict.[key]

  /// <summary>
  /// Print MochaTable.
  /// </summary>
  /// <param name="table">MochaTable to print.</param>
  static member printTable(table:MochaTable) : unit =
    if table.Any() then
      printfn "Table is empty!"
    else
      let mutable tableWidth:int = table.Columns.Sum(fun(x:MochaColumn) -> x.Name.Length)
      let ctx:int = table.Columns.Max(
        fun(x:MochaColumn) -> x.Datas.Sum(fun(y:MochaData) -> y.Data.ToString().Length))
      tableWidth <- if tableWidth < ctx then ctx else tableWidth
      tableWidth <- tableWidth + 10

      /// <summary>
      /// Centre content.
      /// </summary>
      /// <param name="text">Text to centre.</param>
      /// <param name="width">Width.</param>
      /// <returns>Centred text.</returns>
      let alignCentre(text:string, width:int) : string =
        if String.IsNullOrEmpty(text) then
          new String(' ', width)
        else
          text.PadRight(width - (width - text.Length) / 2).PadLeft(width)

      /// <summary>
      /// Print row.
      /// </summary>
      /// <param name="values">Values of row.</param>
      let printRow(values:MochaCollection<'a>) : unit =
        let x:int = (tableWidth - values.Count) / values.Count
        printf "|"
        let mutable finalLine:string = "|"
        for value:'a in values do
          let mutable value:string = (alignCentre(value.ToString(), x))
          value <- value.Replace("\n", " ")
          printf "%s|" value
          finalLine <- finalLine + (new String('-', x)) + "|"
        printfn "\n%s" finalLine

      printRow(table.Columns)
      for row:MochaRow in table.Rows do
        printRow(row.Datas)

  /// <summary>
  /// Print MochaTableResult.
  /// </summary>
  /// <param name="table">MochaTableResult to print.</param>
  static member printTable(table:MochaTableResult) : unit =
    if table.Any() then
      printfn "Table is empty!"
    else
      let mutable tableWidth:int = table.Columns.Sum(fun(x:MochaColumn) -> x.MHQLAsText.Length)
      let ctx:int = table.Columns.Max(
        fun(x:MochaColumn) -> x.Datas.Sum(fun(y:MochaData) -> y.Data.ToString().Length))
      tableWidth <- if tableWidth < ctx then ctx else tableWidth
      tableWidth <- tableWidth + 10

      /// <summary>
      /// Centre content.
      /// </summary>
      /// <param name="text">Text to centre.</param>
      /// <param name="width">Width.</param>
      /// <returns>Centred text.</returns>
      let alignCentre(text:string, x:int) : string =
        if String.IsNullOrEmpty(text) then
          new String(' ', x)
        else
          text.PadRight(x - (x - text.Length) / 2).PadLeft(x)

      /// <summary>
      /// Print row.
      /// </summary>
      /// <param name="values">Values of row.</param>
      /// <param name="count">Count of elements.</param>
      let printRow(values:IEnumerable<'a>, count:int) : unit =
        let x:int = (tableWidth - count) / count
        printf "|"
        let mutable finalLine:string = "|"
        for value:'a in values do
          let mutable value:string = (alignCentre(value.ToString(), x))
          value <- value.Replace("\n", " ")
          printf "%s|" value
          finalLine <- finalLine + (new String('-', x)) + "|"
        printfn "\n%s" finalLine

      printRow(table.Columns.Select(
        fun(x:MochaColumn) -> new MochaData(MochaDataType.String, x.MHQLAsText)),
        table.Columns.Length)
      for row:MochaRow in table.Rows do
        printRow(row.Datas, row.Datas.Count)

  /// <summary>
  /// Print elements of IEnumerable.
  /// </summary>
  /// <param name="enumrable">IEnumerable to print.</param>
  static member printEnumerable(enumerable:IEnumerable) : unit =
    for element:Object in enumerable do
      printfn "%s" (element.ToString())
  end
