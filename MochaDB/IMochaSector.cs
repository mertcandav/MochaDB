namespace MochaDB {
    /// <summary>
    /// Sector interface for MochaDB sectors.
    /// </summary>
    public interface IMochaSector {
        #region Properties

        public string Name { get; set; }
        public string Data { get; set; }
        public string Description { get; set; }

        #endregion
    }
}
