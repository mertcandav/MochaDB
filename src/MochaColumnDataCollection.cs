namespace MochaDB {
  using System;
  using System.Collections;
  using System.Collections.Generic;
  using System.Linq;

  /// <summary>
  /// MochaData collector for MochaColumns.
  /// </summary>
  public class MochaColumnDataCollection:IEnumerable<MochaData>, IEnumerable {
    #region Fields

    internal List<MochaData> collection;
    private MochaDataType dataType;

    #endregion Fields

    #region Constructors

    /// <summary>
    /// Create new MochaColumnDataCollection.
    /// </summary>
    /// <param name="dataType">DataType of column.</param>
    public MochaColumnDataCollection(MochaDataType dataType) {
      collection=new List<MochaData>();
      this.dataType=dataType;
    }

    #endregion Constructors

    #region Events

    /// <summary>
    /// This happens after collection changed.
    /// </summary>
    public event EventHandler<EventArgs> Changed;
    protected virtual void OnChanged(object sender,EventArgs e) {
      //Invoke.
      Changed?.Invoke(this,e);
    }

    #endregion Events

    #region Internal Members

    /// <summary>
    /// Remove all items.
    /// </summary>
    internal protected virtual void Clear() {
      if(collection.Count ==0)
        return;
      collection.Clear();
    }

    /// <summary>
    /// Add item.
    /// </summary>
    /// <param name="item">Item to add.</param>
    internal protected virtual void Add(MochaData item) {
      if(DataType==MochaDataType.AutoInt)
        throw new MochaException("Data cannot be added directly to a column with AutoInt!");
      if(item.DataType == MochaDataType.Unique && !string.IsNullOrEmpty(item.Data.ToString()))
        if(ContainsData(item.Data))
          throw new MochaException("Any value can be added to a unique column only once!");

      if(item.DataType == DataType)
        collection.Add(item);
      else
        throw new MochaException("This data's datatype not compatible column datatype.");
    }

    /// <summary>
    /// Add data.
    /// </summary>
    /// <param name="data">Data to add.</param>
    internal protected virtual void AddData(object data) {
      if(MochaData.IsType(DataType,data))
        Add(new MochaData(DataType,data));
      else
        throw new MochaException("This data's datatype not compatible column datatype.");
    }

    /// <summary>
    /// Add item from range.
    /// </summary>
    /// <param name="items">Range to add items.</param>
    internal protected virtual void AddRange(IEnumerable<MochaData> items) {
      foreach(MochaData data in items)
        Add(data);
    }

    /// <summary>
    /// Remove item.
    /// </summary>
    /// <param name="item">Item to remove.</param>
    internal protected virtual void Remove(MochaData item) =>
      collection.Remove(item);

    /// <summary>
    /// Removes all data equal to sample data.
    /// </summary>
    /// <param name="data">Sample data.</param>
    internal protected virtual void RemoveAllData(object data) {
      int count = collection.Count;
      collection = (
          from currentdata in collection
          where currentdata.Data != data
          select currentdata).ToList();
    }

    /// <summary>
    /// Remove item by index.
    /// </summary>
    /// <param name="index">Index of item to remove.</param>
    internal protected virtual void RemoveAt(int index) =>
      collection.RemoveAt(index);

    #endregion Internal Members

    #region Members

    /// <summary>
    /// Return true if data is contained but return false if not exists.
    /// </summary>
    /// <param name="data">Data to check.</param>
    public virtual bool ContainsData(object data) {
      for(int index = 0; index < Count; ++index)
        if(data.Equals(this[index].Data))
          return true;
      return false;
    }

    /// <summary>
    /// Returns enumerator.
    /// </summary>
    public virtual IEnumerator<MochaData> GetEnumerator() =>
        collection.GetEnumerator();

    /// <summary>
    /// Returns enumerator.
    /// </summary>
    IEnumerator IEnumerable.GetEnumerator() =>
        collection.GetEnumerator();

    #endregion Members

    #region Properties

    /// <summary>
    /// Data type of column.
    /// </summary>
    public virtual MochaDataType DataType {
      get => dataType;
      internal protected set {
        if(value == dataType)
          return;

        dataType = value;

        if(value == MochaDataType.AutoInt)
          return;

        for(int index = 0; index < Count; ++index)
          collection[index].DataType = dataType;
      }
    }

    /// <summary>
    /// Return item by index.
    /// </summary>
    /// <param name="index">Index of item.</param>
    public virtual MochaData this[int index] =>
        collection[index];

    /// <summary>
    /// Count of items.
    /// </summary>
    public virtual int Count =>
        collection.Count;

    #endregion Properties
  }
}
