namespace MochaDB.mhql {
    /// <summary>
    /// Base class for Mhql keywords.
    /// </summary>
    internal class MhqlKeyword:IMhqlKeyword {
        #region Properties

        public MochaDatabase Tdb { get; set; }
        public string Command { get; set; }

        #endregion
    }
}
