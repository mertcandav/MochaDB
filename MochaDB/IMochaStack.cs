using MochaDB.Collections;

namespace MochaDB {
    /// <summary>
    /// Stack interface for MochaDB stacks.
    /// </summary>
    public interface IMochaStack {
        #region Properties

        public string Name { get; set; }
        public string Description { get; set; }
        public MochaStackItemCollection Items { get; }

        #endregion
    }
}
