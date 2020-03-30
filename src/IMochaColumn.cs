using System;

namespace MochaDB {
    /// <summary>
    /// Column interface for MochaDB columns.
    /// </summary>
    public interface IMochaColumn:IMochaDatabaseItem {
        #region Events

        event EventHandler<EventArgs> NameChanged;

        #endregion

        #region Properties

        MochaColumnDataCollection Datas { get; }
        MochaDataType DataType { get; set; }

        #endregion
    }
}
