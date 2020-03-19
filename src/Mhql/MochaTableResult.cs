namespace MochaDB.Mhql {
    /// <summary>
    /// Table result for Mhql query results.
    /// </summary>
    public class MochaTableResult {
        #region Constructors

        /// <summary>
        /// Create a new MochaTableResult.
        /// </summary>
        internal MochaTableResult() {

        }

        #endregion

        #region Internal Methods

        /// <summary>
        /// Set rows by datas of columns.
        /// </summary>
        internal void SetRowsByDatas() {
            if(Columns.Length > 0 && Columns[0].Datas.Count > 0) {
                var firstcolumn = Columns[0];
                MochaArray<MochaRow> rows = new MochaRow[firstcolumn.Datas.Count];
                //Process rows.
                for(var dataindex = 0; dataindex < firstcolumn.Datas.Count; dataindex++) {
                    MochaArray<MochaData> datas = new MochaArray<MochaData>(Columns.Length);
                    for(var columnindex = 0; columnindex < Columns.Length; columnindex++) {
                        var column = Columns[columnindex];
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

        #endregion

        #region Properties

        /// <summary>
        /// Columns.
        /// </summary>
        public MochaArray<MochaColumn> Columns { get; internal set; }

        /// <summary>
        /// Rows.
        /// </summary>
        public MochaArray<MochaRow> Rows { get; internal set; }

        #endregion
    }
}
