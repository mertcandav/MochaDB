namespace MochaDB.Querying {
    /// <summary>
    /// Interface for MochaDB query handlers.
    /// </summary>
    public interface IMochaQuery {
        #region Methods

        public object Dynamic();
        public void Run();
        public object GetRun();

        #endregion

        #region Properties

        public string MochaQ { get; set; }

        #endregion
    }
}
