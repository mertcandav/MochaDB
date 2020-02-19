namespace MochaDB.Connection {
    /// <summary>
    /// Provider interface for MochaDB providers.
    /// </summary>
    public interface IMochaProvider {
        #region Methods

        public void EnableConstant();
        public MochaProviderAttribute GetAttribute(string name);

        #endregion

        #region Properties

        public string ConnectionString { get; set; }
        public string Path { get; }
        public string Password { get; }
        public bool Readonly { get; }
        public bool Constant { get; }

        #endregion
    }
}
