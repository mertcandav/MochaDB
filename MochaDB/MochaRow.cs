using System.Collections.Generic;

namespace MochaDB {
    /// <summary>
    /// This is row object for MochaDB.
    /// </summary>
    public sealed class MochaRow {
        #region Construcors

        /// <summary>
        /// Create new MochaRow.
        /// </summary>
        public MochaRow() {
            Datas = new MochaDataCollection();
        }

        /// <summary>
        /// Create new MochaRow.
        /// </summary>
        /// <param name="datas">Datas of row.</param>
        public MochaRow(IEnumerable<MochaData> datas)
            : this() {
            Datas.AddRange(datas);
        }

        #endregion

        #region Properties

        /// <summary>
        /// Datas of row.
        /// </summary>
        public MochaDataCollection Datas { get; }

        #endregion
    }
}
