using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace MochaDB.Encryptors {
    /// <summary>
    /// AES 256-Bit ecryptor.
    /// </summary>
    internal sealed class AES256 {
        private static string IV = "MochaDB#$#3{2533";
        private static string KEY = "MochaDBM6YxoFsLXu33FpJdjX0R89xGF";

        /// <summary>
        /// Encrypt value with AES256.
        /// </summary>
        /// <param name="data">Data to encrypt.</param>
        public static string Encrypt(string data) {
            byte[] buffer;

            Aes aes = Aes.Create();
            aes.IV = Encoding.UTF8.GetBytes(IV);
            aes.Key = Encoding.UTF8.GetBytes(KEY);

            ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key,aes.IV);
            using(MemoryStream ms = new MemoryStream()) {
                using(CryptoStream cs = new CryptoStream(ms,encryptor,CryptoStreamMode.Write)) {
                    using(StreamWriter sw = new StreamWriter(cs)) {
                        sw.Write(data);
                    }
                }
                buffer = ms.ToArray();
            }
            aes.Dispose();
            encryptor.Dispose();
            return Convert.ToBase64String(buffer);
        }

        /// <summary>
        /// Decrypt value with AES256.
        /// </summary>
        /// <param name="data">Data to decrypt.</param>
        public static string Decrypt(string data) {
            byte[] buffer = Convert.FromBase64String(data);
            string result;

            Aes aes = Aes.Create();
            aes.IV = Encoding.UTF8.GetBytes(IV);
            aes.Key = Encoding.UTF8.GetBytes(KEY);

            ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key,aes.IV);
            using(MemoryStream ms = new MemoryStream(buffer)) {
                using(CryptoStream cs = new CryptoStream(ms,decryptor,CryptoStreamMode.Read)) {
                    using(StreamReader sr = new StreamReader(cs)) {
                        result = sr.ReadToEnd();
                    }
                }
            }
            aes.Dispose();
            decryptor.Dispose();
            return result;
        }
    }
}
