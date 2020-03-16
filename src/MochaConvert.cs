using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MochaDB.Streams;

namespace MochaDB {
    /// <summary>
    /// Converter for MochaDB.
    /// </summary>
    public static class MochaConvert {
        /// <summary>
        /// Returns bytes of string with UTF8 encoding.
        /// </summary>
        /// <param name="value">String value.</param>
        public static byte[] GetStringBytes(string value) =>
            Encoding.UTF8.GetBytes(value);

        /// <summary>
        /// Returns bytes of string with encoding.
        /// </summary>
        /// <param name="value">String value.</param>
        /// <param name="encoding">Encoding.</param>
        public static byte[] GetStringBytes(string value,Encoding encoding) =>
            encoding.GetBytes(value);

        /// <summary>
        /// Returns bytes of string with encoding.
        /// </summary>
        /// <param name="value">String value.</param>
        /// <param name="encoding">Name of encoding.</param>
        public static byte[] GetStringBytes(string value,string encoding) =>
            Encoding.GetEncoding(encoding).GetBytes(value);

        /// <summary>
        /// Returns Base64 string from bytes.
        /// </summary>
        /// <param name="bytes">Bytes.</param>
        public static string ToBase64String(params byte[] bytes) =>
            Convert.ToBase64String(bytes);

        /// <summary>
        /// Returns Base64 string from bytes.
        /// </summary>
        /// <param name="bytes">Bytes.</param>
        public static string ToBase64String(IEnumerable<byte> bytes) =>
            Convert.ToBase64String(bytes.ToArray());

        /// <summary>
        /// Returns Base64 string from stream of bytes.
        /// </summary>
        /// <param name="stream">Stream.</param>
        public static string ToBase64String(MochaStream stream) =>
            Convert.ToBase64String(stream.Bytes);

        /// <summary>
        /// Returns Base64 bytes from Base64 string.
        /// </summary>
        /// <param name="value">Bytes.</param>
        public static byte[] FromBase64String(string value) =>
            Convert.FromBase64String(value);
    }
}
