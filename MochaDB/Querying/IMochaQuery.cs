namespace MochaDB.Querying {
    /// <summary>
    /// Interface for MochaDB query handlers.
    /// </summary>
    public interface IMochaQuery {
        #region Methods

        public IMochaResult Dynamic();
        public void Run();
        public IMochaResult GetRun();

        #endregion

        #region Properties

        public string MochaQ { get; set; }

        #endregion
    }
}
