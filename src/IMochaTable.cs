using System;

namespace MochaDB {
    /// <summary>
    /// Table interface for MochaDB tables.
    /// </summary>
    public interface IMochaTable:IMochaDatabaseItem {
        #region Events

        event EventHandler<EventArgs> NameChanged;

        #endregion

        #region Methods

        void ShortDatas(int index);
        void ShortColumns();

        #endregion

        #region Properties

        MochaColumnCollection Columns { get; }
        MochaRowCollection Rows { get; }

        #endregion
    }
}
