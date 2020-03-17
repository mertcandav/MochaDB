module DbEngine

open MochaDB

let GetDb(path :string) : MochaDatabase =
    let db = new MochaDatabase(path,"")
    db
