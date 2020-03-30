namespace MochaDB.Mochaq {
    /// <summary>
    /// Interface for MochaQ commands.
    /// </summary>
    public interface IMochaQCommand {
        #region Methods

        bool IsDynamicQuery();
        bool IsGetRunQuery();
        bool IsRunQuery();

        #endregion

        #region Properties

        string Command { get; set; }

        #endregion
    }
}
