namespace MochaDB {
    /// <summary>
    /// Interface for MochaDB elements.
    /// </summary>
    public interface IMochaElement {
        #region Properties

        string Name { get; }
        string Description { get; }
        string Value { get; }

        #endregion
    }
}
