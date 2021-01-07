namespace Server

/// <summary>
/// Config settings.
/// </summary>
type configs() =
  /// <summary>
  /// Name of server.
  /// </summary>
  static member val name:string = "MochaDB Server" with get, set

  /// <summary>
  /// IP address.
  /// </summary>
  static member val address:string = null with get, set

  /// <summary>
  /// Port.
  /// </summary>
  static member val port:int = 8085 with get, set

  /// <summary>
  /// The maximum length of the pending connections queue.
  /// </summary>
  static member val listen:int = 10 with get, set
