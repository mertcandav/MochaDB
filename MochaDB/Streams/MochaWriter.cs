using System.Collections.Generic;
using System.Linq;

namespace MochaDB.Streams {
    /// <summary>
    /// Writer for MochaDB.
    /// </summary>
    /// <typeparam name="T">Value type of writer.</typeparam>
    public class MochaWriter<T>:IMochaWriter<T> {
        #region Fields

        private List<T> collection;

        #endregion

        #region Constructors

        /// <summary>
        /// Create a new MochaWriter.
        /// </summary>
        public MochaWriter() {
            collection = new List<T>();
        }

        /// <summary>
        /// Create a new MochaWriter.
        /// </summary>
        /// <param name="values">Values of writer.</param>
        public MochaWriter(params T[] values) :
            this() {
            collection.AddRange(values);
        }

        /// <summary>
        /// Create a new MochaWriter.
        /// </summary>
        /// <param name="values">Values of writer.</param>
        public MochaWriter(IEnumerable<T> values) :
            this() {
            collection.AddRange(values);
        }

        #endregion
        
        #region Static

        /// <summary>
        /// Write to destination collection from source collection.
        /// </summary>
        /// <param name="source">Source collection.</param>
        /// <param name="destination">Destination collection.</param>
        public static void Write(IEnumerable<T> source,IMochaCollection<T> destination) {
            for(int index = 0; index < source.Count(); index++)
                destination.Add(source.ElementAt(index));
        }

        /// <summary>
        /// Write to destination collection from source collection.
        /// </summary>
        /// <param name="source">Source collection.</param>
        /// <param name="destination">Destination collection.</param>
        /// <param name="start">Start index to write.</param>
        public static void Write(IEnumerable<T> source,IMochaCollection<T> destination,int start) {
            for(int index = start; index < source.Count(); index++)
                destination.Add(source.ElementAt(index));
        }

        /// <summary>
        /// Write to destination collection from source collection.
        /// </summary>
        /// <param name="source">Source collection.</param>
        /// <param name="destination">Destination collection.</param>
        /// <param name="start">Start index to write.</param>
        /// <param name="count">Count of item to write.</param>
        public static void Write(IEnumerable<T> source,IMochaCollection<T> destination,int start,int count) {
            for(int counter = 1; counter <= count; counter++) {
                destination.Add(source.ElementAt(start));
                start++;
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Clear all items.
        /// </summary>
        public void Clear() =>
            collection.Clear();

        /// <summary>
        /// Write to destination collection from items.
        /// </summary>
        /// <param name="destination">Destination collection.</param>
        public void Write(IMochaCollection<T> destination) =>
            Write(collection,destination);

        /// <summary>
        /// Write to destination collection from items.
        /// </summary>
        /// <param name="destination">Destination collection.</param>
        /// <param name="start">Start index to write.</param>
        public void Write(IMochaCollection<T> destination,int start) =>
            Write(collection,destination,start);

        /// <summary>
        /// Write to destination collection from items.
        /// </summary>
        /// <param name="destination">Destination collection.</param>
        /// <param name="start">Start index to write.</param>
        /// <param name="count">Count of item to write.</param>
        public void Write(IMochaCollection<T> destination,int start,int count) => 
            Write(collection,destination,start);

        /// <summary>
        /// Read items from collection.
        /// </summary>
        /// <param name="source">Source collections.</param>
        public void Read(IEnumerable<T> source) =>
            collection.AddRange(source);

        /// <summary>
        /// Read items from collection.
        /// </summary>
        /// <param name="source">Source collections.</param>
        /// <param name="start">Start index to read.</param>
        public void Read(IEnumerable<T> source,int start) =>
            collection.AddRange(source.Skip(start));

        /// <summary>
        /// Read items from collection.
        /// </summary>
        /// <param name="source">Source collections.</param>
        /// <param name="start">Start index to read.</param>
        /// <param name="count">Count of item to read.</param>
        public void Read(IEnumerable<T> source,int start,int count) =>
            collection.AddRange(source.Skip(start).Take(count));

        /// <summary>
        /// Returns items as static array.
        /// </summary>
        public T[] ToArray() =>
            collection.ToArray();

        #endregion

        #region Properties

        /// <summary>
        /// Count of item.
        /// </summary>
        public int Length =>
            collection.Count;

        #endregion
    }
}
