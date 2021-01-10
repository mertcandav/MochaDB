namespace config

open System
open System.Collections.Generic
open System.Linq

open utilities

/// <summary>
/// Config parser.
/// </summary>
[<Class>]
type parser() =
  class
  /// <summary>
  /// Process valeu with tags.
  /// </summary>
  /// <param name="name">Name of key.</param>
  /// <param name="value">Value of key.</param>
  /// <returns>Value.</returns>
  member this.processValue(name:string ref, value:string ref) : string =
    match !name:string with
    | "name" ->
      match !value:string with
      | ""
      | "@default" -> "MHServer"
      | _ -> !value
    | "port" ->
      match !value:string with
      | "@default" -> "8085"
      | _ -> !value
    | "address" ->
      match !value:string with
      | "@default"
      | "@localhost" -> "127.0.0.1"
      | _ -> !value
    | "listen" ->
      match !value:string with
      | "@default" -> "10"
      | "@max" -> Int32.MaxValue.ToString()
      | _ -> !value
    | "title" ->
      match !value:string with
      | "@default" -> Console.Title
      | _ -> !value

  /// <summary>
  /// Process key.
  /// </summary>
  /// <param name="statement">Statement to process.</param>
  /// <returns>key if success, empty key if not.</returns>
  member this.processKey(statement:string ref) : key =
    let mutable parts:string[] = lexer.split(statement)
    if parts <> null then
      let mutable name:string = parts.[0]
      if lexer.isKey(name |> ref) <> true then
        cli.printError("There is no such a key! : '" + name + "'")
        new key(String.Empty, String.Empty)
      else
        let mutable value:string = parts.[1]
        if lexer.checkKeyValue(name |> ref, value |> ref) then
          value <- this.processValue(name |> ref, value |> ref)
          new key(name, value)
        else
          new key(String.Empty, String.Empty)
    else
      new key(String.Empty, String.Empty)

  /// <summary>
  /// Decompose and returns keys of config.
  /// </summary>
  /// <returns>Keys of config.</returns>
  member this.getKeys() : List<key> =
    let keys:List<key> = new List<key>()
    for line:string in this.context do
      let mutable line:string = line
      line <- lexer.removeComments(line |> ref)
      if lexer.isSkipableLine(line |> ref) <> true then
        let mutable _key:key = this.processKey(line |> ref)
        if String.IsNullOrEmpty(_key.name) <> true then
          keys.Add(_key)
    if keys.Where(fun(xkey:key) ->
       keys.Where(fun(ykey:key) -> xkey.name = ykey.name).Count() > 1).Any() then
      cli.printError("A key is cannot declare more one times!")
      null
    else
      keys

  /// <summary>
  /// Get key by name.
  /// </summary>
  /// <param name="name">Name of key.</param>
  /// <returns>key if found, empty key if not.</returns>
  member this.getKey(name:string) : key =
    let mutable result:IEnumerable<key> = this.getKeys().Where(fun(x:key) -> x.name = name)
    if result.Any() then
      result.First()
    else
      new key(String.Empty, String.Empty)

  /// <summary>
  /// Config text.
  /// </summary>
  member val context:string[] = [| String.Empty |] with get, set
  end
