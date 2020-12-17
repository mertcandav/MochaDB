namespace utils

open System
open System.Linq
open System.Collections.Generic

open terminal

// CLI module.
type cli() =
  // Print dictionary as table.
  static member printDictAsTable(dict:IDictionary<'a,'b>) : unit =
    // Return whitespaced string by count.
    let getWS(count:int) : string =
      let sb = new System.Text.StringBuilder(String.Empty)
      for _ in 1..count do
        sb.Append(" ") |> ignore
      sb.ToString()
    let maxlen = dict.Keys.Max(fun(x) -> x.ToString().Length) + 5
    for key in dict.Keys do
      terminal.printc(key, ConsoleColor.Yellow)
      printf "%s" (getWS(maxlen - key.Length))
      printfn "%s" dict.[key]

