﻿namespace MochaDB {
  using System;
  using System.Collections;
  using System.Collections.Generic;

  /// <summary>
  /// Collection interface for MochaDB.
  /// </summary>
  /// <typeparam name="T">Item type of collector.</typeparam>
  public interface IMochaCollection<T>:IEnumerable<T>, IEnumerable, ICollection<T> {
    #region Events

    event EventHandler<EventArgs> Changed;

    #endregion Events

    #region Members

    void AddRange(IEnumerable<T> items);
    int MaxIndex();
    bool IsEmptyCollection();
    T ElementAt(int index);
    T[] ToArray();
    List<T> ToList();
    T GetFirst();
    T GetLast();

    #endregion Members

    #region Properties

    T this[int index] { get; }

    #endregion Properties
  }
}
