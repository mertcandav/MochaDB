namespace MochaDB.Connection {
    /// <summary>
    /// Provider attribute interface for MochaDB provider attributes.
    /// </summary>
    public interface IMochaProviderAttribute {
        #region Properties

        string Name { get; set; }
        string Value { get; set; }

        #endregion
    }
}
