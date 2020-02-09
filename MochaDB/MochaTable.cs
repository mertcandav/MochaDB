using System;
using System.Collections.Generic;

namespace MochaDB {
    /// <summary>
    /// This is table object for MochaDB.
    /// </summary>
    public sealed class MochaTable {
        #region Fields

        internal List<MochaColumn> columns;
        internal List<MochaRow> rows;

        #endregion

        #region Constructors

        /// <summary>
        /// Create new MochaTable.
        /// </summary>
        /// <param name="name">Name of table.</param>
        public MochaTable(string name) {
            Name = name;
            Description = string.Empty;
            columns = new List<MochaColumn>();
            rows = new List<MochaRow>();
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

        /// <summary>
        /// Set datas by row datas.
        /// </summary>
        internal void SetDatasByRows() {
            for(int columnIndex = 0; columnIndex < ColumnCount; columnIndex++) {
                columns[columnIndex].Changed-=Column_Changed;
                columns[columnIndex].ClearDatas();
            }

            if(RowCount == 0)
                return;

            for(int rowIndex = 0; rowIndex < RowCount; rowIndex++) {
                if(rows[rowIndex].DataCount!=ColumnCount)
                    throw new Exception("The number of data must be equal to the number of columns!");

                for(int columnIndex = 0; columnIndex < ColumnCount; columnIndex++) {
                    if(columns[columnIndex].DataType!=MochaDataType.AutoInt)
                        columns[columnIndex].AddData(rows[rowIndex].Datas[columnIndex]);
                    else {
                        if(columns[columnIndex].DataCount>0) {
                            MochaData data = new MochaData() {
                                data=1 + (int)columns[columnIndex].Datas[^1].Data,
                                dataType=MochaDataType.AutoInt
                            };
                            columns[columnIndex].datas.Add(data);
                            rows[rowIndex].datas[columnIndex]= data;
                        } else {
                            MochaData data = new MochaData() { data=1,dataType=MochaDataType.AutoInt };
                            columns[columnIndex].datas.Add(data);
                            rows[rowIndex].datas[columnIndex] = data;
                        }
                    }
                }
            }

            for(int columnIndex = 0; columnIndex < ColumnCount; columnIndex++) {
                columns[columnIndex].Changed+=Column_Changed;
            }
        }

        /// <summary>
        /// Set rows by column datas.
        /// </summary>
        internal void SetRowsByDatas() {
            rows.Clear();

            if(ColumnCount == 0)
                return;

            MochaData[] datas;
            for(int dataIndex = 0; dataIndex < columns[0].DataCount; dataIndex++) {
                datas = new MochaData[ColumnCount];

                for(int columnIndex = 0; columnIndex < ColumnCount; columnIndex++) {
                    datas[columnIndex] = columns[columnIndex].Datas[dataIndex];
                }

                rows.Add(new MochaRow(datas));
            }
        }

        /// <summary>
        /// Short datas.
        /// </summary>
        /// <param name="index">Index of column.</param>
        public void ShortDatas(int index) {
            SetRowsByDatas();
            rows.Sort((X,Y) => X.Datas[index].ToString().CompareTo(Y.Datas[index].ToString()));
            SetDatasByRows();
        }

        /// <summary>
        /// Sort columns by name.
        /// </summary>
        public void ShortColumns() {
            columns.Sort((X,Y) => X.Name.CompareTo(Y.Name));
            SetRowsByDatas();
        }

        #endregion

        #region Column

        /// <summary>
        /// Add column to table.
        /// </summary>
        /// <param name="column">MochaColumn object to add.</param>
        public void AddColumn(MochaColumn column) {
            if(column == null)
                return;

            if(!ExistsColumn(column.Name)) {
                column.Changed+=Column_Changed;
                columns.Add(column);
                SetRowsByDatas();
            } else
                throw new Exception("There is no such table or there is already a table with this name!");
        }

        /// <summary>
        /// Add column to table.
        /// </summary>
        /// <param name="dataType">Data type of column.</param>
        /// <param name="name">Name of column.</param>
        public void AddColumn(MochaDataType dataType,string name) {
            AddColumn(new MochaColumn(name,dataType));
        }

        /// <summary>
        /// Remove column from table.
        /// </summary>
        /// <param name="name">Name of column to remove.</param>
        public void RemoveColumn(string name) {
            for(int index = 0; index < columns.Count; index++)
                if(columns[index].Name == name) {
                    columns[index].Changed-=Column_Changed;
                    columns.RemoveAt(index);
                    break;
                }
            SetRowsByDatas();
        }

        /// <summary>
        /// Remove all columns.
        /// </summary>
        public void ClearColumns() {
            for(int index = 0; index < columns.Count; index++) {
                columns[index].Changed-=Column_Changed;
                columns.RemoveAt(index);
            }

            SetRowsByDatas();
        }

        /// <summary>
        /// Returns whether there is a column with the specified name.
        /// </summary>
        /// <param name="name">Name of column to check.</param>
        public bool ExistsColumn(string name) {
            for(int index = 0; index < columns.Count; index++)
                if(columns[index].Name == name)
                    return true;
            return false;
        }

        #endregion

        #region Row

        /// <summary>
        /// Add row to table.
        /// </summary>
        /// <param name="row">MochaRow object to add.</param>
        public void AddRow(MochaRow row) {
            if(row == null)
                return;

            if(row.DataCount != ColumnCount)
                throw new Exception("The number of data must be equal to the number of columns!");

            rows.Add(row);
            row.Changed+=Row_Changed;
            SetDatasByRows();
        }

        /// <summary>
        /// Remove row from table.
        /// </summary>
        /// <param name="Data">Index of row to remove.</param>
        public void RemoveRow(int index) {
            if(!ExistsRow(index))
                return;

            rows[index].Changed-=Row_Changed;
            rows.RemoveAt(index);
            SetDatasByRows();
        }

        /// <summary>
        /// Remove all rows.
        /// </summary>
        public void ClearRows() {
            for(int index = 0; index < rows.Count; index++) {
                rows[index].Changed-=Row_Changed;
                rows.RemoveAt(index);
            }

            SetDatasByRows();
        }

        /// <summary>
        /// Returns whether there is a row with the specified index.
        /// </summary>
        /// <param name="index">Index of row.</param>
        public bool ExistsRow(int index) {
            if(rows.Count >= index)
                return true;
            else
                return false;
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
        /// All columns.
        /// </summary>
        public MochaColumn[] Columns =>
            columns.ToArray();

        /// <summary>
        /// Count of column.
        /// </summary>
        public int ColumnCount =>
            columns.Count;

        /// <summary>
        /// All rows.
        /// </summary>
        public MochaRow[] Rows =>
            rows.ToArray();

        /// <summary>
        /// Count of row.
        /// </summary>
        public int RowCount =>
            rows.Count;

        #endregion
    }
}
