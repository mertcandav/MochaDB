namespace MochaDB.Cryptography {
    /// <summary>
    /// Char encryptor.
    /// </summary>
    internal class Mocha_ACE:IMochaEncryptor {
        #region Constructors

        /// <summary>
        /// Create new Mocha_ACE.
        /// </summary>
        public Mocha_ACE() {
            Data=string.Empty;
        }

        /// <summary>
        /// Create new Mocha_ACE;
        /// </summary>
        /// <param name="data">Data to set data.</param>
        public Mocha_ACE(string data) {
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

        /// <summary>
        /// Converts the character to its encrypted counterpart.
        /// </summary>
        public char TranslateCharToCode(char data) {
            #region Numbers.

            if(data == '1') return 'A';
            else if(data == '2') return '*';
            else if(data == '3') return 'ª';
            else if(data == '4') return 'S';
            else if(data == '5') return '.';
            else if(data == '6') return '=';
            else if(data == '7') return '§';
            else if(data == '8') return '1';
            else if(data == '9') return '%';
            else if(data == '0') return 'b';

            #endregion

            #region Lowercase Chars.

            else if(data == 'a') return ':';
            else if(data == 'b') return '&';
            else if(data == 'c') return '$';
            else if(data == 'ç') return '~';
            else if(data == 'd') return '3';
            else if(data == 'e') return '_';
            else if(data == 'f') return '0';
            else if(data == 'g') return 'Y';
            else if(data == 'ğ') return 'a';
            else if(data == 'h') return 'd';
            else if(data == 'ı') return '/';
            else if(data == 'i') return 'g';
            else if(data == 'j') return 'ö';
            else if(data == 'k') return 'V';
            else if(data == 'l') return 'x';
            else if(data == 'm') return '#';
            else if(data == 'n') return ';';
            else if(data == 'o') return '2';
            else if(data == 'ö') return 'w';
            else if(data == 'p') return '{';
            else if(data == 'q') return 'z';
            else if(data == 'r') return '!';
            else if(data == 's') return 'f';
            else if(data == 'ş') return 's';
            else if(data == 't') return '?';
            else if(data == 'u') return 'ü';
            else if(data == 'ü') return 'o';
            else if(data == 'v') return 'y';
            else if(data == 'x') return 'h';
            else if(data == 'w') return 'Ş';
            else if(data == 'y') return 'j';
            else if(data == 'z') return ',';

            #endregion

            #region Uppercase Chars.

            else if(data == 'A') return '£';
            else if(data == 'B') return 'p';
            else if(data == 'C') return 'u';
            else if(data == 'Ç') return '9';
            else if(data == 'D') return 'é';
            else if(data == 'E') return 'ß';
            else if(data == 'F') return 'G';
            else if(data == 'G') return 'y';
            else if(data == 'Ğ') return 'F';
            else if(data == 'H') return 'D';
            else if(data == 'I') return '>';
            else if(data == 'İ') return '4';
            else if(data == 'J') return 'c';
            else if(data == 'K') return 'ş';
            else if(data == 'L') return 'B';
            else if(data == 'M') return 'v';
            else if(data == 'N') return 't';
            else if(data == 'O') return '[';
            else if(data == 'Ö') return 'ğ';
            else if(data == 'P') return '¥';
            else if(data == 'Q') return '<';
            else if(data == 'R') return 'k';
            else if(data == 'S') return '^';
            else if(data == 'Ş') return 'e';
            else if(data == 'T') return ']';
            else if(data == 'U') return '}';
            else if(data == 'Ü') return 'ş';
            else if(data == 'V') return '₺';
            else if(data == 'X') return 'r';
            else if(data == 'W') return '6';
            else if(data == 'Y') return '+';
            else if(data == 'Z') return 'ç';

            #endregion

            #region Special Chars.

            else if(data == '<') return 'l';
            else if(data == '>') return 'C';
            else if(data == '@') return 'Ğ';
            else if(data == ':') return '5';
            else if(data == '¸') return '\\';
            else if(data == '*') return 'J';
            else if(data == '\\') return 'M';
            else if(data == '/') return '¡';
            else if(data == '-') return '(';
            else if(data == '_') return 'n';
            else if(data == '=') return '·';
            else if(data == ']') return 'E';
            else if(data == '[') return 'L';
            else if(data == '{') return 'ı';
            else if(data == '}') return 'O';
            else if(data == '$') return 'P';
            else if(data == '+') return 'Q';
            else if(data == '!') return 'K';
            else if(data == '\"') return 'R';
            else if(data == '.') return 'T';
            else if(data == 'ß') return 'S';
            else if(data == '₺') return 'Ö';
            else if(data == '?') return 'm';
            else if(data == ',') return '7';
            else if(data == ')') return '-';
            else if(data == '(') return 'I';
            else if(data == '&') return 'N';
            else if(data == '%') return ')';
            else if(data == '#') return 'Ç';
            else if(data == '~') return 'İ';
            else if(data == '^') return '@';
            else if(data == '£') return 'i';
            else if(data == 'é') return '8';
            else if(data == '¥') return 'g';
            else if(data == '|') return 'q';
            else if(data == 'æ') return 'U';
            else if(data == '`') return 'X';
            else if(data == '¢') return 'W';
            else if(data == '¶') return 'Z';
            else if(data == '¯') return 'Y';
            else if(data == '´') return 'Ü';

            #endregion

            return data;
        }

        /// <summary>
        /// Converts encryption to its real value.
        /// </summary>
        public char TranslateCodeToChar(char data) {
            #region Numbers.

            if(data == 'A') return '1';
            else if(data == '*') return '2';
            else if(data == 'ª') return '3';
            else if(data == 'S') return '4';
            else if(data == '.') return '5';
            else if(data == '=') return '6';
            else if(data == '§') return '7';
            else if(data == '1') return '8';
            else if(data == '%') return '9';
            else if(data == 'b') return '0';

            #endregion

            #region Lowercase Chars.

            else if(data == ':') return 'a';
            else if(data == '&') return 'b';
            else if(data == '$') return 'c';
            else if(data == '~') return 'ç';
            else if(data == '3') return 'd';
            else if(data == '_') return 'e';
            else if(data == '0') return 'f';
            else if(data == 'Y') return 'g';
            else if(data == 'a') return 'ğ';
            else if(data == 'd') return 'h';
            else if(data == '/') return 'ı';
            else if(data == 'g') return 'i';
            else if(data == 'ö') return 'j';
            else if(data == 'V') return 'k';
            else if(data == 'x') return 'l';
            else if(data == '#') return 'm';
            else if(data == ';') return 'n';
            else if(data == '2') return 'o';
            else if(data == 'w') return 'ö';
            else if(data == '{') return 'p';
            else if(data == 'z') return 'q';
            else if(data == '!') return 'r';
            else if(data == 'f') return 's';
            else if(data == 's') return 'ş';
            else if(data == '?') return 't';
            else if(data == 'ü') return 'u';
            else if(data == 'o') return 'ü';
            else if(data == 'y') return 'v';
            else if(data == 'h') return 'x';
            else if(data == 'Ş') return 'w';
            else if(data == 'j') return 'y';
            else if(data == ',') return 'z';

            #endregion

            #region Uppercase Chars.

            else if(data == '£') return 'A';
            else if(data == 'p') return 'B';
            else if(data == 'u') return 'C';
            else if(data == '9') return 'Ç';
            else if(data == 'é') return 'D';
            else if(data == 'ß') return 'E';
            else if(data == 'G') return 'F';
            else if(data == 'y') return 'G';
            else if(data == 'F') return 'Ğ';
            else if(data == 'D') return 'H';
            else if(data == '>') return 'I';
            else if(data == '4') return 'İ';
            else if(data == 'c') return 'J';
            else if(data == 'ş') return 'K';
            else if(data == 'B') return 'L';
            else if(data == 'v') return 'M';
            else if(data == 't') return 'N';
            else if(data == '[') return 'O';
            else if(data == 'ğ') return 'Ö';
            else if(data == '¥') return 'P';
            else if(data == '<') return 'Q';
            else if(data == 'k') return 'R';
            else if(data == '^') return 'S';
            else if(data == 'e') return 'Ş';
            else if(data == ']') return 'T';
            else if(data == '}') return 'U';
            else if(data == 'ş') return 'Ü';
            else if(data == '₺') return 'V';
            else if(data == 'r') return 'X';
            else if(data == '6') return 'W';
            else if(data == '+') return 'Y';
            else if(data == 'ç') return 'Z';

            #endregion

            #region Special Chars.

            else if(data == 'l') return '<';
            else if(data == 'C') return '>';
            else if(data == 'Ğ') return '@';
            else if(data == '5') return ':';
            else if(data == '\\') return '¸';
            else if(data == 'J') return '*';
            else if(data == 'M') return '\\';
            else if(data == '¡') return '/';
            else if(data == '(') return '-';
            else if(data == 'n') return '_';
            else if(data == '·') return '=';
            else if(data == 'E') return ']';
            else if(data == 'L') return '[';
            else if(data == 'ı') return '}';
            else if(data == 'O') return '}';
            else if(data == 'P') return '$';
            else if(data == 'Q') return '+';
            else if(data == 'K') return '!';
            else if(data == 'R') return '\"';
            else if(data == 'T') return '.';
            else if(data == 'S') return 'ß';
            else if(data == 'Ö') return '₺';
            else if(data == 'm') return '?';
            else if(data == '7') return ',';
            else if(data == '-') return ')';
            else if(data == 'I') return '(';
            else if(data == 'N') return '&';
            else if(data == ')') return '%';
            else if(data == 'Ç') return '#';
            else if(data == 'İ') return '~';
            else if(data == '@') return '^';
            else if(data == 'i') return '£';
            else if(data == '8') return 'é';
            else if(data == 'g') return '¥';
            else if(data == 'q') return '|';
            else if(data == 'U') return 'æ';
            else if(data == 'X') return '`';
            else if(data == 'W') return '¢';
            else if(data == 'Z') return '¶';
            else if(data == 'Y') return '¯';
            else if(data == 'Ü') return '´';

            #endregion

            return data;
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
