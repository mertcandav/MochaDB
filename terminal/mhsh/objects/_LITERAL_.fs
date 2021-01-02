namespace mhsh.objects

open System

/// <summary>
/// A literal instance.
/// </summary>
type _LITERAL_() =
  /// <summary>
  /// Value of literal.
  /// </summary>
  member val value:string = String.Empty with get, set

  /// <summary>
  /// Lines count of literal.
  /// </summary>
  member val line:int = 0 with get, set
