using System;
using System.Collections.Generic;

namespace MochaDB {
    /// <summary>
    /// This is table object for MochaDB.
    /// </summary>
    public class MochaTable {
        #region Fields

        private List<MochaColumn> columns;
        private List<MochaRow> rows;

        #endregion

        #region Constructors

        /// <summary>
        /// Create new MochaTable.
        /// </summary>
        /// <param name="name">Name of table.</param>
        public MochaTable(string name) {
            Name = name;
            columns = new List<MochaColumn>();
            rows = new List<MochaRow>();
        }

        /// <summary>
        /// Create new MochaTable.
        /// </summary>
        /// <param name="name">Name of table.</param>
        /// <param name="columns">Columns of table.</param>
        /// <param name="rows">Rows of table.</param>
        public MochaTable(string name,IEnumerable<MochaColumn> columns,IEnumerable<MochaRow> rows)
            : this(name) {
            this.columns.AddRange(columns);
            this.rows.AddRange(rows);
        }

        #endregion

        #region Methods

        /// <summary>
        /// Set datas by row datas.
        /// </summary>
        internal void SetDatasByRows() {
            for(int columnIndex = 0; columnIndex < ColumnCount; columnIndex++) {
                columns[columnIndex].ClearDatas();
            }

            if(RowCount == 0)
                return;

            for(int rowIndex = 0; rowIndex < RowCount; rowIndex++) {
                for(int columnIndex = 0; columnIndex < ColumnCount; columnIndex++) {
                    columns[columnIndex].AddData(rows[rowIndex].Datas[columnIndex]);
                }
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

        #endregion

        #region Column

        /// <summary>
        /// Add column to table.
        /// </summary>
        /// <param name="column">MochaColumn object to add.</param>
        public void AddColumn(MochaColumn column) {
            if(column == null)
                return;

            if(!ExistColumn(column.Name)) {
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
            IList<MochaColumn> columns = Columns;
            for(int index = 0; index < columns.Count; index++)
                if(columns[index].Name == name) {
                    this.columns.RemoveAt(index);
                    break;
                }
            SetRowsByDatas();
        }

        /// <summary>
        /// Remove all columns.
        /// </summary>
        public void ClearColumns() {
            columns.Clear();
            SetRowsByDatas();
        }

        /// <summary>
        /// Returns whether there is a column with the specified name.
        /// </summary>
        /// <param name="name">Name of column to check.</param>
        public bool ExistColumn(string name) {
            IList<MochaColumn> columns = Columns;
            for(int index = 0; index < columns.Count; index++)
                if(columns[index].Name == name)
                    return true;
            return false;
        }

        /// <summary>
        /// Sort columns by name.
        /// </summary>
        public void ShortColumns() {
            columns.Sort((X,Y) => X.Name.CompareTo(Y.Name));
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

            rows.Add(row);
            SetDatasByRows();
        }

        /// <summary>
        /// Remove row from table.
        /// </summary>
        /// <param name="Data">Index of row to remove.</param>
        public void RemoveRow(int index) {
            if(!ExistRow(index))
                return;

            rows.RemoveAt(index);
            SetDatasByRows();
        }

        /// <summary>
        /// Remove all rows.
        /// </summary>
        public void ClearRows() {
            rows.Clear();
            SetDatasByRows();
        }

        /// <summary>
        /// Returns whether there is a row with the specified index.
        /// </summary>
        /// <param name="index">Index of row.</param>
        public bool ExistRow(int index) {
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
        /// All columns.
        /// </summary>
        public IList<MochaColumn> Columns =>
            columns;

        /// <summary>
        /// Count of column.
        /// </summary>
        public int ColumnCount =>
            columns.Count;

        /// <summary>
        /// All rows.
        /// </summary>
        public IList<MochaRow> Rows =>
            rows;

        /// <summary>
        /// Count of row.
        /// </summary>
        public int RowCount =>
            rows.Count;

        #endregion
    }
}
