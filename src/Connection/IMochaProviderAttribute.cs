namespace MochaDB.Connection {
    /// <summary>
    /// Provider attribute interface for MochaDB provider attributes.
    /// </summary>
    public interface IMochaProviderAttribute:IMochaAttribute {
        #region Methods

        string GetProviderString();

        #endregion
    }
}
