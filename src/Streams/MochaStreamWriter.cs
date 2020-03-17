using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace MochaDB.Streams {
    /// <summary>
    /// Stream writer for MochaDB.
    /// </summary>
    public class MochaStreamWriter:IMochaWriter<byte> {
        #region Fields

        private List<byte> collection;

        #endregion

        #region Constructors

        /// <summary>
        /// Create a new MochaStreamWriter.
        /// </summary>
        public MochaStreamWriter() {
            collection = new List<byte>();
        }

        /// <summary>
        /// Create a new MochaStreamWriter.
        /// </summary>
        /// <param name="values">Values of stream writer.</param>
        public MochaStreamWriter(params byte[] values) :
            this() {
            collection.AddRange(values);
        }

        /// <summary>
        /// Create a new MochaStreamWriter.
        /// </summary>
        /// <param name="values">Values of stream writer.</param>
        public MochaStreamWriter(IEnumerable<byte> values) :
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
        public static void Write(IEnumerable<byte> source,IMochaCollection<byte> destination) =>
            MochaWriter<byte>.Write(source,destination);

        /// <summary>
        /// Write to destination collection from source collection.
        /// </summary>
        /// <param name="source">Source collection.</param>
        /// <param name="destination">Destination collection.</param>
        /// <param name="start">Start index to write.</param>
        public static void Write(IEnumerable<byte> source,IMochaCollection<byte> destination,int start) =>
            MochaWriter<byte>.Write(source,destination,start);

        /// <summary>
        /// Write to destination collection from source collection.
        /// </summary>
        /// <param name="source">Source collection.</param>
        /// <param name="destination">Destination collection.</param>
        /// <param name="start">Start index to write.</param>
        /// <param name="count">Count of item to write.</param>
        public static void Write(IEnumerable<byte> source,IMochaCollection<byte> destination,int start,int count) =>
            MochaWriter<byte>.Write(source,destination,start,count);

        /// <summary>
        /// Write to stream from source collection.
        /// </summary>
        /// <param name="source">Source collection.</param>
        /// <param name="destination">Destination stream.</param>
        public static void WriteToStream(IEnumerable<byte> source,Stream destination) {
            for(int index = 0; index < source.Count(); index++)
                destination.WriteByte(source.ElementAt(index));
        }

        /// <summary>
        /// Write to stream from source collection.
        /// </summary>
        /// <param name="source">Source collection.</param>
        /// <param name="destination">Destination stream.</param>
        /// <param name="start">Start index to write.</param>
        public static void WriteToStream(IEnumerable<byte> source,Stream destination,int start) {
            for(int index = start; index < source.Count(); index++)
                destination.WriteByte(source.ElementAt(index));
        }

        /// <summary>
        /// Write to stream from source collection.
        /// </summary>
        /// <param name="source">Source collection.</param>
        /// <param name="destination">Destination stream.</param>
        /// <param name="start">Start index to write.</param>
        /// <param name="count">Count of item to write.</param>
        public static void WriteToStream(IEnumerable<byte> source,Stream destination,int start,int count) {
            for(int counter = 1; counter <= count; counter++) {
                destination.WriteByte(source.ElementAt(start));
                start++;
            }
        }

        /// <summary>
        /// Write asynchronous to destination collection from source collection.
        /// </summary>
        /// <param name="source">Source collection.</param>
        /// <param name="destination">Destination collection.</param>
        public static void WriteAsync(IEnumerable<byte> source,IMochaCollection<byte> destination) {
            var async = new Task(() => { Write(source,destination); });
            async.Start();
        }

        /// <summary>
        /// Write asynchronous to destination collection from source collection.
        /// </summary>
        /// <param name="source">Source collection.</param>
        /// <param name="destination">Destination collection.</param>
        /// <param name="start">Start index to write.</param>
        public static void WriteAsync(IEnumerable<byte> source,IMochaCollection<byte> destination,int start) {
            var async = new Task(() => { Write(source,destination,start); });
            async.Start();
        }

        /// <summary>
        /// Write asynchronous to destination collection from source collection.
        /// </summary>
        /// <param name="source">Source collection.</param>
        /// <param name="destination">Destination collection.</param>
        /// <param name="start">Start index to write.</param>
        /// <param name="count">Count of item to write.</param>
        public static void WriteAsync(IEnumerable<byte> source,IMochaCollection<byte> destination,int start,int count) {
            var async = new Task(() => { Write(source,destination,start,count); });
            async.Start();
        }

        /// <summary>
        /// Write asynchronous to stream from source collection.
        /// </summary>
        /// <param name="source">Source collection.</param>
        /// <param name="destination">Destination stream.</param>
        public static void WriteToStreamAsync(IEnumerable<byte> source,Stream destination) {
            var async = new Task(() => {
                for(int index = 0; index < source.Count(); index++)
                    destination.WriteByte(source.ElementAt(index));
            });
            async.Start();
        }

        /// <summary>
        /// Write asynchronous to stream from source collection.
        /// </summary>
        /// <param name="source">Source collection.</param>
        /// <param name="destination">Destination stream.</param>
        /// <param name="start">Start index to write.</param>
        public static void WriteToStreamAsync(IEnumerable<byte> source,Stream destination,int start) {
            var async = new Task(() => {
                for(int index = start; index < source.Count(); index++)
                    destination.WriteByte(source.ElementAt(index));
            });
            async.Start();
        }

        /// <summary>
        /// Write asynchronous to stream from source collection.
        /// </summary>
        /// <param name="source">Source collection.</param>
        /// <param name="destination">Destination stream.</param>
        /// <param name="start">Start index to write.</param>
        /// <param name="count">Count of item to write.</param>
        public static void WriteToStreamAsync(IEnumerable<byte> source,Stream destination,int start,int count) {
            var async = new Task(() => {
                for(int counter = 1; counter <= count; counter++) {
                    destination.WriteByte(source.ElementAt(start));
                    start++;
                }
            });
            async.Start();
        }

        #endregion

        #region Methods

        /// <summary>
        /// Clear all bytes.
        /// </summary>
        public void Clear() =>
            collection.Clear();

        /// <summary>
        /// Write to destination collection from bytes.
        /// </summary>
        /// <param name="destination">Destination collection.</param>
        public void Write(IMochaCollection<byte> destination) =>
            Write(collection,destination);

        /// <summary>
        /// Write to destination collection from bytes.
        /// </summary>
        /// <param name="destination">Destination collection.</param>
        /// <param name="start">Start index to write.</param>
        public void Write(IMochaCollection<byte> destination,int start) =>
            Write(collection,destination,start);

        /// <summary>
        /// Write to destination collection from bytes.
        /// </summary>
        /// <param name="destination">Destination collection.</param>
        /// <param name="start">Start index to write.</param>
        /// <param name="count">Count of item to write.</param>
        public void Write(IMochaCollection<byte> destination,int start,int count) =>
            Write(collection,destination,start,count);

        /// <summary>
        /// Write to stream from source collection.
        /// </summary>
        /// <param name="destination">Destination stream.</param>
        public void WriteToStream(Stream destination) =>
            WriteToStream(collection,destination);

        /// <summary>
        /// Write to stream from source collection.
        /// </summary>
        /// <param name="destination">Destination stream.</param>
        /// <param name="start">Start index to write.</param>
        public void WriteToStream(Stream destination,int start) =>
            WriteToStream(collection,destination,start);

        /// <summary>
        /// Write to stream from source collection.
        /// </summary>
        /// <param name="destination">Destination stream.</param>
        /// <param name="start">Start index to write.</param>
        /// <param name="count">Count of item to write.</param>
        public void WriteToStream(Stream destination,int start,int count) =>
            WriteToStream(collection,destination,start,count);

        /// <summary>
        /// Write asynchronous to destination collection from bytes.
        /// </summary>
        /// <param name="destination">Destination collection.</param>
        public void WriteAsync(IMochaCollection<byte> destination) =>
            WriteAsync(collection,destination);

        /// <summary>
        /// Write asynchronous to destination collection from bytes.
        /// </summary>
        /// <param name="destination">Destination collection.</param>
        /// <param name="start">Start index to write.</param>
        public void WriteAsync(IMochaCollection<byte> destination,int start) =>
            WriteAsync(collection,destination,start);

        /// <summary>
        /// Write asynchronous to destination collection from bytes.
        /// </summary>
        /// <param name="destination">Destination collection.</param>
        /// <param name="start">Start index to write.</param>
        /// <param name="count">Count of item to write.</param>
        public void WriteAsync(IMochaCollection<byte> destination,int start,int count) =>
            WriteAsync(collection,destination,start,count);

        /// <summary>
        /// Write asynchronous to stream from source collection.
        /// </summary>
        /// <param name="destination">Destination stream.</param>
        public void WriteToStreamAsync(Stream destination) =>
            WriteToStreamAsync(collection,destination);

        /// <summary>
        /// Write asynchronous to stream from source collection.
        /// </summary>
        /// <param name="destination">Destination stream.</param>
        /// <param name="start">Start index to write.</param>
        public void WriteToStreamAsync(Stream destination,int start) =>
            WriteToStreamAsync(collection,destination,start);

        /// <summary>
        /// Write asynchronous to stream from source collection.
        /// </summary>
        /// <param name="destination">Destination stream.</param>
        /// <param name="start">Start index to write.</param>
        /// <param name="count">Count of item to write.</param>
        public void WriteToStreamAsync(Stream destination,int start,int count) =>
            WriteToStreamAsync(collection,destination,start,count);

        /// <summary>
        /// Read bytes from collection.
        /// </summary>
        /// <param name="source">Source collections.</param>
        public void Read(IEnumerable<byte> source) =>
            collection.AddRange(source);

        /// <summary>
        /// Read bytes from collection.
        /// </summary>
        /// <param name="source">Source collections.</param>
        /// <param name="start">Start index to read.</param>
        public void Read(IEnumerable<byte> source,int start) =>
            collection.AddRange(source.Skip(start));

        /// <summary>
        /// Read bytes from collection.
        /// </summary>
        /// <param name="source">Source collections.</param>
        /// <param name="start">Start index to read.</param>
        /// <param name="count">Count of item to read.</param>
        public void Read(IEnumerable<byte> source,int start,int count) =>
            collection.AddRange(source.Skip(start).Take(count));

        /// <summary>
        /// Returns bytes as static array.
        /// </summary>
        public byte[] ToArray() =>
            collection.ToArray();

        /// <summary>
        /// Returns bytes as MochaStream.
        /// </summary>
        public MochaReader<byte> ToReader() =>
            new MochaReader<byte>(collection);

        /// <summary>
        /// Returns bytes as MochaStream.
        /// </summary>
        /// <returns></returns>
        public MochaStream ToStream() =>
            new MochaStream(collection);

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
