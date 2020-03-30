namespace MochaDB {
    /// <summary>
    /// Interface for MochaDB database data items.
    /// </summary>
    public interface IMochaDatabaseItem {
        #region Properties

        string Name { get; set; }
        string Description { get; set; }

        #endregion
    }
}
