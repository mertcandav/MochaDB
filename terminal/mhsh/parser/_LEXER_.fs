namespace mhsh.parser

open System
open System.Collections.Generic
open System.Diagnostics

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
  /// Lex variable.
  /// </summary>
  /// <param name="index">Last index of lines.</param>
  /// <param name="lines">Lines.</param>
  /// <returns>Parts of variable.</returns>
  static member lexVariable(index:int ref, lines:string[] ref) : List<string> =
    let parts:List<string> = new List<string>()
    let pos:int = (!lines).[!index].IndexOf(_TOKENS_.EQUAL)
    parts.Add(if pos <> -1 then (!lines).[!index].Substring(1, !index - 1) else (!lines).[!index].Substring(1))
    if pos <> -1 then
      parts.Add((!lines).[!index].Substring(!index + 1));
      index := !index + 1
    else
      let mutable value:string = String.Empty
      let mutable break:bool = false
      while break = false && !index < (!lines).Length do
        index := !index + 1
        let line = _LEXER_.removeComments((!lines).[!index])
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
  static member getLiteral(index:int ref, lines:string[] ref, variables:List<_VARIABLE_> ref) : _LITERAL_ =
    let lit:_LITERAL_ = new _LITERAL_()
    if _LEXER_.isLiteralStatement((!lines).[!index]) = false then
      terminal.printError("Literal origin not defined!")
      _TOKENS_.FAILED <- true
    lit.value <- _LEXER_.processValue(variables, (!lines).[!index].Substring(1))
    index := !index + 1
    let mutable break:bool = false
    while break = false && !index < (!lines).Length do
      let line:string = _LEXER_.removeComments((!lines).[!index]).TrimStart()
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
      lit.value <- lit.value + _LEXER_.processValue(variables, line)
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
  /// Find work index.
  /// </summary>
  /// <param name="name">Name of work.</param>
  /// <param name="index">Last index of lines.</param>
  /// <param name="lines">Lines.</param>
  /// <returns>Index of line.</returns>
  static member findWork(name:string, index:int ref, lines:string[] ref) : int =
    let mutable break:bool = false
    while break = false && !index < (!lines).Length do
      index := !index + 1
      let line:string = (!lines).[!index].Trim()
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

  /// <summary>
  /// Process escape sequence.
  /// </summary>
  /// <param name="value">Statement.</param>
  /// <returns>Value of escape sequence.</returns>
  static member processSequence(value:string) : string =
    if value.Substring(0, 1) <> _TOKENS_.ESCAPESEQUENCE then
      _LEXER_.failProcess
    else
      let value:string = value.Substring(1, 1)
      if value = " " then
        terminal.printError("Escape sequence is cannot use alone!")
        _LEXER_.failProcess
      else if value = _TOKENS_.ESCAPESEQUENCE then _TOKENS_.ESCAPESEQUENCE
      else if value = _TOKENS_.INLINECOMMENT then _TOKENS_.INLINECOMMENT
      else if value = _TOKENS_.DOLLAR then _TOKENS_.DOLLAR
      else if value = _TOKENS_.VBAR then _TOKENS_.VBAR
      else _LEXER_.failProcess

  /// <summary>
  /// Check name is valid or not.
  /// </summary>
  /// <param name="name">Name to check.</param>
  /// <returns>true is valid, false if not.</returns>
  static member isValidVariableName(name:string) : bool =
    _LEXER_.findVariableLimit(name) = -1

  /// <summary>
  /// Process work.
  /// </summary>
  /// <param name="name">Name of work.</param>
  /// <param name="index">Last index of lines.</param>
  /// <param name="lines">Lines.</param>
  /// <param name="variables">Variables.</param>
  /// <returns>true if success, false if not.</returns>
  static member processWork(name:string, index:int ref, lines:string[] ref, variables:List<_VARIABLE_> ref) : bool =
    let workindex:int = _LEXER_.findWork(name, index, lines)

    let work:_WORK_ = _LEXER_.skipWork(workindex |> ref, lines, variables)
    for literal:string in work.literals do
      printfn "%s" literal
      let process:Process = new Process()
      process.StartInfo = new ProcessStartInfo()
      process.StartInfo.WindowStyle <- ProcessWindowStyle.Hidden
      process.StartInfo.CreateNoWindow <- true
      process.StartInfo.UseShellExecute <- true
      process.StartInfo.Arguments <- literal
      process.Start()
      process.WaitForExit()
    true

  /// <summary>
  /// Skip work with processing.
  /// </summary>
  /// <param name="index">Last index of lines.</param>
  /// <param name="lines">Lines.</param>
  /// <param name="variables">Variables.</param>
  /// <returns>Work instance of skipped work.</returns>
  static member skipWork(index:int ref, lines:string[] ref, variables:List<_VARIABLE_> ref) : _WORK_ =
    if _LEXER_.isWorkStatement((!lines).[!index].Trim()) then
      terminal.printError("This statement is not work statement!")
      _TOKENS_.FAILED <- true
    let work:_WORK_ = new _WORK_()
    work.name <- _LEXER_.getWorkName((!lines).[!index], false)
    let pos:int = work.name.IndexOf(" ")
    if pos <> -1 then 
      work.parameters <- _LEXER_.lexParameters(work.name.Substring(pos))
      work.name <- work.name.Substring(0, pos)
    if _LEXER_.isValidVariableName(work.name) = false then
      terminal.printError("Work name is not valid!")
      _TOKENS_.FAILED <- true
    let mutable counter:int = 0
    for parameter:string in work.parameters do
      let parameter:string = _LEXER_.processValue(variables, parameter)
      let var:_VARIABLE_ = new _VARIABLE_()
      counter <- counter + 1
      var.name <- counter.ToString()
      var.value <- parameter
      (!variables).Add(var)
    index := !index + 1
    let mutable break:bool = false
    while break = false && !index < (!lines).Length do
      let line:string = _LEXER_.removeComments((!lines).[!index]).TrimStart()

      if line = String.Empty then
        break <- true
      else if _LEXER_.isSkippableStatement(line) then
        break <- true
      else
        work.literals.Add((_LEXER_.getLiteral(index, lines, variables)).value.Trim())
      index := !index + 1
    for _:int in 0..counter - 1 do
      (!variables).RemoveAt((!variables).Count - 1)
    work
  
  /// <summary>
  /// Process variable.
  /// </summary>
  /// <param name="index">Last index of lines.</param>
  /// <param name="lines">Lines.</param>
  /// <param name="variables">Variables.</param>
  /// <returns>true if success, false if not.</returns>
  static member processVariable(index:int ref, lines:string[] ref, variables:List<_VARIABLE_> ref) : bool =
    if _LEXER_.isVariableStatement((!lines).[!index]) = false then
      false
    else
      let variable:_VARIABLE_ = new _VARIABLE_()
      let parts:List<string> = _LEXER_.lexVariable(index, lines)
      variable.name <- parts.[0].Trim()
      if _LEXER_.isValidVariableName(variable.name) = false then
        terminal.printError("Variable name is not valid!")
        _TOKENS_.FAILED <- true
        false
      else
        let mutable iindex:int = -1
        while iindex < (!variables).Count do
          iindex <- iindex + 1
          let vvariable:_VARIABLE_ = (!variables).[iindex]
          if vvariable.name = variable.name then
            vvariable.value <- _LEXER_.processValue(variables, parts.[parts.Count - 1]).Trim()
            iindex <- (!variables).Count + 1
        if iindex <> (!variables).Count + 1 then
          index := !index + 1
          variable.value <- _LEXER_.processValue(variables, parts.[parts.Count - 1]).Trim()
          (!variables).Add(variable)
        true

  /// <summary>
  /// Process value statement.
  /// </summary>
  /// <param name="variables">Variables.</param>
  /// <param name="value">Value to process.</param>
  /// <returns>Value.</returns>
  static member processValue(variables:List<_VARIABLE_> ref, value:string) : string =
    let mutable _val:string = String.Empty
    let mutable index:int = -1
    while index < value.Length do
      index <- index + 1
      let mutable sequence:string = _LEXER_.processSequence(value.Substring(index))
      if sequence <> _LEXER_.failProcess then
        _val <- _val + sequence
        index <- index + 1
      else
        sequence <- _LEXER_.getVariableNameFromStatement(value.Substring(index))
        if sequence <> _LEXER_.failProcess then
          sequence <- sequence.Trim()
          index <- index + sequence.Length
          let mutable iindex:int = -1
          while iindex < (!variables).Count do
            iindex <- iindex + 1
            let variable:_VARIABLE_ = (!variables).[iindex]
            if variable.name = sequence then
              _val <- _val + variable.value
              sequence <- _LEXER_.failProcess
              iindex <- (!variables).Count
          if sequence <> _LEXER_.failProcess then
            terminal.printError("Variable is cannot define!")
            _TOKENS_.FAILED <- true
        else if value.[index] = _TOKENS_.LPAR.[0] then
          let statement:string = _LEXER_.lexBraceRange(_TOKENS_.LPAR.[0], _TOKENS_.RPAR.[0],
            value.Substring(index))
          _val <- _val + _LEXER_.processValue(variables, statement)
          index <- index + statement.Length + 1
        else
          _val <- _val + value.[index].ToString()
    _val

  /// <summary>
  /// Process workflow.
  /// </summary>
  /// <param name="index">Last index of lines.</param>
  /// <param name="lines">Lines.</param>
  /// <param name="variables">Variables.</param>
  /// <returns>true if success, false if not.</returns>
  static member processWorkflow(index:int ref, lines:string[] ref, variables:List<_VARIABLE_> ref) : bool =
    if _LEXER_.isWorkflowStatement((!lines).[!index]) = false then
      false
    else
      if (!lines).[!index].Trim() <> _TOKENS_.workflowDefine then
        terminal.printError("Workflow define line is must are alone!")
        _TOKENS_.FAILED <- true
        false
      else
        index := !index + 1
        let workflow:_WORKFLOW_ = new _WORKFLOW_()
        let mutable break:bool = false
        while break = false && !index < (!lines).Length do
          let line:string = _LEXER_.removeComments((!lines).[!index]).TrimStart()
          if line = String.Empty then
            break <- true
          else if _LEXER_.isSkippableStatement(line) then
            break <- true
          else
            let literal:_LITERAL_ = _LEXER_.getLiteral(index, lines, variables)
            workflow.works.Add(literal.value.Trim())
        for work:string in workflow.works do
          for part:string in work.Split(" ") do
            if part <> String.Empty then
              _LEXER_.processWork(part, index, lines, variables) |> ignore
        true
