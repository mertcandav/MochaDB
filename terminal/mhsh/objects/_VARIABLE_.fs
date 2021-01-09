namespace mhsh.objects

open System

/// <summary>
/// A variable instance.
/// </summary>
[<Class>]
type _VARIABLE_() =
  class
  /// <summary>
  /// Name of variable.
  /// </summary>
  member val name:string = String.Empty with get, set

  /// <summary>
  /// Value of variable.
  /// </summary>
  member val value:string = String.Empty with get, set
  end
