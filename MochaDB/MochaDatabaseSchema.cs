using MochaDB.Streams;

namespace MochaDB {
    /// <summary>
    /// Database schema for MochaDB.
    /// </summary>
    public class MochaDatabaseSchema:IMochaDatabaseSchema {
        #region Constructors

        /// <summary>
        /// Create a new MochaDatabaseSchema.
        /// </summary>
        public MochaDatabaseSchema() {
            Sectors = new MochaSectorCollection();
            Stacks = new MochaStackCollection();
            Tables = new MochaTableCollection();
        }

        #endregion

        #region Methods

        /// <summary>
        /// Returns sectors in MochaReader.
        /// </summary>
        public MochaReader<MochaSector> SectorsToRead() =>
            new MochaReader<MochaSector>(Sectors);

        /// <summary>
        /// Returns stacks in MochaReader.
        /// </summary>
        public MochaReader<MochaStack> StacksToRead() =>
            new MochaReader<MochaStack>(Stacks);

        /// <summary>
        /// Returns tables in MochaReader.
        /// </summary>
        public MochaReader<MochaTable> TablesToRead() =>
            new MochaReader<MochaTable>(Tables);

        #endregion

        #region Properties

        /// <summary>
        /// Sectors of database.
        /// </summary>
        public MochaSectorCollection Sectors { get; }

        /// <summary>
        /// Stack of database.
        /// </summary>
        public MochaStackCollection Stacks { get; }

        /// <summary>
        /// Tables of database.
        /// </summary>
        public MochaTableCollection Tables { get; }

        #endregion
    }
}
