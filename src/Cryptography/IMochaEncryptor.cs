namespace MochaDB.Cryptography {
    /// <summary>
    /// Interface for MochaDB encryptors.
    /// </summary>
    public interface IMochaEncryptor {
        #region Methods

        string Encrypt();
        string Encrypt(string data);
        string Decrypt();
        string Decrypt(string data);

        #endregion

        #region Properties

        string Data { get; set; }

        #endregion
    }
}
