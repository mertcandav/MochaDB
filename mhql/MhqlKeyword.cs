namespace MochaDB.mhqlcore {
    /// <summary>
    /// Base class for Mhql keywords.
    /// </summary>
    internal abstract class MhqlKeyword:IMhqlKeyword {
        #region Properties

        public MochaDatabase Tdb { get; set; }
        public string Command { get; set; }

        #endregion
    }
}
