module FileEngine

//Libraries
open System
open System.IO
open System.Text

//Returns true if file is exists, returns false if not.
//path: Path of file.
let ExistsFile(path:string) : bool =
  if File.Exists(path) = false then
    printfn "ERROR: File is not exists!"
    false
  else
    true

//Returns file content with UTF8.
//path: Path of file.
let GetFileContent(path:string) : string =
  if File.Exists path = false then
    String.Empty
  else
    File.ReadAllText(path, Encoding.UTF8)

//Set file content with UTF8.
//path: Path of file.
//content: Content to set.
let SetFileContent(path:string, content:string) : unit =
  if File.Exists(path) = false then
    printf ""
  else
    File.WriteAllText(path, content, Encoding.UTF8)
