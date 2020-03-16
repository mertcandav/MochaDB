namespace MochaDB.Connection {
    /// <summary>
    /// Provider interface for MochaDB providers.
    /// </summary>
    public interface IMochaProvider {
        #region Methods

        void EnableConstant();
        MochaProviderAttribute GetAttribute(string name);

        #endregion

        #region Properties

        string ConnectionString { get; set; }
        string Path { get; }
        string Password { get; }
        bool Readonly { get; }
        bool Constant { get; }

        #endregion
    }
}
