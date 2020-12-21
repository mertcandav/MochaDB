namespace MochaDB {
  using System;
  using System.Collections.Generic;
  using System.Linq;

  /// <summary>
  /// MochaData collector.
  /// </summary>
  public class MochaDataCollection:MochaCollection<MochaData> {
    #region Constructors

    /// <summary>
    /// Create new MochaDataCollection.
    /// </summary>
    public MochaDataCollection() =>
      collection=new List<MochaData>();

    #endregion Constructors

    #region Members

    public override void Clear() {
      if(collection.Count==0)
        return;

      collection.Clear();
      OnChanged(this,new EventArgs());
    }

    public override void Add(MochaData item) {
      collection.Add(item);
      OnChanged(this,new EventArgs());
    }

    public override void AddRange(IEnumerable<MochaData> items) {
      foreach(MochaData data in items)
        Add(data);
    }

    public override void Remove(MochaData item) {
      if(collection.Remove(item))
        OnChanged(this,new EventArgs());
    }

    /// <summary>
    /// Removes all data equal to sample data.
    /// </summary>
    /// <param name="data">Sample data.</param>
    public virtual void RemoveAllData(object data) {
      int count = collection.Count;
      collection = (
          from currentdata in collection
          where currentdata.Data != data
          select currentdata).ToList();

      if(collection.Count != count)
        OnChanged(this,new EventArgs());
    }

    public override void RemoveAt(int index) {
      collection.RemoveAt(index);
      OnChanged(this,new EventArgs());
    }

    #endregion Members
  }
}
