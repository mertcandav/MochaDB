namespace MochaDB.Mhql {
    /// <summary>
    /// Interface for MochaDB Mhql commands.
    /// </summary>
    public interface IMhqlCommand {
        #region Methods

        bool IsExecuteCompatible();
        bool IsReaderCompatible();
        bool IsScalarCompatible();

        #endregion

        #region Properties

        string Command { get; set; }

        #endregion
    }
}
