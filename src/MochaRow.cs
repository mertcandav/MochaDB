﻿namespace MochaDB {
  using System.Collections.Generic;

  /// <summary>
  /// This is row object for MochaDB.
  /// </summary>
  public class MochaRow {
    #region Constructors

    /// <summary>
    /// Create new MochaRow.
    /// </summary>
    public MochaRow() =>
      Datas = new MochaDataCollection();

    /// <summary>
    /// Create new MochaRow.
    /// </summary>
    /// <param name="datas">Datas of row.</param>
    public MochaRow(params MochaData[] datas) :
        this() => Datas.collection.AddRange(datas);

    /// <summary>
    /// Create new MochaRow.
    /// </summary>
    /// <param name="datas">Datas of row.</param>
    public MochaRow(IEnumerable<MochaData> datas)
        : this() => Datas.collection.AddRange(datas);

    /// <summary>
    /// Create new MochaRow.
    /// </summary>
    /// <param name="datas">Datas of row.</param>
    public MochaRow(params object[] datas) :
        this() {
      for(int dex = 0; dex < datas.Length; ++dex) {
        object data = datas[dex];
        Datas.collection.Add(
            new MochaData() {
              dataType = data == null ? MochaDataType.String : MochaData.GetDataTypeFromType(data.GetType()),
              data = data
            });
      }
    }

    #endregion Constructors

    #region Properties

    /// <summary>
    /// Datas of row.
    /// </summary>
    public virtual MochaDataCollection Datas { get; }

    #endregion Properties
  }
}
