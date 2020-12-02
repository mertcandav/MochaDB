namespace MochaDB {
  using System;
  using System.Collections.Generic;
  using System.Linq;

  /// <summary>
  /// MochaRow collector.
  /// </summary>
  public class MochaRowCollection:MochaCollection<MochaRow> {
    #region Constructors

    /// <summary>
    /// Create new MochaRowCollection.
    /// </summary>
    public MochaRowCollection() =>
      collection=new List<MochaRow>();

    #endregion Constructors

    #region Events

    /// <summary>
    /// This happens after Changed event of any item in collection.
    /// </summary>
    public event EventHandler<EventArgs> RowChanged;
    private void OnRowChanged(object sender,EventArgs e) {
      //Invoke.
      RowChanged?.Invoke(sender,e);
    }

    #endregion Events

    #region Item Events

    private void Item_Changed(object sender,EventArgs e) =>
      OnRowChanged(sender,e);

    #endregion Item Events

    #region Members

    public override void Clear() {
      for(int index = 0; index < Count; ++index)
        collection[index].Datas.Changed-=Item_Changed;
      collection.Clear();
    }

    public override void Add(MochaRow item) {
      if(item== null)
        return;

      collection.Add(item);
      item.Datas.Changed+=Item_Changed;
      OnChanged(this,new EventArgs());
    }

    /// <summary>
    /// Add item.
    /// </summary>
    /// <param name="datas">Datas of item.</param>
    public void Add(params MochaData[] datas) =>
        Add(new MochaRow(datas));

    /// <summary>
    /// Add item.
    /// </summary>
    /// <param name="datas">Datas of item.</param>
    public void Add(IEnumerable<MochaData> datas) =>
        Add(new MochaRow(datas));

    /// <summary>
    /// Add item.
    /// </summary>
    /// <param name="datas">Datas of item.</param>
    public void Add(params object[] datas) =>
        Add(new MochaRow(datas));

    public override void AddRange(IEnumerable<MochaRow> items) {
      for(int index = 0; index < items.Count(); ++index)
        Add(items.ElementAt(index));
    }

    public override void Remove(MochaRow item) {
      int dex = IndexOf(item);
      if(dex!=-1)
        RemoveAt(dex);
    }

    public override void RemoveAt(int index) {
      collection[index].Datas.Changed-=Item_Changed;
      collection.RemoveAt(index);
      OnChanged(this,new EventArgs());
    }

    public override MochaRow GetFirst() =>
        IsEmptyCollection() ? null : this[0];

    public override MochaRow GetLast() =>
        IsEmptyCollection() ? null : this[MaxIndex()];

    #endregion Members
  }
}
