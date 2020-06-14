using System.Text;

namespace MochaDB.Cryptography {
    /// <summary>
    /// MD5 encryptor of MochaDB.
    /// </summary>
    public class MD5:IMochaHashEncryptor {
        #region Constructors

        /// <summary>
        /// Constructor.
        /// </summary>
        public MD5() {
            Data=string.Empty;
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="data">Data to set data.</param>
        public MD5(string data) {
            Data=data;
        }

        #endregion

        #region Operators

        public static explicit operator string(MD5 value) =>
            value.ToString();

        #endregion

        #region Members

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
            var md5 = System.Security.Cryptography.MD5.Create();
            byte[] data = md5.ComputeHash(Encoding.UTF8.GetBytes(Data));
            StringBuilder sBuilder = new StringBuilder();
            for(int i = 0; i < data.Length; i++)
                sBuilder.Append(data[i].ToString("x2"));
            md5.Dispose();
            return sBuilder.ToString();
        }

        #endregion

        #region Overrides

        /// <summary>
        /// Returns <see cref="Data"/>.
        /// </summary>
        public override string ToString() {
            return Data;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Data of use the cryptography processes.
        /// </summary>
        public string Data { get; set; }

        #endregion
    }
}
