using MochaDB.Collections;

namespace MochaDB {
    /// <summary>
    /// Row interface for MochaDB rows.
    /// </summary>
    public interface IMochaRow {
        #region Properties

        MochaDataCollection Datas { get; }

        #endregion
    }
}
