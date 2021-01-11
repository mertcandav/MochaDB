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
  /// Process value as boolean.
  /// </summary>
  /// <param name="value">Value to process.</param>
  /// <returns>Boolean value.</returns>
  member this.processBooleanValue(value:string ref) : string =
    match !value with
    | "@default"
    | "@disable" -> "False"
    | "@enable" -> "True"
    | _ -> !value

  /// <summary>
  /// Process key.
  /// </summary>
  /// <param name="statement">Statement to process.</param>
  /// <returns>key instance.</returns>
  member this.processKey(statement:string ref) : key =
    let mutable parts:string[] = lexer.split(statement)
    if parts |> isNull then
      cli.exitError("Key define is invalid! : '" + !statement + "'")
    let mutable name:string = parts.[0]
    if lexer.isKey(name |> ref) = false then
      cli.exitError("There is no such a key! : '" + name + "'")
    let mutable value:string = this.processValue(name |> ref, parts.[1] |> ref)
    if lexer.checkKeyValue(name |> ref, value |> ref) = false then
      cli.exitError("Value is invalid of key! : '" + name + "'")
    new key(name, value)

  /// <summary>
  /// Decompose and returns keys of config.
  /// </summary>
  /// <returns>Keys of config.</returns>
  member this.getKeys() : List<key> =
    let keys:List<key> = new List<key>()
    for line:string in
      this.context.Where(
      fun(element:string) -> lexer.isKeyStatement(element |> ref)) do
      let mutable line:string = lexer.removeComments(line |> ref)
      if lexer.isSkipableLine(line |> ref) = false
      then keys.Add(this.processKey(line |> ref))
    if keys.Where(fun(xkey:key) ->
       keys.Where(fun(ykey:key) -> xkey.name = ykey.name).Count() > 1).Any()
    then cli.exitError("A key is cannot declare more one times!")
    keys

  /// <summary>
  /// Process database.
  /// </summary>
  /// <param name="statement">Statement to process.</param>
  /// <returns>database instance.</returns>
  member this.processDatabase(statement:string ref) : database =
    new database()

  /// <summary>
  /// Decompose and returns database connections of config.
  /// </summary>
  /// <returns>Database connections of config.</returns>
  member this.getDatabases() : List<database> =
    let mutable connections:List<database> = new List<database>()
    for line:string in
      this.context.Where(
      fun(element:string) -> lexer.isDatabaseStatement(element |> ref)) do
      let mutable line:string = lexer.removeComments(line |> ref)
      if lexer.isSkipableLine(line |> ref) = false
      then connections.Add(this.processDatabase(line |> ref))
    if connections.Where(fun(xconnection:database) ->
       connections.Where(
        fun(yconnection:database) -> xconnection.name = yconnection.name)
        .Count() > 1).Any()
    then cli.exitError("A database connection is already exists this name!")
    connections

  /// <summary>
  /// Check keys.
  /// </summary>
  /// <param name="keys">Keys to check.</param>
  /// <returns>true is success, false if not.</returns>
  member this.checkKeys(keys:List<key> ref) : bool =
    (!keys).Where(fun(_key:key) -> _key.name = "title").Any() &&
    (!keys).Where(fun(_key:key) -> _key.name = "name").Any() &&
    (!keys).Where(fun(_key:key) -> _key.name = "address").Any() &&
    (!keys).Where(fun(_key:key) -> _key.name = "port").Any() &&
    (!keys).Where(fun(_key:key) -> _key.name = "listen").Any()

  /// <summary>
  /// Get key by name.
  /// </summary>
  /// <param name="name">Name of key.</param>
  /// <returns>key instance if found, empty key instance if not.</returns>
  member this.getKey(name:string) : key =
    let mutable result:IEnumerable<key> = this.getKeys().Where(
      fun(xkey:key) -> xkey.name = name)
    if result.Any()
    then result.First()
    else new key(String.Empty, String.Empty)

  /// <summary>
  /// Get database by name.
  /// </summary>
  /// <param name="name">Name of database.</param>
  /// <returns>database instance if found, empty database instance if not.</returns>
  member this.getDatabase(name:string) : database =
    let mutable result:IEnumerable<database> = this.getDatabases().Where(
      fun(connection:database) -> connection.name = name)
    if result.Any()
    then result.First()
    else new database()

  /// <summary>
  /// Config text.
  /// </summary>
  member val context:string[] = [| String.Empty |] with get, set
  end
