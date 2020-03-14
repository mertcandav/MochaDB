namespace MochaDB {
    /// <summary>
    /// Interface for MochaDB database schemas.
    /// </summary>    
    public interface IMochaDatabaseSchema {
        #region Properties

        MochaSectorCollection Sectors { get; }
        MochaStackCollection Stacks { get; }
        MochaTableCollection Tables { get; }

        #endregion
    }
}
