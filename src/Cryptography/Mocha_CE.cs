namespace MochaDB.Cryptography {
    /// <summary>
    /// Basic char encryptor.
    /// </summary>
    public class Mocha_CE:IMochaEncryptor {
        #region Constructors

        /// <summary>
        /// Create new Mocha_CE.
        /// </summary>
        public Mocha_CE() {
            Data=string.Empty;
        }

        /// <summary>
        /// Create new Mocha_CE;
        /// </summary>
        /// <param name="data">Data to set data.</param>
        public Mocha_CE(string data) {
            Data=data;
        }

        #endregion

        #region Operators

        public static explicit operator string(Mocha_CE value) =>
            value.ToString();

        #endregion

        #region Static

        /// <summary>
        /// Converts the character to its encrypted counterpart.
        /// </summary>
        public static char TranslateCharToCode(char data) {
            #region Numbers.

            if(data == '1') return '0';
            else if(data == '2') return '1';
            else if(data == '3') return '2';
            else if(data == '4') return '5';
            else if(data == '5') return '7';
            else if(data == '6') return '4';
            else if(data == '7') return '9';
            else if(data == '8') return '3';
            else if(data == '9') return '6';
            else if(data == '0') return '8';

            #endregion

            #region Lowercase Chars.

            else if(data == 'a') return 'f';
            else if(data == 'b') return 'j';
            else if(data == 'c') return 'h';
            else if(data == 'ç') return 'g';
            else if(data == 'd') return 'k';
            else if(data == 'e') return 'd';
            else if(data == 'f') return 'l';
            else if(data == 'g') return 's';
            else if(data == 'ğ') return 'ş';
            else if(data == 'h') return 'a';
            else if(data == 'ı') return 'i';
            else if(data == 'i') return 'q';
            else if(data == 'j') return 'ü';
            else if(data == 'k') return 'ğ';
            else if(data == 'l') return 'w';
            else if(data == 'm') return 'p';
            else if(data == 'n') return 'e';
            else if(data == 'o') return 'o';
            else if(data == 'ö') return 'r';
            else if(data == 'p') return 'ı';
            else if(data == 'q') return 't';
            else if(data == 'r') return 'u';
            else if(data == 's') return 'y';
            else if(data == 'ş') return 'z';
            else if(data == 't') return 'ç';
            else if(data == 'u') return 'x';
            else if(data == 'ü') return 'ö';
            else if(data == 'v') return 'c';
            else if(data == 'x') return 'm';
            else if(data == 'w') return 'b';
            else if(data == 'y') return 'v';
            else if(data == 'z') return 'n';

            #endregion

            #region Uppercase Chars.

            else if(data == 'A') return 'Q';
            else if(data == 'B') return 'T';
            else if(data == 'C') return 'K';
            else if(data == 'Ç') return 'Ş';
            else if(data == 'D') return 'L';
            else if(data == 'E') return 'U';
            else if(data == 'F') return 'E';
            else if(data == 'G') return 'H';
            else if(data == 'Ğ') return 'A';
            else if(data == 'H') return 'V';
            else if(data == 'I') return 'B';
            else if(data == 'İ') return 'P';
            else if(data == 'J') return 'O';
            else if(data == 'K') return 'R';
            else if(data == 'L') return 'Ü';
            else if(data == 'M') return 'Ö';
            else if(data == 'N') return 'İ';
            else if(data == 'O') return 'Ğ';
            else if(data == 'Ö') return 'I';
            else if(data == 'P') return 'G';
            else if(data == 'Q') return 'W';
            else if(data == 'R') return 'X';
            else if(data == 'S') return 'F';
            else if(data == 'Ş') return 'N';
            else if(data == 'T') return 'Z';
            else if(data == 'U') return 'S';
            else if(data == 'Ü') return 'Ç';
            else if(data == 'V') return 'M';
            else if(data == 'X') return 'Y';
            else if(data == 'W') return 'J';
            else if(data == 'Y') return 'C';
            else if(data == 'Z') return 'D';

            #endregion

            else return data;
        }

        /// <summary>
        /// Converts encryption to its real value.
        /// </summary>
        public static char TranslateCodeToChar(char data) {
            #region Numbers.

            if(data == '0') return '1';
            else if(data == '1') return '2';
            else if(data == '2') return '3';
            else if(data == '5') return '4';
            else if(data == '7') return '5';
            else if(data == '4') return '6';
            else if(data == '9') return '7';
            else if(data == '3') return '8';
            else if(data == '6') return '9';
            else if(data == '8') return '0';

            #endregion

            #region Lowercase Chars.

            else if(data == 'f') return 'a';
            else if(data == 'j') return 'b';
            else if(data == 'h') return 'c';
            else if(data == 'g') return 'ç';
            else if(data == 'k') return 'd';
            else if(data == 'd') return 'e';
            else if(data == 'l') return 'f';
            else if(data == 's') return 'g';
            else if(data == 'ş') return 'ğ';
            else if(data == 'a') return 'h';
            else if(data == 'i') return 'ı';
            else if(data == 'q') return 'i';
            else if(data == 'ü') return 'j';
            else if(data == 'ğ') return 'k';
            else if(data == 'w') return 'l';
            else if(data == 'p') return 'm';
            else if(data == 'e') return 'n';
            else if(data == 'o') return 'o';
            else if(data == 'r') return 'ö';
            else if(data == 'ı') return 'p';
            else if(data == 't') return 'q';
            else if(data == 'u') return 'r';
            else if(data == 'y') return 's';
            else if(data == 'z') return 'ş';
            else if(data == 'ç') return 't';
            else if(data == 'x') return 'u';
            else if(data == 'ö') return 'ü';
            else if(data == 'c') return 'v';
            else if(data == 'm') return 'x';
            else if(data == 'b') return 'w';
            else if(data == 'v') return 'y';
            else if(data == 'n') return 'z';

            #endregion

            #region Uppercase Chars.

            else if(data == 'Q') return 'A';
            else if(data == 'T') return 'B';
            else if(data == 'K') return 'C';
            else if(data == 'Ş') return 'Ç';
            else if(data == 'L') return 'D';
            else if(data == 'U') return 'E';
            else if(data == 'E') return 'F';
            else if(data == 'H') return 'G';
            else if(data == 'A') return 'Ğ';
            else if(data == 'V') return 'H';
            else if(data == 'B') return 'I';
            else if(data == 'P') return 'İ';
            else if(data == 'O') return 'J';
            else if(data == 'R') return 'K';
            else if(data == 'Ü') return 'L';
            else if(data == 'Ö') return 'M';
            else if(data == 'İ') return 'N';
            else if(data == 'Ğ') return 'O';
            else if(data == 'I') return 'Ö';
            else if(data == 'G') return 'P';
            else if(data == 'W') return 'Q';
            else if(data == 'X') return 'R';
            else if(data == 'F') return 'S';
            else if(data == 'N') return 'Ş';
            else if(data == 'Z') return 'T';
            else if(data == 'S') return 'U';
            else if(data == 'Ç') return 'Ü';
            else if(data == 'M') return 'V';
            else if(data == 'Y') return 'X';
            else if(data == 'J') return 'W';
            else if(data == 'C') return 'Y';
            else if(data == 'D') return 'Z';

            #endregion

            else return data;
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
            string encryptText = "";

            for(int i = 0; i < Data.Length; i++)
                encryptText += TranslateCharToCode(Data[i]);

            return encryptText;
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
            string decryptText = "";

            for(int i = 0; i < Data.Length; i++)
                decryptText += TranslateCodeToChar(Data[i]);

            return decryptText;
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
