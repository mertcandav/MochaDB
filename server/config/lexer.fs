namespace config

open System

open utilities

/// <summary>
/// Lexer of config system.
/// </summary>
[<Class>]
type lexer() =
  class
  /// <summary>
  /// This statement is comment line?
  /// </summary>
  /// <param name="statement">Statement to check.</param>
  /// <returns>true if statement is comment line, false if not.</returns>
  static member isCommentLine(statement:string ref) : bool =
    (!statement).StartsWith(tokens.INLINECOMMENT)

  /// <summary>
  /// Remove comments from statement.
  /// </summary>
  /// <param name="statement">Statement.</param>
  static member removeComments(statement:string ref) : string =
    let position:int = (!statement).IndexOf(tokens.INLINECOMMENT)
    if position <> -1 then (!statement).Substring(0, position) else !statement

  /// <summary>
  /// This statement is database statement?
  /// </summary>
  /// <param name="statement">Statement to check.</param>
  /// <returns>true if statement is database statement, false if not.</returns>
  static member isDatabaseStatement(statement:string ref) : bool =
    (!statement).StartsWith(tokens.EXCLAMATION)

  /// <summary>
  /// This statement is key statement?
  /// </summary>
  /// <param name="statement">Statement to check.</param>
  /// <returns>true if statement is key statement, false if not.</returns>
  static member isKeyStatement(statement:string ref) : bool =
    lexer.isDatabaseStatement(statement) = false

  /// <summary>
  /// This statement is skipable statement?
  /// </summary>
  /// <param name="statement">Statement to check.</param>
  /// <returns>true if statement is skipable statement, false if not.</returns>
  static member isSkipableLine(statement:string ref) : bool =
    String.IsNullOrWhiteSpace(!statement)

  /// <summary>
  /// Decompose bracket range.
  /// </summary>
  /// <param name="statement">Statement.</param>
  /// <param name="openBrace">Open bracket.</param>
  /// <param name="closeBrace">Close bracket.</param>
  /// <returns>Range of brackets.</returns>
  static member getBraceRange(statement:string ref, openBrace:char ref, closeBrace:char ref) : string =
    let mutable position:int = (!statement).IndexOf(!openBrace)
    if position = -1 then !statement
    else
      let mutable endPosition:int = (!statement).IndexOf(!closeBrace, position)
      if endPosition = -1 then cli.exitError("Bracket is opened but not closed!")
      if (position + 1) = endPosition then String.Empty
      else (!statement).Substring(position + 1, endPosition)

  /// <summary>
  /// This key a valid key?
  /// </summary>
  /// <param name="name">Name of key.</param>
  /// <returns>true if key is valid, false if not.</returns>
  static member isKey(name:string ref) : bool =
    lexer.isStringKey(name) || lexer.isIntegerKey(name)

  /// <summary>
  /// Split statement to key-value format.
  /// </summary>
  /// <param name="statement">Statement to split.</param>
  /// <returns>Splitted parts if success, null if not.</returns>
  static member split(statement:string ref) : string[] =
    let mutable parts:string[] = (!statement).Split(tokens.COLON, 2)
    if parts.Length < 2 then
      cli.printError("Key is defien but value is not define!")
      null
    else
      parts.[0] <- parts.[0].Trim().ToLower()
      parts.[1] <- parts.[1].Trim()
      parts

  /// <summary>
  /// Check key is valid value.
  /// </summary>
  /// <param name="name">Name of key.</param>
  /// <param name="value">Value of key.</param>
  /// <returns>true is valid, false if not.</returns>
  static member checkKeyValue(name:string ref, value:string ref) : bool =
    if lexer.isStringKey(name) then
      true
    else if lexer.isIntegerKey(name) then
      let mutable _value:int = 0
      Int32.TryParse(!value, _value |> ref)
    else
      false

  /// <summary>
  /// Key type is string?
  /// </summary>
  /// <param name="name">Name of key.</param>
  /// <returns>true is type of key is string, false if not.</returns>
  static member isStringKey(name:string ref) : bool =
    match !name:string with
    | "name"
    | "title"
    | "address" -> true
    | _ -> false

  /// <summary>
  /// Key type is integer?
  /// </summary>
  /// <param name="name">Name of key.</param>
  /// <returns>true is type of key is integer, false if not.</returns>
  static member isIntegerKey(name:string ref) : bool =
    match !name:string with
    | "port"
    | "listen" -> true
    | _ -> false
  end
