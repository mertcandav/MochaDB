namespace MochaDB.Mhql {
  using System;
  using System.Linq;

  /// <summary>
  /// Table result for Mhql query results.
  /// </summary>
  public class MochaTableResult {
    #region Internal Constructors

    /// <summary>
    /// Create a new MochaTableResult.
    /// </summary>
    internal protected MochaTableResult() { }

    #endregion Internal Constructors

    #region Internal Members

    /// <summary>
    /// Set rows by datas of columns.
    /// </summary>
    internal protected virtual void SetRowsByDatas() {
      if(Columns.Length > 0 && Columns[0].Datas.Count > 0) {
        MochaColumn firstcolumn = Columns[0];
        MochaRow[] rows = new MochaRow[firstcolumn.Datas.Count];
        //Process rows.
        for(int dataindex = 0; dataindex < firstcolumn.Datas.Count; ++dataindex) {
          MochaData[] datas = new MochaData[Columns.Length];
          for(int columnindex = 0; columnindex < Columns.Length; ++columnindex) {
            MochaColumn column = Columns[columnindex];
            datas[columnindex] =
                column.Datas.Count < dataindex+1 ?
                new MochaData { data =string.Empty,dataType=MochaDataType.String } :
                column.Datas[dataindex];
          }
          rows[dataindex] = new MochaRow(datas);
        }
        Rows = rows;
        return;
      } else
        Rows = new MochaRow[0];
    }

    /// <summary>
    /// Set datas by row datas.
    /// </summary>
    internal protected virtual void SetDatasByRows() {
      for(int columnIndex = 0; columnIndex < Columns.Length; ++columnIndex)
        Columns[columnIndex].Datas.collection.Clear();

      for(int rowIndex = 0; rowIndex < Rows.Length; ++rowIndex) {
        MochaRow currentRow = Rows[rowIndex];
        for(int columnIndex = 0; columnIndex < Columns.Length; ++columnIndex) {
          MochaColumn currentColumn = Columns[columnIndex];
          if(columnIndex >= currentRow.Datas.Count) {
            currentRow.Datas.collection.Add(new MochaData() {
              dataType = currentColumn.DataType,
              data = MochaData.TryGetData(currentColumn.DataType,null)
            });
          }
          currentColumn.Datas.collection.Add(currentRow.Datas[columnIndex]);
        }
      }
    }

    #endregion Internal Members

    #region Members

    /// <summary>
    /// Returns table empty state.
    /// </summary>
    public virtual bool IsEmpty() =>
      Rows.Length == 0;

    /// <summary>
    /// Filter rows by condition.
    /// <param name="filter">Condition for filtering.</param>
    /// </summary>
    public virtual void Filter(Func<MochaRow,bool> filter) =>
      Rows = Rows.Where(filter).ToArray();

    /// <summary>
    /// Filter columns by condition.
    /// <param name="filter">Condition for filtering.</param>
    /// </summary>
    public virtual void Filter(Func<MochaColumn,bool> filter) =>
      Columns = Columns.Where(filter).ToArray();

    #endregion Members

    #region Properties

    /// <summary>
    /// Columns.
    /// </summary>
    public virtual MochaColumn[] Columns { get; internal protected set; }

    /// <summary>
    /// Rows.
    /// </summary>
    public virtual MochaRow[] Rows { get; internal protected set; }

    #endregion Properties
  }
}
