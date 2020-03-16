using System;

namespace MochaDB {
    /// <summary>
    /// Column interface for MochaDB columns.
    /// </summary>
    public interface IMochaColumn {
        #region Events

        event EventHandler<EventArgs> NameChanged;

        #endregion

        #region Properties

        string Name { get; set; }
        string Description { get; set; }
        MochaColumnDataCollection Datas { get; }
        MochaDataType DataType { get; set; }

        #endregion
    }
}
