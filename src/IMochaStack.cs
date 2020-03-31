using System;

namespace MochaDB {
    /// <summary>
    /// Stack interface for MochaDB stacks.
    /// </summary>
    public interface IMochaStack:IMochaDatabaseItem {
        #region Events

        event EventHandler<EventArgs> NameChanged;

        #endregion

        #region Properties

        MochaAttributeCollection Attributes { get; }
        MochaStackItemCollection Items { get; }

        #endregion
    }
}
