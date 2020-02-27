using System;

namespace MochaDB.Dynamic {
    /// <summary>
    /// Interface for MochaDB attributes.
    /// </summary>
    public interface IMochaAttribute {
        #region Events

        event EventHandler<EventArgs> NameChanged;
        event EventHandler<EventArgs> ValueChanged;

        #endregion

        #region Properties

        string Name { get; set; }
        string Value { get; set; }

        #endregion
    }
}