using System.Collections.Generic;

namespace MochaDB {
    /// <summary>
    /// This is row object for MochaDB.
    /// </summary>
    public class MochaRow {
        #region Construcors

        /// <summary>
        /// Create new MochaRow.
        /// </summary>
        public MochaRow() {
            Datas = new MochaData[0];
        }

        /// <summary>
        /// Create new MochaRow.
        /// </summary>
        /// <param name="datas">Datas of row.</param>
        public MochaRow(IList<MochaData> datas) {
            Datas = datas;
        }

        #endregion

        #region Property

        /// <summary>
        /// All datas.
        /// </summary>
        public IList<MochaData> Datas { get; set; }

        /// <summary>
        /// Count of data.
        /// </summary>
        public int DataCount =>
            Datas.Count;

        #endregion
    }
}
