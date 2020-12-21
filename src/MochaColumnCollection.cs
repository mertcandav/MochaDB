namespace MochaDB {
  using System;
  using System.Collections.Generic;
  using System.Linq;

  /// <summary>
  /// MochaColumn collector.
  /// </summary>
  public class MochaColumnCollection:MochaCollection<MochaColumn> {
    #region Constructors

    /// <summary>
    /// Create new MochaColumnCollection.
    /// </summary>
    public MochaColumnCollection() =>
      collection =new List<MochaColumn>();

    #endregion Constructors

    #region Events

    /// <summary>
    /// This happens after NameChanged event of any item in collection.
    /// </summary>
    public event EventHandler<EventArgs> ColumnNameChanged;
    protected virtual void OnColumnNameChanged(object sender,EventArgs e) {
      //Invoke.
      ColumnNameChanged?.Invoke(sender,e);
    }

    #endregion Events

    #region Item Events

    private void Item_NameChanged(object sender,EventArgs e) {
      IEnumerable<MochaColumn> result = collection.Where(x => x.Name==(sender as MochaColumn).Name);
      if(result.Count() > 1)
        throw new MochaException("There is already a column with this name!");

      OnColumnNameChanged(sender,e);
    }

    #endregion Item Events

    #region Members

    /// <summary>
    /// Remove all items.
    /// </summary>
    public override void Clear() {
      for(int index = 0; index < Count; ++index)
        collection[index].NameChanged-=Item_NameChanged;
      collection.Clear();
      OnChanged(this,new EventArgs());
    }

    /// <summary>
    /// Add item.
    /// </summary>
    /// <param name="item">Item to add.</param>
    public override void Add(MochaColumn item) {
      if(item == null)
        return;
      if(Contains(item.Name))
        throw new MochaException("There is already a column with this name!");

      item.NameChanged+=Item_NameChanged;
      collection.Add(item);
      OnChanged(this,new EventArgs());
    }

    /// <summary>
    /// Add item.
    /// </summary>
    /// <param name="name">Name of item.</param>
    public virtual void Add(string name) =>
        Add(new MochaColumn(name));

    /// <summary>
    /// Add item.
    /// </summary>
    /// <param name="name">Name of item.</param>
    /// <param name="datatype">Datatype of item.</param>
    public virtual void Add(string name,MochaDataType datatype) =>
        Add(new MochaColumn(name,dataType: datatype));

    /// <summary>
    /// Add item from range.
    /// </summary>
    /// <param name="items">Range to add items.</param>
    public override void AddRange(IEnumerable<MochaColumn> items) {
      foreach(MochaColumn column in items)
        Add(column);
    }

    /// <summary>
    /// Remove item.
    /// </summary>
    /// <param name="item">Item to remove.</param>
    public override void Remove(MochaColumn item) =>
      Remove(item.Name);

    /// <summary>
    /// Remove item by name.
    /// </summary>
    /// <param name="name">Name of item to remove.</param>
    public virtual void Remove(string name) {
      for(int index = 0; index < Count; ++index)
        if(collection[index].Name == name) {
          collection[index].NameChanged-=Item_NameChanged;
          collection.RemoveAt(index);
          OnChanged(this,new EventArgs());
          break;
        }
    }

    /// <summary>
    /// Remove item by index.
    /// </summary>
    /// <param name="index">Index of item to remove.</param>
    public override void RemoveAt(int index) =>
      Remove(collection[index].Name);

    /// <summary>
    /// Return index if index is find but return -1 if index is not find.
    /// </summary>
    /// <param name="name">Name of item to find index.</param>
    public virtual int IndexOf(string name) {
      for(int index = 0; index < Count; ++index)
        if(this[index].Name==name)
          return index;
      return -1;
    }

    /// <summary>
    /// Return true if item is exists but return false if item not exists.
    /// </summary>
    /// <param name="name">Name of item to exists check.</param>
    public virtual bool Contains(string name) =>
      IndexOf(name) != -1;

    /// <summary>
    /// Return first element in collection.
    /// </summary>
    public override MochaColumn GetFirst() =>
        IsEmptyCollection() ? null : this[0];

    /// <summary>
    /// Return last element in collection.
    /// </summary>
    public override MochaColumn GetLast() =>
        IsEmptyCollection() ? null : this[MaxIndex()];

    #endregion Members

    #region Properties

    /// <summary>
    /// Return item by name.
    /// </summary>
    /// <param name="name">Name of item.</param>
    public virtual MochaColumn this[string name] {
      get {
        int dex = IndexOf(name);
        if(dex!=-1)
          return ElementAt(dex);
        return null;
      }
    }

    #endregion Properties
  }
}
