using MochaDB.Collections;
using System;

namespace MochaDB {
    /// <summary>
    /// StackItem interface for MochaDB stack items.
    /// </summary>
    public interface IMochaStackItem {
        #region Events

        public event EventHandler<EventArgs> NameChanged;

        #endregion

        #region Properties

        public string Name { get; set; }
        public string Value { get; set; }
        public string Description { get; set; }
        public MochaStackItemCollection Items { get; }

        #endregion
    }
}
