using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace MochaDB.Streams {
    /// <summary>
    /// Stream for MochaDB.
    /// </summary>
    public class MochaStream:Stream, IMochaStream {
        #region Fields

        private MemoryStream baseStream;

        #endregion

        #region Constructors

        /// <summary>
        /// Create a new MochaStream.
        /// </summary>
        public MochaStream() {
            baseStream=new MemoryStream();
        }

        /// <summary>
        /// Create a new MochaStream.
        /// </summary>
        /// <param name="bytes">Bytes of stream.</param>
        public MochaStream(params byte[] bytes) {
            baseStream=new MemoryStream(bytes);
        }

        /// <summary>
        /// Create a new MochaStream.
        /// </summary>
        /// <param name="stream">Source stream.</param>
        public MochaStream(Stream stream) {
            baseStream=new MemoryStream();
            stream.CopyTo(stream);
        }

        /// <summary>
        /// Create a new MochaStream.
        /// </summary>
        /// <param name="bytes">Bytes of stream.</param>
        public MochaStream(IEnumerable<byte> bytes) {
            baseStream=new MemoryStream(bytes.ToArray());
        }

        /// <summary>
        /// Create a new MochaStream.
        /// </summary>
        /// <param name="writable">Whether the stream supports writing.</param>
        /// <param name="bytes">Bytes of stream.</param>
        public MochaStream(bool writable,params byte[] bytes) {
            baseStream=new MemoryStream(bytes,writable);
        }

        /// <summary>
        /// Create a new MochaStream.
        /// </summary>
        /// <param name="writable">Whether the stream supports writing.</param>
        /// <param name="bytes">Bytes of stream.</param>
        public MochaStream(bool writable,IEnumerable<byte> bytes) {
            baseStream=new MemoryStream(bytes.ToArray(),writable);
        }

        #endregion

        #region Operators

        public static explicit operator string(MochaStream value) =>
            value.ToString();

        #endregion

        #region Methods

        /// <summary>
        /// Reads the bytes from the current stream and writes them to another stream.
        /// </summary>
        /// <param name="destination">Destination stream.</param>
        public new void CopyTo(Stream destination) =>
            baseStream.CopyTo(destination);

        /// <summary>
        /// Reads the bytes from the current stream and writes them to another stream.
        /// </summary>
        /// <param name="destination">Destination stream.</param>
        /// <param name="buffer">Buffer size.</param>
        public new void CopyTo(Stream destination,int buffer) =>
            baseStream.CopyTo(destination,buffer);

        /// <summary>
        /// Returns bytes in MochaReader.
        /// </summary>
        public MochaReader<byte> ToReader() =>
            new MochaReader<byte>(Bytes);

        /// <summary>
        /// Returns byte enumerable of stream bytes.
        /// </summary>
        public IEnumerable<byte> ToEnumerable() {
            return baseStream.ToArray().AsEnumerable();
        }

        /// <summary>
        /// Returns bytes as MochaWriter.
        /// </summary>
        public MochaWriter<byte> ToWriter() =>
            new MochaWriter<byte>(Bytes);

        /// <summary>
        /// Returns bytes as MochaStreamWriter.
        /// </summary>
        public MochaStreamWriter ToStreamWriter() =>
            new MochaStreamWriter(Bytes);

        /// <summary>
        /// Writes the stream contents to a byte array, regardless of the Position property.
        /// </summary>
        public byte[] ToArray() =>
            baseStream.ToArray();

        /// <summary>
        /// Releases all resources used by the Stream.
        /// </summary>
        public new void Dispose() {
            baseStream.Dispose();
        }

        /// <summary>
        /// Reads a block of bytes from the current stream and writes the data to a buffer.
        /// </summary>
        /// <param name="buffer">When this method returns, contains the specified byte array with the values between offset and (offset + count - 1) replaced by the characters read from the current stream.</param>
        /// <param name="offset">The zero-based byte offset in buffer at which to begin storing data from the current stream.</param>
        /// <param name="count">The maximum number of bytes to read.</param>
        public override int Read(byte[] buffer,int offset,int count) {
            return baseStream.Read(buffer,offset,count);
        }

        /// <summary>
        /// Writes a block of bytes to the current stream using data read from a buffer.
        /// </summary>
        /// <param name="buffer">The buffer to write data from.</param>
        /// <param name="offset">The zero-based byte offset in buffer at which to begin copying bytes to the current stream.</param>
        /// <param name="count">The maximum number of bytes to write.</param>
        public override void Write(byte[] buffer,int offset,int count) {
            baseStream.Write(buffer,offset,count);
        }

        /// <summary>
        /// Reads a byte from the current stream.
        /// </summary>
        public override int ReadByte() {
            return baseStream.ReadByte();
        }

        /// <summary>
        /// Writes a byte to the current stream at the current position.
        /// </summary>
        /// <param name="value">The byte to write.</param>
        public override void WriteByte(byte value) {
            baseStream.WriteByte(value);
        }

        /// <summary>
        /// Overrides the Flush() method so that no action is performed.
        /// </summary>
        public override void Flush() {
            baseStream.Flush();
        }

        /// <summary>
        /// Sets the position within the current stream to the specified value.
        /// </summary>
        /// <param name="offset">The new position within the stream. This is relative to the loc parameter, and can be positive or negative.</param>
        /// <param name="loc">A value of type SeekOrigin, which acts as the seek reference point.</param>
        public override long Seek(long offset,SeekOrigin loc) {
            return baseStream.Seek(offset,loc);
        }

        /// <summary>
        /// Writes the entire contents of this memory stream to another stream.
        /// </summary>
        /// <param name="destination">The stream to write this memory stream to.</param>
        public void WriteTo(Stream destination) {
            baseStream.WriteTo(destination);
        }

        /// <summary>
        /// Sets the length of the current stream to the specified value.
        /// </summary>
        /// <param name="value">The value at which to set the length.</param>
        public override void SetLength(long value) {
            baseStream.SetLength(value);
        }

        #endregion

        #region Overrides

        /// <summary>
        /// Returns <see cref="Bytes"/> as string with UTF8.
        /// </summary>
        public override string ToString() {
            return Encoding.UTF8.GetString(Bytes);
        }

        #endregion

        #region Properties

        /// <summary>
        /// Bytes of stream. RESET STREAM IF SET.
        /// </summary>
        public byte[] Bytes {
            get =>
                ToArray();
            set {
                baseStream =new MemoryStream(value);
            }
        }

        /// <summary>
        /// Gets or sets the number of bytes allocated for this stream.
        /// </summary>
        public int Capacity {
            get => baseStream.Capacity;
            set => baseStream.Capacity=value;
        }

        /// <summary>
        /// Gets the length of the stream in bytes.
        /// </summary>
        public override long Length =>
            baseStream.Length;

        /// <summary>
        /// Gets a value indicating whether the current stream supports reading.
        /// </summary>
        public override bool CanRead =>
            baseStream.CanRead;

        /// <summary>
        /// Gets a value indicating whether the current stream supports writing.
        /// </summary>
        public override bool CanWrite =>
            baseStream.CanWrite;

        /// <summary>
        /// Gets a value that determines whether the current stream can time out.
        /// </summary>
        public override bool CanTimeout =>
            baseStream.CanTimeout;

        /// <summary>
        /// Gets a value indicating whether the current stream supports seeking.
        /// </summary>
        public override bool CanSeek =>
            baseStream.CanSeek;

        /// <summary>
        /// Gets or sets a value, in milliseconds, that determines how long the stream will attempt to read before timing out.
        /// </summary>
        public override int ReadTimeout {
            get => baseStream.ReadTimeout;
            set => baseStream.ReadTimeout=value;
        }

        /// <summary>
        /// Gets or sets a value, in milliseconds, that determines how long the stream will attempt to write before timing out.
        /// </summary>
        public override int WriteTimeout {
            get => baseStream.WriteTimeout;
            set => baseStream.WriteTimeout=value;
        }

        /// <summary>
        /// Gets or sets the current position within the stream.
        /// </summary>
        public override long Position {
            get => baseStream.Position;
            set => baseStream.Position=value;
        }

        #endregion
    }
}
