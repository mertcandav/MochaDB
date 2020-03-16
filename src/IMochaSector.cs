using System;

namespace MochaDB {
    /// <summary>
    /// Sector interface for MochaDB sectors.
    /// </summary>
    public interface IMochaSector {
        #region Events

        event EventHandler<EventArgs> NameChanged;

        #endregion

        #region Properties

        string Name { get; set; }
        string Data { get; set; }
        string Description { get; set; }

        #endregion
    }
}
