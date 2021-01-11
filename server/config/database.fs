namespace config

open MochaDB

/// <summary>
/// A database instance.
/// </summary>
[<Struct>]
type database =
  struct
  /// <summary>
  /// Name of database.
  /// </summary>
  val name:string

  /// <summary>
  /// Connection of connection.
  /// <summary>
  val connection:MochaDatabase

  /// <summary>
  /// Create a new instance of <see cref="database"/>.
  /// </summary>
  /// <param name="name">Name of database.</param>
  /// <param name="value">Connection of database.</param>
  new(name:string, connection:MochaDatabase) = {
    name = name
    connection = connection
  }
  end
