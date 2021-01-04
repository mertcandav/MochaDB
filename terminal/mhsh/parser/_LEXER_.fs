namespace mhsh.parser

open System
open System.Collections.Generic

open mhsh.objects
open terminal

/// <summary>
/// Lexer.
/// </summary>
type _LEXER_() =
  /// <summary>
  /// Failed process state.
  /// </summary>
  static member val failProcess:string = "_ERROR_" with get

  /// <summary>
  /// Check statement is skippable statement.
  /// </summary>
  /// <param name="statement">Statement.</param>
  /// <returns>true if statement is skippable, false if not.</returns>
  static member isSkippableStatement(statement:string) : bool =
    String.IsNullOrEmpty(statement)
  
  /// <summary>
  /// Check statement is literal statement.
  /// </summary>
  /// <param name="statement">Statement.</param>
  /// <returns>true if statement is literal, false if not.</returns>
  static member isLiteralStatement(statement:string) : bool =
    statement.[0] = _TOKENS_.VBAR.[0]

  /// <summary>
  /// Find limit of variable statement.
  /// </summary>
  /// <param name="statement">Statement.</param>
  /// <returns>Index of limit if found, negative one if not.</returns>
  static member findVariableLimit(statement:string) : int =
    let mutable position:int = statement.IndexOf(" ");
    position <- if position = -1 then statement.IndexOf(_TOKENS_.ESCAPESEQUENCE) else position
    position <- if position = -1 then statement.IndexOf(_TOKENS_.LPAR) else position
    position <- if position = -1 then statement.IndexOf(_TOKENS_.RPAR) else position
    position <- if position = -1 then statement.IndexOf(_TOKENS_.DOLLAR) else position
    position <- if position = -1 then statement.IndexOf(_TOKENS_.SLASH) else position
    position <- if position = -1 then statement.IndexOf(_TOKENS_.COMMA) else position
    position <- if position = -1 then statement.IndexOf(_TOKENS_.AT) else position
    position <- if position = -1 then statement.IndexOf(_TOKENS_.AMPER) else position
    position <- if position = -1 then statement.IndexOf(_TOKENS_.SEMICOLON) else position
    position <- if position = -1 then statement.IndexOf(_TOKENS_.TILDE) else position
    position <- if position = -1 then statement.IndexOf(_TOKENS_.STAR) else position
    position <- if position = -1 then statement.IndexOf(_TOKENS_.PERCENT) else position
    position <- if position = -1 then statement.IndexOf(_TOKENS_.EQUAL) else position
    position <- if position = -1 then statement.IndexOf(_TOKENS_.PLUS) else position
    position <- if position = -1 then statement.IndexOf(_TOKENS_.MINUS) else position
    position <- if position = -1 then statement.IndexOf(_TOKENS_.VBAR) else position
    position <- if position = -1 then statement.IndexOf(_TOKENS_.GREATER) else position
    position <- if position = -1 then statement.IndexOf(_TOKENS_.LESS) else position
    position <- if position = -1 then statement.IndexOf(_TOKENS_.COLON) else position
    position <- if position = -1 then statement.IndexOf(_TOKENS_.CIRCUMFLEX) else position
    position

  /// <summary>
  /// Remove comments from statement.
  /// </summary>
  /// <param name="statement">Statement.</param>
  /// <returns>Statement without comments.</returns>
  static member removeComments(statement:string) : string =
    let mutable index:int = -1
    let mutable rstatement:string = statement
    while index < statement.Length do
      index <- index + 1
      let ch:string = statement.Substring(0, 1)
      if _LEXER_.processSequence(statement.Substring(index)) <> _LEXER_.failProcess then
        index <- index + 1
      else
        if ch = _TOKENS_.INLINECOMMENT then
          if index = 0 then
            rstatement <- String.Empty
          else
            rstatement <- statement.Substring(0, index)
          index <- statement.Length
    rstatement

  /// <summary>
  /// Decompose range.
  /// </summary>
  /// <param name="openc">Open char.</param>
  /// <param name="closec">Close char.</param>
  /// <param name="statement">Statement.</param>
  /// <returns>Statement without range.</returns>
  static member lexBraceRange(openc:char, closec:char, statement:string) : string =
   let mutable count:int = 0
   let mutable index:int = -1
   let mutable statement:string = statement
   while index < statement.Length do
     index <- index + 1
     if _LEXER_.processSequence(statement.Substring(index)) <> _LEXER_.failProcess then
       index <- index + 1
     else
       let ch:char = statement.[index]
       if ch = openc then
         count <- count + 1
       else if ch = closec then
         count <- count - 1
       if count = 0 && statement.[0] = openc then
         index <- index - 1
         statement <- statement.Substring(1, index)
         index <- statement.Length
   if count > 0 then
     terminal.printError("Bracket is opened but not closed!")
     _TOKENS_.FAILED <- true
   else if count < 0 then
     terminal.printError("Bracket is not opened but closed!")
     _TOKENS_.FAILED <- true
   statement

  /// <summary>
  /// Process escape sequence.
  /// </summary>
  /// <param name="value">Statement.</param>
  /// <returns>Value of escape sequence.</returns>
  static member processSequence(value:string) : string =
    if value.Length > 0 && value.[0] <> _TOKENS_.ESCAPESEQUENCE.[0] then
      _LEXER_.failProcess
    else
      let value:char = if value.Length > 1 then value.[1] else '_'
      if value = ' ' then
        terminal.printError("Escape sequence is cannot use alone!")
        _TOKENS_.FAILED <- true
        _LEXER_.failProcess
      else if value = _TOKENS_.ESCAPESEQUENCE.[0] then _TOKENS_.ESCAPESEQUENCE
      else if value = _TOKENS_.INLINECOMMENT.[0] then _TOKENS_.INLINECOMMENT
      else if value = _TOKENS_.DOLLAR.[0] then _TOKENS_.DOLLAR
      else if value = _TOKENS_.VBAR.[0] then _TOKENS_.VBAR
      else _LEXER_.failProcess
 
  /// <summary>
  /// Process value statement.
  /// </summary>
  /// <param name="variables">Variables.</param>
  /// <param name="value">Value to process.</param>
  /// <returns>Value.</returns>
  static member processValue(variables:List<_VARIABLE_> ref, value:string) : string =
    value
