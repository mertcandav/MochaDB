namespace MochaDB.Connection {
    /// <summary>
    /// Provider attribute interface for MochaDB provider attributes.
    /// </summary>
    public interface IMochaProviderAttribute {
        #region Properties

        public string Name { get; set; }
        public string Value { get; set; }

        #endregion
    }
}
