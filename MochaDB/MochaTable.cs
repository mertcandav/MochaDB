using System;

namespace MochaDB {
    /// <summary>
    /// This is table object for MochaDB.
    /// </summary>
    public sealed class MochaTable {
        #region Constructors

        /// <summary>
        /// Create new MochaTable.
        /// </summary>
        /// <param name="name">Name of table.</param>
        public MochaTable(string name) {
            Name = name;
            Description = string.Empty;
            Columns = new MochaColumnCollection();
            Columns.ColumnChanged+=Column_Changed;
            Rows = new MochaRowCollection();
            Rows.RowChanged+=Row_Changed;
        }

        /// <summary>
        /// Create new MochaTable.
        /// </summary>
        /// <param name="name">Name of table.</param>
        /// <param name="description">Description of table.</param>
        public MochaTable(string name,string description) :
            this(name) {
            Description=description;
        }

        #endregion

        #region Content Events

        private void Column_Changed(object sender,EventArgs e) {
            SetRowsByDatas();
        }

        private void Row_Changed(object sender,EventArgs e) {
            SetDatasByRows();
        }

        #endregion

        #region Methods

        #region Internal

        /// <summary>
        /// Set datas by row datas.
        /// </summary>
        internal void SetDatasByRows() {
            for(int columnIndex = 0; columnIndex < Columns.Count; columnIndex++) {
                Columns[columnIndex].Datas.Changed-=Column_Changed;
                Columns[columnIndex].Datas.Clear();
            }

            if(Rows.Count == 0)
                return;

            for(int rowIndex = 0; rowIndex < Rows.Count; rowIndex++) {
                if(Rows[rowIndex].Datas.Count!=Columns.Count)
                    throw new Exception("The number of data must be equal to the number of columns!");

                for(int columnIndex = 0; columnIndex < Columns.Count; columnIndex++) {
                    if(Columns[columnIndex].DataType!=MochaDataType.AutoInt)
                        Columns[columnIndex].Datas.Add(Rows[rowIndex].Datas[columnIndex]);
                    else {
                        if(Columns[columnIndex].Datas.Count>0) {
                            MochaData data = new MochaData() {
                                data=1 + (int)Columns[columnIndex].Datas[^1].Data,
                                dataType=MochaDataType.AutoInt
                            };
                            Columns[columnIndex].Datas.Add(data);
                            Rows[rowIndex].Datas[columnIndex]= data;
                        } else {
                            MochaData data = new MochaData() { data=1,dataType=MochaDataType.AutoInt };
                            Columns[columnIndex].Datas.Add(data);
                            Rows[rowIndex].Datas[columnIndex] = data;
                        }
                    }
                }
            }

            for(int columnIndex = 0; columnIndex < Columns.Count; columnIndex++) {
                Columns[columnIndex].Datas.Changed+=Column_Changed;
            }
        }

        /// <summary>
        /// Set rows by column datas.
        /// </summary>
        internal void SetRowsByDatas() {
            Rows.Clear();

            if(Columns.Count == 0)
                return;

            MochaData[] datas;
            for(int dataIndex = 0; dataIndex < Columns[0].Datas.Count; dataIndex++) {
                datas = new MochaData[Columns.Count];

                for(int columnIndex = 0; columnIndex < Columns.Count; columnIndex++) {
                    datas[columnIndex] = Columns[columnIndex].Datas[dataIndex];
                }

                Rows.Add(new MochaRow(datas));
            }
        }

        #endregion

        /// <summary>
        /// Short datas.
        /// </summary>
        /// <param name="index">Index of column.</param>
        public void ShortDatas(int index) {
            SetRowsByDatas();
            Rows.collection.Sort((X,Y) => X.Datas[index].ToString().CompareTo(Y.Datas[index].ToString()));
            SetDatasByRows();
        }

        /// <summary>
        /// Sort columns by name.
        /// </summary>
        public void ShortColumns() {
            Columns.collection.Sort((X,Y) => X.Name.CompareTo(Y.Name));
            SetRowsByDatas();
        }

        #endregion

        #region Properties

        /// <summary>
        /// Name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Description.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Columns of table.
        /// </summary>
        public MochaColumnCollection Columns { get; }

        /// <summary>
        /// Rows of table.
        /// </summary>
        public MochaRowCollection Rows { get; }

        #endregion
    }
}
