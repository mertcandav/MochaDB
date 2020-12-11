﻿namespace MochaDB.Streams {
  using System.Collections.Generic;
  using System.Linq;

  /// <summary>
  /// Stream reader for MochaDB.
  /// </summary>
  /// <typeparam name="T">Value type of stream reader.</typeparam>
  public class MochaReader<T> {
    #region Fields

    internal IEnumerable<T> array;

    #endregion Fields

    #region Constructors

    /// <summary>
    /// Create a new MochaReader.
    /// </summary>
    /// <param name="values">Values of stream.</param>
    public MochaReader(params T[] values) {
      array = values;
      Value=null;
      Position=-1;
    }

    /// <summary>
    /// Create a new MochaReader.
    /// </summary>
    /// <param name="values">Values of stream.</param>
    public MochaReader(IEnumerable<T> values) {
      array = values.ToArray();
      Value=null;
      Position=-1;
    }

    #endregion Constructors

    #region Operators

    public static explicit operator string(MochaReader<T> value) =>
        value.ToString();

    #endregion Operators

    #region Members

    /// <summary>
    /// Returns true if value is exists in next position but returns if not.
    /// </summary>
    public virtual bool Read() {
      if(Position+1 < Count) {
        Value = array.ElementAt(++Position);
        return true;
      }

      Value=null;
      return false;
    }

    /// <summary>
    /// Go to previous position.
    /// </summary>
    public virtual void GoBack() =>
      Position = Position != -1 ? Position - 1 : Position;

    /// <summary>
    /// Go to first position.
    /// </summary>
    public virtual void GoFirst() =>
        Position=-1;

    /// <summary>
    /// Go to last position.
    /// </summary>
    public virtual void GoLast() =>
        Position=Count-2 < -1 ? -1 : Count-2;

    #endregion Members

    #region Overrides

    /// <summary>
    /// Returns converted to string result of <see cref="Value"/>.
    /// </summary>
    public override string ToString() =>
      Value.ToString();

    #endregion Overrides

    #region Properties

    /// <summary>
    /// Current value.
    /// </summary>
    public virtual object Value { get; internal protected set; }

    /// <summary>
    /// Current value index of reader.
    /// </summary>
    public virtual int Position { get; internal protected set; }

    /// <summary>
    /// Count of value.
    /// </summary>
    public virtual int Count =>
        array.Count();

    #endregion Properties
  }
}
