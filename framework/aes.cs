namespace MochaDB.framework {
  using System;
  using System.IO;
  using System.Security.Cryptography;
  using System.Text;

  /// <summary>
  /// AES encryptor of MochaDB.
  /// </summary>
  internal static class aes {
    /// <summary>
    /// Encrypt content.
    /// </summary>
    /// <param name="iv">IV.</param>
    /// <param name="key">Key.</param>
    /// <param name="content">Content.</param>
    public static string Encrypt(string iv,string key,string content) {
      byte[] buffer;

      Aes aes = Aes.Create();
      aes.IV = Encoding.UTF8.GetBytes(iv);
      aes.Key = Encoding.UTF8.GetBytes(key);

      ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key,aes.IV);
      using(MemoryStream ms = new MemoryStream()) {
        using(CryptoStream cs = new CryptoStream(ms,encryptor,CryptoStreamMode.Write))
        using(StreamWriter sw = new StreamWriter(cs))
          sw.Write(content);
        buffer = ms.ToArray();
      }
      aes.Dispose();
      encryptor.Dispose();
      return Convert.ToBase64String(buffer);
    }

    /// <summary>
    /// Decrypt.
    /// </summary>
    /// <param name="iv">IV.</param>
    /// <param name="key">Key.</param>
    /// <param name="content">Content.</param>
    public static string Decrypt(string iv,string key,string content) {
      byte[] buffer = Convert.FromBase64String(content);
      string result;

      Aes aes = Aes.Create();
      aes.IV = Encoding.UTF8.GetBytes(iv);
      aes.Key = Encoding.UTF8.GetBytes(key);

      ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key,aes.IV);
      using(MemoryStream ms = new MemoryStream(buffer))
      using(CryptoStream cs = new CryptoStream(ms,decryptor,CryptoStreamMode.Read))
      using(StreamReader sr = new StreamReader(cs))
        result = sr.ReadToEnd();
      aes.Dispose();
      decryptor.Dispose();
      return result;
    }
  }
}
