using MochaDB.Collections;

namespace MochaDB {
    /// <summary>
    /// Table interface for MochaDB tables.
    /// </summary>
    public interface IMochaTable {
        #region Methods

        public void ShortDatas(int index);
        public void ShortColumns();

        #endregion

        #region Properties

        public string Name { get; set; }
        public string Description { get; set; }
        public MochaColumnCollection Columns { get; }
        public MochaRowCollection Rows { get; }

        #endregion
    }
}
