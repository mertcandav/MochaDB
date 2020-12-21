namespace MochaDB {
  using System;
  using System.Collections;
  using System.Collections.Generic;

  /// <summary>
  /// Base class for MochaDB collections.
  /// </summary>    
  /// <typeparam name="T">Item type.</typeparam>
  public abstract class MochaCollection<T>:IMochaCollection<T> {
    #region Fields

    internal protected List<T> collection;

    #endregion Fields

    #region Events

    /// <summary>
    /// This happens after collection changed.
    /// </summary>
    public event EventHandler<EventArgs> Changed;
    protected virtual void OnChanged(object sender,EventArgs e) {
      //Invoke.
      Changed?.Invoke(sender,e);
    }

    #endregion Events

    #region Abstract Members

    /// <summary>
    /// Remove all items.
    /// </summary>
    public abstract void Clear();

    /// <summary>
    /// Add item.
    /// </summary>
    /// <param name="item">Item to add.</param>
    public abstract void Add(T item);

    /// <summary>
    /// Add item from range.
    /// </summary>
    /// <param name="items">Range to add items.</param>
    public abstract void AddRange(IEnumerable<T> items);

    /// <summary>
    /// Remove item.
    /// </summary>
    /// <param name="item">Item to remove.</param>
    public abstract void Remove(T item);

    /// <summary>
    /// Remove item by index.
    /// </summary>
    /// <param name="index">Index of item to remove.</param>
    public abstract void RemoveAt(int index);

    #endregion Abstract Members

    #region Virtual Members

    /// <summary>
    /// Return index if index is find but return -1 if index is not find.
    /// </summary>
    /// <param name="item">Item to find index.</param>
    public virtual int IndexOf(T item) =>
      collection.IndexOf(item);

    /// <summary>
    /// Return true if item is exists but return false if item not exists.
    /// </summary>
    /// <param name="item">Item to exists check.</param>
    public virtual bool Contains(T item) =>
      collection.Contains(item);

    /// <summary>
    /// Return element by index.
    /// </summary>
    /// <param name="index">Index of element.</param>
    public virtual T ElementAt(int index) =>
        collection[index];

    /// <summary>
    /// Create and return static array from collection.
    /// </summary>
    public virtual T[] ToArray() =>
        collection.ToArray();

    /// <summary>
    /// Returns enumerator.
    /// </summary>
    public virtual IEnumerator<T> GetEnumerator() =>
        collection.GetEnumerator();

    /// <summary>
    /// Copy items to array by start index.
    /// </summary>
    /// <param name="array">Destination array.</param>
    /// <param name="arrayIndex">Index to start copying.</param>
    public virtual void CopyTo(T[] array,int arrayIndex) =>
      collection.CopyTo(array,arrayIndex);

    #endregion Virtual Members

    #region Members

    /// <summary>
    /// Returns enumerator.
    /// </summary>
    IEnumerator IEnumerable.GetEnumerator() =>
        collection.GetEnumerator();

    /// <summary>
    /// Removes the first occurrence of a specific object from the collection.
    /// </summary>
    /// <param name="item">Item to remove.</param>
    bool ICollection<T>.Remove(T item) {
      bool state = Contains(item);
      Remove(item);
      return state;
    }

    #endregion Members

    #region Properties

    /// <summary>
    /// Return item by index.
    /// </summary>
    /// <param name="index">Index of item.</param>
    public virtual T this[int index] =>
        ElementAt(index);

    /// <summary>
    /// Count of items.
    /// </summary>
    public virtual int Count =>
        collection.Count;

    /// <summary>
    /// Is Readonly collection.
    /// </summary>
    public virtual bool IsReadOnly =>
        false;

    #endregion Properties
  }
}
