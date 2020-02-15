using MochaDB.Collections;

namespace MochaDB {
    /// <summary>
    /// Column interface for MochaDB columns.
    /// </summary>
    public interface IMochaColumn {
        #region Properties

        public string Name { get; set; }
        public string Description { get; set; }
        public MochaColumnDataCollection Datas { get; }
        public MochaDataType DataType { get; set; }

        #endregion
    }
}
