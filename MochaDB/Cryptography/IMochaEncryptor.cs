namespace MochaDB.Cryptography {
    /// <summary>
    /// Interface for MochaDB encryptors.
    /// </summary>
    public interface IMochaEncryptor {
        #region Methods

        public string Encrypt();
        public string Encrypt(string data);
        public string Decrypt();
        public string Decrypt(string data);

        #endregion

        #region Properties

        public string Data { get; set; }

        #endregion
    }
}
