namespace MochaDB.Mochaq {
    /// <summary>
    /// Interface for MochaDB query handlers.
    /// </summary>
    public interface IMochaQuery {
        #region Methods

        IMochaResult Dynamic();
        void Run();
        IMochaResult GetRun();

        #endregion
    }
}
