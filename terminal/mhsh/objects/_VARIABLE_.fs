namespace mhsh.objects

open System

/// <summary>
/// A variable instance.
/// </summary>
type _VARIABLE_ =
  struct
    /// <summary>
    /// Name of variable.
    /// </summary>
    val name:string

    /// <summary>
    /// Value of variable.
    /// </summary>
    val value:string

    /// <summary>
    /// Create new instance of <see cref="_VARIABLE_"/>
    /// </summary>
    /// <param name="name">Name of variable.</param>
    new(name:string) = { name = name; value = String.Empty; }

    /// <summary>
    /// Create new instance of <see cref="_VARIABLE_"/>
    /// </summary>
    /// <param name="name">Name of variable.</param>
    /// <param name="value">Value of variable.</param>
    new(name:string, value:string) = { name = name; value = value; }
  end
