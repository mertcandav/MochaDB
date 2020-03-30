using MochaDB.Streams;

namespace MochaDB.Mhql {
    /// <summary>
    /// Interface for MHQL commands processors.
    /// </summary>
    public interface IMochaDbCommand {
        #region Methods

        //void ExecuteCommand();
        object ExecuteScalar();
        MochaReader<object> ExecuteReader();

        #endregion

        #region Properties

        MhqlCommand Command { get; set; }

        #endregion
    }
}
