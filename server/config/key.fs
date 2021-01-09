namespace config

open System

/// <summary>
/// A key instance.
/// </summary>
[<Struct>]
type key =
  struct
  /// <summary>
  /// Name of key.
  /// </summary>
  val name:string

  /// <summary>
  /// Value of key.
  /// <summary>
  val value:string

  /// <summary>
  /// Create a new instance of <see cref="key"/>.
  /// </summary>
  /// <param name="name">Name of key.</param>
  new(name:string) = {
    name = name
    value = String.Empty
  }

  /// <summary>
  /// Create a new instance of <see cref="key"/>.
  /// </summary>
  /// <param name="name">Name of key.</param>
  /// <param name="value">Value of key.</param>
  new(name:string, value:string) = {
    name = name
    value = value
  }
  end
