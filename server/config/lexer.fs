namespace config

/// <summary>
/// Lexer of config system.
/// </summary>
type lexer() =
  /// <summary>
  /// Key type is string?
  /// </summary>
  /// <param name="name">Name of key.</param>
  /// <returns>true is type of key is string, false if not.</returns>
  static member isStringKey(name:string) : bool =
    match name with
    | "name"
    | "address" -> true
    | _ -> false

  /// <summary>
  /// Key type is integer?
  /// </summary>
  /// <param name="name">Name of key.</param>
  /// <returns>true is type of key is integer, false if not.</returns>
  static member isIntegerKey(name:string) : bool =
    match name with
    | "port"
    | "listen" -> true
    | _ -> false
