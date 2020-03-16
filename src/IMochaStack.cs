using System;

namespace MochaDB {
    /// <summary>
    /// Stack interface for MochaDB stacks.
    /// </summary>
    public interface IMochaStack {
        #region Events

        event EventHandler<EventArgs> NameChanged;

        #endregion

        #region Properties

        string Name { get; set; }
        string Description { get; set; }
        MochaStackItemCollection Items { get; }

        #endregion
    }
}
