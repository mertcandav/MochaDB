namespace config

/// <summary>
/// Tokens.
/// </summary>
[<Class>]
type tokens() =
  class
  /// <summary>
  /// Inline comment.
  /// </summary>
  static member val INLINECOMMENT:string = "#" with get

  /// <summary>
  /// Escape sequence character of values.
  /// </summary>
  static member val ESCAPESEQUENCE:string = "\\" with get
  
  /// <summary>
  /// Left parentheses.
  /// </summary>
  static member val LPAR:string = "(" with get
  
  /// <summary>
  /// Right parantheses.
  /// </summary>
  static member val RPAR:string = ")" with get
  
  /// <summary>
  /// Colon.
  /// </summary>
  static member val COLON:string = ":" with get
  
  /// <summary>
  /// Comma.
  /// </summary>
  static member val COMMA:string = "," with get
  
  /// <summary>
  /// Semicolon.
  /// </summary>
  static member val SEMICOLON:string = ";" with get
  
  /// <summary>
  /// Plus.
  /// </summary>
  static member val PLUS:string = "+" with get
  
  /// <summary>
  /// Minus.
  /// </summary>
  static member val MINUS:string = "-" with get
  
  /// <summary>
  /// Start.
  /// </summary>
  static member val STAR:string = "*" with get
  
  /// <summary>
  /// Slash.
  /// </summary>
  static member val SLASH:string = "/" with get
  
  /// <summary>
  /// Vertical Bar.
  /// </summary>
  static member val VBAR:string = "|" with get
  
  /// <summary>
  /// Amper.
  /// </summary>
  static member val AMPER:string = "&" with get
  
  /// <summary>
  /// Less than.
  /// </summary>
  static member val LESS:string = "<" with get
  
  /// <summary>
  /// Greater than.
  /// </summary>
  static member val GREATER:string = ">" with get
  
  /// <summary>
  /// Equal.
  /// </summary>
  static member val EQUAL:string = "=" with get
  
  /// <summary>
  /// Dot.
  /// </summary>
  static member val DOT:string = "." with get
  
  /// <summary>
  /// Percent.
  /// </summary>
  static member val PERCENT:string = "%" with get
  
  /// <summary>
  /// Left bracket.
  /// </summary>
  static member val LBRACE:string = "{" with get
  
  /// <summary>
  /// Right bracket.
  /// </summary>
  static member val RBRACE:string = "}" with get
  
  /// <summary>
  /// Equals to.
  /// </summary>
  static member val EQEQUAL:string = "==" with get
  
  /// <summary>
  /// Not equals to.
  /// </summary>
  static member val NOTEQUAL:string = "!=" with get
  
  /// <summary>
  /// Less or equals.
  /// </summary>
  static member val LESSEQUAL:string = "<=" with get
  
  /// <summary>
  /// Greater or equals.
  /// </summary>
  static member val GREATEREQUAL:string = ">=" with get
  
  /// <summary>
  /// Tilde.
  /// </summary>
  static member val TILDE:string = "~" with get
  
  /// <summary>
  /// Circumflex.
  /// </summary>
  static member val CIRCUMFLEX:string = "^" with get
  
  /// <summary>
  /// Left shift.
  /// </summary>
  static member val LEFTSHIFT:string = "<<" with get
  
  /// <summary>
  /// Right shift.
  /// </summary>
  static member val RIGHTSHIFT:string = ">>" with get
  
  /// <summary>
  /// Double star.
  /// </summary>
  static member val DOUBLESTAR:string = "**" with get
  
  /// <summary>
  /// Plus equal.
  /// </summary>
  static member val PLUSEQUAL:string = "+=" with get
  
  /// <summary>
  /// Minus equal.
  /// </summary>
  static member val MINEQUAL:string = "-=" with get
  
  /// <summary>
  /// Slash equal.
  /// </summary>
  static member val SLASHEQUAL:string = "/=" with get
  
  /// <summary>
  /// Percent equal.
  /// </summary>
  static member val PERCENTEQUAL:string = "%=" with get
  
  /// <summary>
  /// Amper equal.
  /// </summary>
  static member val AMPEREQUAL:string = "&=" with get
  
  /// <summary>
  /// Vertical Bar equal.
  /// </summary>
  static member val VBAREQUAL:string = "|=" with get
  
  /// <summary>
  /// Circumflex equal.
  /// </summary>
  static member val CIRCUMFLEXEQUAL:string = "^=" with get
  
  /// <summary>
  /// Left shift equal
  /// </summary>
  static member val LEFTSHIFTEQUAL:string = "<<=" with get
  
  /// <summary>
  /// Right shift equal.
  /// </summary>
  static member val RIGHTSHIFTEQUAL:string = ">>=" with get
  
  /// <summary>
  /// Double start equal.
  /// </summary>
  static member val DOUBLESTAREQUAL:string = "**=" with get
  
  /// <summary>
  /// Double slahs equal.
  /// </summary>
  static member val DOUBLESLASHEQUAL:string = "//=" with get
  
  /// <summary>
  /// At.
  /// </summary>
  static member val AT:string = "@" with get
  
  /// <summary>
  /// At equal.
  /// </summary>
  static member val ATEQUAL:string = "@=" with get
  
  /// <summary>
  /// Left arrow.
  /// </summary>
  static member val LARROW:string = "<-" with get
  
  /// <summary>
  /// Right arrow.
  /// </summary>
  static member val RARROW:string = "->" with get
  
  /// <summary>
  /// Ellipsis.
  /// </summary>
  static member val ELLIPSIS:string = "..." with get
  
  /// <summary>
  /// Colon equal.
  /// </summary>
  static member val COLONEQUAL:string = ":=" with get
  
  /// <summary>
  /// Dollar mark.
  /// </summary>
  static member val DOLLAR:string = "$" with get
  
  /// <summary>
  /// Dollar equal.
  /// </summary>
  static member val DOLLAREQUAL:string = "$=" with get
  end
