namespace submodules.connection

open System

open MochaDB
open MochaDB.Mochaq

open terminal

// MochaQ module.
type mochaq() =
  // Process command in module.
  static member proc(db:MochaDatabase, cmd:string) : unit =
    // Execute MochaQ command.
    let exec(q:string) : unit =
      let mq = new MochaQCommand(q)
      if mq.IsDynamicQuery()
      then printfn "Dynamic"
      else if mq.IsGetRunQuery()
      then printfn "GetRun"
      else if mq.IsRunQuery()
      then printfn "Run"
      else terminal.printError("MochaQ command is invalid!")

    if cmd = String.Empty then
      let mutable break = false
      while break = false do
        let input = terminal.getInput(db.Name + "\MochaQ ", ConsoleColor.White)
        if input = String.Empty then
          break <- true
        else
          exec(input)
    else
      exec(cmd)
