namespace MochaDB.Querying {
    /// <summary>
    /// Interface for MochaQ commands.
    /// </summary>
    public interface IMochaQCommand {
        #region Methods

        public bool IsDynamicQuery();
        public bool IsGetRunQuery();
        public bool IsRunQuery();

        #endregion

        #region Properties

        public string Command { get; set; }

        #endregion
    }
}