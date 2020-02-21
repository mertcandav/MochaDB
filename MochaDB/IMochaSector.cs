namespace MochaDB {
    /// <summary>
    /// Sector interface for MochaDB sectors.
    /// </summary>
    public interface IMochaSector {
        #region Properties

        string Name { get; set; }
        string Data { get; set; }
        string Description { get; set; }

        #endregion
    }
}
