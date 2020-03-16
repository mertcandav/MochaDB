using System;

namespace MochaDB.Logging {
    /// <summary>
    /// Interface for MochaDB logs.
    /// </summary>
    public interface IMochaLog {
        #region Properties

        string Log { get; set; }
        DateTime Time { get; set; }

        #endregion
    }
}
