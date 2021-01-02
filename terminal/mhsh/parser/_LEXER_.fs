namespace mhsh.parser

open System
open System.Collections.Generic

open terminal
open mhsh.objects

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
  /// Check statement is variable statement.
  /// </summary>
  /// <param name="statement">Statement.</param>
  /// <returns>true if statement is variable, false if not.</returns>
  static member isVariableStatement(statement:string) : bool =
    statement.Substring(0, 1) = _TOKENS_.DOLLAR
  
  /// <summary>
  /// Check statement is workflow statement.
  /// </summary>
  /// <param name="statement">Statement.</param>
  /// <returns>true if statement is workflow, false if not.</returns>
  static member isWorkflowStatement(statement:string) : bool =
    statement = _TOKENS_.workflowDefine
  
  /// <summary>
  /// Check statement is work statement.
  /// </summary>
  /// <param name="statement">Statement.</param>
  /// <returns>true if statement is work, false if not.</returns>
  static member isWorkStatement(statement:string) : bool =
    statement.Substring(0, _TOKENS_.workDefine.Length + 1) = _TOKENS_.workDefine + " "
  
  /// <summary>
  /// Check statement is literal statement.
  /// </summary>
  /// <param name="statement">Statement.</param>
  /// <returns>true if statement is literal, false if not.</returns>
  static member isLiteralStatement(statement:string) : bool =
    statement.Substring(0, 1) = _TOKENS_.VBAR

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
      if _PROCESSOR_.processSequence(statement.Substring(index)) <> _LEXER_.failProcess then
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
  /// Lex variable.
  /// </summary>
  /// <param name="index">Index.</param>
  /// <param name="Lines">Lines.</param>
  /// <returns>Parts of variable.</returns>
  static member lexVariable(index:int ref, lines:string[]) : List<string> =
    let parts:List<string> = new List<string>()
    let pos:int = lines.[!index].IndexOf(_TOKENS_.EQUAL)
    parts.Add(if pos <> -1 then lines.[!index].Substring(1, !index - 1) else lines.[!index].Substring(1))
    if pos <> -1 then
      parts.Add(lines.[!index].Substring(!index + 1));
      index := !index + 1
    else
      let mutable value:string = String.Empty
      let mutable break:bool = false
      while break = false && !index < lines.Length do
        index := !index + 1
        let line = _LEXER_.removeComments(lines.[!index])
        if line.TrimStart() = String.Empty then
          break <- true
        else if _LEXER_.isSkippableStatement(line) then
          break <- true
        value <- value + line
      parts.Add(value)
    parts

  /// <summary>
  /// Decompose variable name.
  /// </summary>
  /// <param name="statement">Statement.</param>
  /// <returns>Name of variable.</returns>
  static member getVariableNameFromStatement(statement:string) : string =
    if statement.Substring(0, 1) <> _TOKENS_.DOLLAR then
      _LEXER_.failProcess
    else
      let pos:int = _LEXER_.findVariableLimit(statement.Substring(1))
      if pos <> -1 then statement.Substring(1, pos) else statement.Substring(1)

  /// <summary>
  /// Decompose literal.
  /// </summary>
  /// <param name="index">Index.</param>
  /// <param name="lines">Lines.</param>
  /// <param name="variables">Variables.</param>
  /// <returns>Literal.</returns>
  static member getLiteral(index:int ref, lines:string[], variables:List<_VARIABLE_>) : _LITERAL_ =
    let lit:_LITERAL_ = new _LITERAL_()
    if _LEXER_.isLiteralStatement(lines.[!index]) = false then
      terminal.printError("Literal origin not defined!")
      _TOKENS_.FAILED <- true
    lit.value <- _PROCESSOR_.processValue(variables, lines.[!index].Substring(1))
    index := !index + 1
    let mutable break:bool = false
    while break = false && !index < lines.Length do
      let line:string = _LEXER_.removeComments(lines.[!index]).TrimStart()
      if line = String.Empty then
        index := !index - 1
        break <- true
      else if _LEXER_.isSkippableStatement(line) then
        index := !index - 1
        break <- true
      else if _LEXER_.isLiteralStatement(line) then
        index := !index - 1
        break <- true
      lit.line <- lit.line + 1
      lit.value <- lit.value + _PROCESSOR_.processValue(variables, line)
    lit

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
     if _PROCESSOR_.processSequence(statement.Substring(index)) <> _LEXER_.failProcess then
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
  /// Find work index.
  /// </summary>
  /// <param name="name">Name of work.</param>
  /// <param name="index">Index.</param>
  /// <param name="lines">Lines.</param>
  /// <returns>Index of line.</returns>
  static member findWork(name:string, index:int ref, lines:string[]) : int =
    let mutable break:bool = false
    while break = false && !index < lines.Length do
      index := !index + 1
      let line:string = lines.[!index].Trim()
      if _LEXER_.isWorkStatement(line) then
        if _LEXER_.getWorkName(line, true) = name then
          break <- true
    if break then
      !index
    else
      terminal.printError("Work is cannot define!")
      _TOKENS_.FAILED <- true
      -1

  /// <summary>
  /// Decompose work name.
  /// </summary>
  /// <param name="statement">Statement.</param>
  /// <param name="removeParameters">Remove parameters.</param>
  /// <returns>Name of work.</returns>
  static member getWorkName(statement:string, removeParameters:bool) : string =
    let mutable value:string = statement.Trim().Substring(_TOKENS_.workDefine.Length).Trim();
    if removeParameters then
      let pos:int = value.IndexOf(' ')
      value <- if pos <> -1 then value.Substring(0, pos) else value
    value

  /// <summary>
  /// Lex parameters.
  /// </summary>
  /// <param name="statement">Statement.</param>
  /// <returns>Parameters.</returns>
  static member lexParameters(statement:string) : List<string> =
    let parameters:List<string> = new List<string>()
    let mutable statement:string = statement
    let mutable last:int = 0
    let mutable index:int = -1
    while _TOKENS_.FAILED = false && index < statement.Length do
      index <- index + 1
      if statement.[index] = _TOKENS_.LPAR.[0] then
        index <- index + _LEXER_.lexBraceRange(_TOKENS_.LPAR.[0], _TOKENS_.RPAR.[0],
          statement.Substring(index)).Length + 1
      if statement.[index] = _TOKENS_.COMMA.[0] then
        if index > 0 then
          if statement.[index - 1] = _TOKENS_.COMMA.[0] then
            terminal.printError("Cannot define empty parameter!")
            _TOKENS_.FAILED <- true
          if (index > 1 && (statement.[index - 1] = _TOKENS_.ESCAPESEQUENCE.[0] &&
               statement.[index - 2] <> _TOKENS_.ESCAPESEQUENCE.[0])) then
            statement <- statement.Remove(index - 1, 1)
        let value = statement.Substring(last, index - last).Trim()
        if value <> String.Empty then
          parameters.Add(value);
          last <- index + 1;
    if last <> statement.Length then
      parameters.Add(statement.Substring(last).Trim());
    parameters
