namespace utils

open System

/// <summary>
/// MochaQ utilities.
/// </summary>
[<Class>]
type mhq() =
  class
  /// <summary>
  /// Command is run type or not?
  /// </summary>
  /// <param name="command">Command to check.</param>
  /// <returns>true if command is run type, false if not.</returns>
  static member CommandIsRunType(command:string) : bool =
    let command:string = command.ToUpperInvariant()
    String.IsNullOrEmpty(command) = false && (
      command.StartsWith("RESET") ||
      command.StartsWith("SET") ||
      command.StartsWith("ADD") ||
      command.StartsWith("CREATE") ||
      command.StartsWith("CLEAR") ||
      command.StartsWith("REMOVE") ||
      command.StartsWith("RENAME") ||
      command.StartsWith("UPDATE") ||
      command.StartsWith("RESTORE"))

  /// <summary>
  /// Command is getrun type or not?
  /// </summary>
  /// <param name="command">Command to check.</param>
  /// <returns>true if command is getrun type, false if not.</returns>
  static member CommandIsGetRunType(command:string) : bool =
    let command:string = command.ToUpperInvariant()
    String.IsNullOrEmpty(command) = false && (
      command.StartsWith("GET") ||
      command.StartsWith("TABLECOUNT") ||
      command.StartsWith("COLUMNCOUNT") ||
      command.StartsWith("ROWCOUNT") ||
      command.StartsWith("DATACOUNT") ||
      command.StartsWith("EXISTS"))
  end
