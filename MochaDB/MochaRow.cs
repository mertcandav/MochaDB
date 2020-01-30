using System;
using System.Collections.Generic;

namespace MochaDB {
    /// <summary>
    /// This is row object for MochaDB.
    /// </summary>
    public class MochaRow {
        #region Fields

        private List<MochaData> datas;

        #endregion

        #region Construcors

        /// <summary>
        /// Create new MochaRow.
        /// </summary>
        public MochaRow() {
            datas = new List<MochaData>();
        }

        /// <summary>
        /// Create new MochaRow.
        /// </summary>
        /// <param name="datas">Datas of row.</param>
        public MochaRow(IList<MochaData> datas)
            : this() {
            this.datas.AddRange(datas);
        }

        #endregion

        #region Events

        /// <summary>
        /// This happends after data changed.
        /// </summary>
        internal event EventHandler<EventArgs> Changed;

        #endregion

        #region Data

        /// <summary>
        /// Add data.
        /// </summary>
        /// <param name="data">MochaData to add.</param>
        public void AddData(MochaData data) {
            datas.Add(data);
            Changed?.Invoke(this,new EventArgs());
        }

        /// <summary>
        /// Add datas from array.
        /// </summary>
        /// <param name="datas">Container array of datas.</param>
        public void AddDataRange(IList<MochaData> datas) {
            this.datas.AddRange(datas);
            Changed?.Invoke(this,new EventArgs());
        }

        /// <summary>
        /// Remove data by index.
        /// </summary>
        /// <param name="data">Index of remove.</param>
        public void RemoveDataAt(int index) {
            datas.RemoveAt(index);
            Changed?.Invoke(this,new EventArgs());
        }

        /// <summary>
        /// Remove all datas.
        /// </summary>
        public void ClearDatas() {
            datas.Clear();
            Changed?.Invoke(this,new EventArgs());
        }

        #endregion

        #region Properties

        /// <summary>
        /// All datas.
        /// </summary>
        public IList<MochaData> Datas =>
            datas;

        /// <summary>
        /// Count of data.
        /// </summary>
        public int DataCount =>
            Datas.Count;

        #endregion
    }
}
