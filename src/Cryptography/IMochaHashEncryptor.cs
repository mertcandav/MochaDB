namespace MochaDB.Cryptography {
    /// <summary>
    /// Interface for MochaDB hash encryptors.
    /// </summary>
    public interface IMochaHashEncryptor {
        #region Members

        string Encrypt();
        string Encrypt(string data);

        #endregion

        #region Properties

        string Data { get; set; }

        #endregion
    }
}
