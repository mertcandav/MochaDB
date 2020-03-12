using System.Collections.Generic;

namespace MochaDB {
    /// <summary>
    /// This is row object for MochaDB.
    /// </summary>
    public class MochaRow:IMochaRow {
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
        public MochaRow(params object[] datas) :
            this() {
            MochaData[] coll = new MochaData[datas.Length];
            for(int index = 0; index < datas.Length; index++) {
                object data = datas[index]==null ? string.Empty : datas[index];
                coll[index] = new MochaData(MochaData.GetDataTypeFromType(data.GetType()),data);
            }
            Datas.collection.AddRange(coll);
        }

        /// <summary>
        /// Create new MochaRow.
        /// </summary>
        /// <param name="datas">Datas of row.</param>
        public MochaRow(params MochaData[] datas) :
            this() {
            Datas.collection.AddRange(datas);
        }

        /// <summary>
        /// Create new MochaRow.
        /// </summary>
        /// <param name="datas">Datas of row.</param>
        public MochaRow(IEnumerable<MochaData> datas)
            : this() {
            Datas.collection.AddRange(datas);
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
