using MochaDB.Collections;

namespace MochaDB {
    /// <summary>
    /// Table interface for MochaDB tables.
    /// </summary>
    public interface IMochaTable {
        #region Methods

        void ShortDatas(int index);
        void ShortColumns();

        #endregion

        #region Properties

        string Name { get; set; }
        string Description { get; set; }
        MochaColumnCollection Columns { get; }
        MochaRowCollection Rows { get; }

        #endregion
    }
}
