using System.Collections.Generic;

namespace MochaDB.Streams {
    /// <summary>
    /// Stream reader for MochaDB.
    /// </summary>
    /// <typeparam name="T">Value type of stream reader.</typeparam>
    public class MochaReader<T>:IMochaReader<T> {
        #region Fields

        private List<T> collection;

        #endregion

        #region Constructors

        /// <summary>
        /// Create a new MochaReader.
        /// </summary>
        /// <param name="values">Values of stream.</param>
        public MochaReader(params T[] values) {
            collection = new List<T>(values);
            Value=null;
            Position=-1;
        }

        /// <summary>
        /// Create a new MochaReader.
        /// </summary>
        /// <param name="values">Values of stream.</param>
        public MochaReader(IEnumerable<T> values) {
            collection = new List<T>(values);
            Value=null;
            Position=-1;
        }

        #endregion

        #region Operators

        public static explicit operator string(MochaReader<T> value) =>
            value.ToString();

        #endregion

        #region Methods

        /// <summary>
        /// Returns true if value is exists in next position but returns if not.
        /// </summary>
        public bool Read() {
            if(Position+1 < Count) {
                Position++;
                Value = collection[Position];
                return true;
            }

            Value=null;
            return false;
        }

        /// <summary>
        /// Go to previous position.
        /// </summary>
        public void GoBack() {
            if(Position!=-1)
                Position--;
        }

        /// <summary>
        /// Go to first position.
        /// </summary>
        public void GoFirst() =>
            Position=-1;

        /// <summary>
        /// Go to last position.
        /// </summary>
        public void GoLast() =>
            Position=Count-2 < -1 ? -1 : Count-2;

        #endregion

        #region Overrides

        /// <summary>
        /// Returns converted to string result of <see cref="Value"/>.
        /// </summary>
        public override string ToString() {
            return Value.ToString();
        }

        #endregion

        #region Properties

        /// <summary>
        /// Current value.
        /// </summary>
        public object Value { get; internal set; }

        /// <summary>
        /// Current value index of reader.
        /// </summary>
        public int Position { get; internal set; }

        /// <summary>
        /// Count of value.
        /// </summary>
        public int Count =>
            collection.Count;

        #endregion
    }
}
