namespace mhsh.parser

/// <summary>
/// Grammar.
/// </summary>
type _GRAMMAR_() =
  static member isValidVariableName(name:string) : bool =
    _LEXER_.findVariableLimit(name) = -1
