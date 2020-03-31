using System;

namespace MochaDB {
    /// <summary>
    /// Sector interface for MochaDB sectors.
    /// </summary>
    public interface IMochaSector:IMochaDatabaseItem {
        #region Events

        event EventHandler<EventArgs> NameChanged;

        #endregion

        #region Properties

        string Data { get; set; }
        MochaAttributeCollection Attributes { get; }

        #endregion
    }
}
