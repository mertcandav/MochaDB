using System;

namespace MochaDB {
    /// <summary>
    /// StackItem interface for MochaDB stack items.
    /// </summary>
    public interface IMochaStackItem {
        #region Events

        event EventHandler<EventArgs> NameChanged;

        #endregion

        #region Properties

        string Name { get; set; }
        string Value { get; set; }
        string Description { get; set; }
        MochaStackItemCollection Items { get; }

        #endregion
    }
}
