module FileEngine

// Libraries
open System
open System.IO
open System.Text

/// <summary>
/// Returns true if file is exists, returns false if not.
/// </summary>
/// <param name="path">Path of file.</param>
/// <returns>Result.</returns>
let ExistsFile(path:string) : bool =
  if File.Exists(path) = false then
    printfn "ERROR: File is not exists!"
    false
  else
    true

/// <summary>
/// Returns file content with UTF8.
/// </summary>
/// <param name="path">path of file.</param>
/// <returns>Content of file.</returns>
let GetFileContent(path:string) : string =
  if File.Exists path = false then
    String.Empty
  else
    File.ReadAllText(path, Encoding.UTF8)

/// <summary>
/// Set file content with UTF8.
/// </summary>
/// <param name="path">Path of file.</param>
/// <param name="content">Content to set.</param>
let SetFileContent(path:string, content:string) : unit =
  if File.Exists(path) = false then
    printf ""
  else
    File.WriteAllText(path, content, Encoding.UTF8)
