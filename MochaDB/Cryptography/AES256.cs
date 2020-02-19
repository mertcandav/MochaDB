using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace MochaDB.Cryptography {
    /// <summary>
    /// AES 256-Bit encryptor.
    /// </summary>
    public class AES256:IMochaEncryptor {
        #region Constructors

        /// <summary>
        /// Create new AES256.
        /// </summary>
        /// <param name="iv">Iv.</param>
        /// <param name="key">Key.</param>
        public AES256(string iv,string key) {
            Iv=iv;
            Key=key;
            Data=string.Empty;
        }

        /// <summary>
        /// Create new AES256.
        /// </summary>
        /// <param name="iv">Iv.</param>
        /// <param name="key">Key.</param>
        /// <param name="data">Data to set data.</param>
        public AES256(string iv,string key,string data) :
            this(iv,key) {
            Data=data;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Encrypt.
        /// </summary>
        /// <param name="data">Data to set data.</param>
        public string Encrypt(string data) {
            Data=data;
            return Encrypt();
        }

        /// <summary>
        /// Encrypt.
        /// </summary>
        public string Encrypt() {
            byte[] buffer;

            Aes aes = Aes.Create();
            aes.IV = Encoding.UTF8.GetBytes(Iv);
            aes.Key = Encoding.UTF8.GetBytes(Key);

            ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key,aes.IV);
            using(MemoryStream ms = new MemoryStream()) {
                using(CryptoStream cs = new CryptoStream(ms,encryptor,CryptoStreamMode.Write)) {
                    using(StreamWriter sw = new StreamWriter(cs)) {
                        sw.Write(Data);
                    }
                }
                buffer = ms.ToArray();
            }
            aes.Dispose();
            encryptor.Dispose();
            return Convert.ToBase64String(buffer);
        }

        /// <summary>
        /// Decrypt.
        /// </summary>
        /// <param name="data">Data to set data.</param>
        public string Decrypt(string data) {
            Data=data;
            return Decrypt();
        }

        /// <summary>
        /// Decrypt.
        /// </summary>
        public string Decrypt() {
            byte[] buffer = Convert.FromBase64String(Data);
            string result;

            Aes aes = Aes.Create();
            aes.IV = Encoding.UTF8.GetBytes(Iv);
            aes.Key = Encoding.UTF8.GetBytes(Key);

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

        #endregion

        #region Properties

        /// <summary>
        /// Initialization vector.
        /// </summary>
        public string Iv { get; set; }

        /// <summary>
        /// Sector key.
        /// </summary>
        public string Key { get; set; }

        /// <summary>
        /// Data of use the cryptography processes.
        /// </summary>
        public string Data { get; set; }

        #endregion
    }
}
