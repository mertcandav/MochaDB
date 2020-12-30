namespace mhsh

open System
open System.IO

/// <summary>
/// Interpreter for mhsh(MochaDB Shell Script).
/// </summary>
type interpreter() =
  /// <summary>
  /// path declare for path property.
  /// </summary>
  let mutable _path:string = String.Empty

  /// <summary>
  /// Interpret script codes.
  /// </summary>
  member this.interpret() : unit =
    printfn "Interpret called!"

  /// <summary>
  /// Path of MochaDB Shell Script file.
  /// </summary>
  member this.path
    with get() = _path
    and set(value:string) = _path <- value
