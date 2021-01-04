namespace mhsh.objects

open System
open System.Collections.Generic

/// <summary>
/// Work instance.
/// </summary>
type _WORK_() =
  /// <summary>
  /// Name of work.
  /// </summary>
  member val name:string = String.Empty with get, set

  /// <summary>
  /// Parameters of work.
  /// </summary>
  member val parameters:List<string> = new List<string>() with get, set

  /// <summary>
  /// Commands of work.
  /// </summary>
  member val literals:List<string> = new List<string>() with get, set
