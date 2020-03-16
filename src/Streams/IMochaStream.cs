using System;
using System.Collections.Generic;
using System.IO;

namespace MochaDB.Streams {
    /// <summary>
    /// Interface for MochaDB streams.
    /// </summary>
    public interface IMochaStream:IDisposable {
        #region Methods

        void WriteTo(Stream destination);
        void SetLength(long value);
        int Read(byte[] buffer,int offset,int count);
        void Write(byte[] buffer,int offset,int count);
        int ReadByte();
        void WriteByte(byte value);
        void Flush();
        long Seek(long offset,SeekOrigin loc);
        IEnumerable<byte> ToEnumerable();
        void CopyTo(Stream destination);
        byte[] ToArray();

        #endregion

        #region Properties

        bool CanRead { get; }
        bool CanTimeout { get; }
        bool CanWrite { get; }
        bool CanSeek { get; }
        int ReadTimeout { get; set; }
        int WriteTimeout { get; set; }
        byte[] Bytes { get; set; }
        int Capacity { get; set; }
        long Position { get; set; }
        long Length { get; }

        #endregion
    }
}
