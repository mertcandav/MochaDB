namespace MochaDB.Encryptors {
    /// <summary>
    /// Basic char encryptor.
    /// </summary>
    internal class Mocha_CE {
        /// <summary>
        /// Encrypt.
        /// </summary>
        public static string Encrypt(string Text) {
            string EncryptText = "";

            for(int i = 0; i < Text.Length; i++)
                EncryptText += TranslateCharToCode(Text[i]);

            return EncryptText;
        }

        /// <summary>
        /// Decrypt.
        /// </summary>
        public static string Decrypt(string Text) {
            string DecryptText = "";

            for(int i = 0; i < Text.Length; i++)
                DecryptText += TranslateCodeToChar(Text[i]);

            return DecryptText;
        }

        /// <summary>
        /// Converts the character to its encrypted counterpart.
        /// </summary>
        public static char TranslateCharToCode(char Char) {
            #region Numbers.

            if(Char == '1') return '0';
            else if(Char == '2') return '1';
            else if(Char == '3') return '2';
            else if(Char == '4') return '5';
            else if(Char == '5') return '7';
            else if(Char == '6') return '4';
            else if(Char == '7') return '9';
            else if(Char == '8') return '3';
            else if(Char == '9') return '6';
            else if(Char == '0') return '8';

            #endregion

            #region Lowercase Chars.

            else if(Char == 'a') return 'f';
            else if(Char == 'b') return 'j';
            else if(Char == 'c') return 'h';
            else if(Char == 'ç') return 'g';
            else if(Char == 'd') return 'k';
            else if(Char == 'e') return 'd';
            else if(Char == 'f') return 'l';
            else if(Char == 'g') return 's';
            else if(Char == 'ğ') return 'ş';
            else if(Char == 'h') return 'a';
            else if(Char == 'ı') return 'i';
            else if(Char == 'i') return 'q';
            else if(Char == 'j') return 'ü';
            else if(Char == 'k') return 'ğ';
            else if(Char == 'l') return 'w';
            else if(Char == 'm') return 'p';
            else if(Char == 'n') return 'e';
            else if(Char == 'o') return 'o';
            else if(Char == 'ö') return 'r';
            else if(Char == 'p') return 'ı';
            else if(Char == 'q') return 't';
            else if(Char == 'r') return 'u';
            else if(Char == 's') return 'y';
            else if(Char == 'ş') return 'z';
            else if(Char == 't') return 'ç';
            else if(Char == 'u') return 'x';
            else if(Char == 'ü') return 'ö';
            else if(Char == 'v') return 'c';
            else if(Char == 'x') return 'm';
            else if(Char == 'w') return 'b';
            else if(Char == 'y') return 'v';
            else if(Char == 'z') return 'n';

            #endregion

            #region Uppercase Chars.

            else if(Char == 'A') return 'Q';
            else if(Char == 'B') return 'T';
            else if(Char == 'C') return 'K';
            else if(Char == 'Ç') return 'Ş';
            else if(Char == 'D') return 'L';
            else if(Char == 'E') return 'U';
            else if(Char == 'F') return 'E';
            else if(Char == 'G') return 'H';
            else if(Char == 'Ğ') return 'A';
            else if(Char == 'H') return 'V';
            else if(Char == 'I') return 'B';
            else if(Char == 'İ') return 'P';
            else if(Char == 'J') return 'O';
            else if(Char == 'K') return 'R';
            else if(Char == 'L') return 'Ü';
            else if(Char == 'M') return 'Ö';
            else if(Char == 'N') return 'İ';
            else if(Char == 'O') return 'Ğ';
            else if(Char == 'Ö') return 'I';
            else if(Char == 'P') return 'G';
            else if(Char == 'Q') return 'W';
            else if(Char == 'R') return 'X';
            else if(Char == 'S') return 'F';
            else if(Char == 'Ş') return 'N';
            else if(Char == 'T') return 'Z';
            else if(Char == 'U') return 'S';
            else if(Char == 'Ü') return 'Ç';
            else if(Char == 'V') return 'M';
            else if(Char == 'X') return 'Y';
            else if(Char == 'W') return 'J';
            else if(Char == 'Y') return 'C';
            else if(Char == 'Z') return 'D';

            #endregion

            else return Char;
        }

        /// <summary>
        /// Converts encryption to its real value.
        /// </summary>
        public static char TranslateCodeToChar(char Char) {
            #region Numbers.

            if(Char == '0') return '1';
            else if(Char == '1') return '2';
            else if(Char == '2') return '3';
            else if(Char == '5') return '4';
            else if(Char == '7') return '5';
            else if(Char == '4') return '6';
            else if(Char == '9') return '7';
            else if(Char == '3') return '8';
            else if(Char == '6') return '9';
            else if(Char == '8') return '0';

            #endregion

            #region Lowercase Chars.

            else if(Char == 'f') return 'a';
            else if(Char == 'j') return 'b';
            else if(Char == 'h') return 'c';
            else if(Char == 'g') return 'ç';
            else if(Char == 'k') return 'd';
            else if(Char == 'd') return 'e';
            else if(Char == 'l') return 'f';
            else if(Char == 's') return 'g';
            else if(Char == 'ş') return 'ğ';
            else if(Char == 'a') return 'h';
            else if(Char == 'i') return 'ı';
            else if(Char == 'q') return 'i';
            else if(Char == 'ü') return 'j';
            else if(Char == 'ğ') return 'k';
            else if(Char == 'w') return 'l';
            else if(Char == 'p') return 'm';
            else if(Char == 'e') return 'n';
            else if(Char == 'o') return 'o';
            else if(Char == 'r') return 'ö';
            else if(Char == 'ı') return 'p';
            else if(Char == 't') return 'q';
            else if(Char == 'u') return 'r';
            else if(Char == 'y') return 's';
            else if(Char == 'z') return 'ş';
            else if(Char == 'ç') return 't';
            else if(Char == 'x') return 'u';
            else if(Char == 'ö') return 'ü';
            else if(Char == 'c') return 'v';
            else if(Char == 'm') return 'x';
            else if(Char == 'b') return 'w';
            else if(Char == 'v') return 'y';
            else if(Char == 'n') return 'z';

            #endregion

            #region Uppercase Chars.

            else if(Char == 'Q') return 'A';
            else if(Char == 'T') return 'B';
            else if(Char == 'K') return 'C';
            else if(Char == 'Ş') return 'Ç';
            else if(Char == 'L') return 'D';
            else if(Char == 'U') return 'E';
            else if(Char == 'E') return 'F';
            else if(Char == 'H') return 'G';
            else if(Char == 'A') return 'Ğ';
            else if(Char == 'V') return 'H';
            else if(Char == 'B') return 'I';
            else if(Char == 'P') return 'İ';
            else if(Char == 'O') return 'J';
            else if(Char == 'R') return 'K';
            else if(Char == 'Ü') return 'L';
            else if(Char == 'Ö') return 'M';
            else if(Char == 'İ') return 'N';
            else if(Char == 'Ğ') return 'O';
            else if(Char == 'I') return 'Ö';
            else if(Char == 'G') return 'P';
            else if(Char == 'W') return 'Q';
            else if(Char == 'X') return 'R';
            else if(Char == 'F') return 'S';
            else if(Char == 'N') return 'Ş';
            else if(Char == 'Z') return 'T';
            else if(Char == 'S') return 'U';
            else if(Char == 'Ç') return 'Ü';
            else if(Char == 'M') return 'V';
            else if(Char == 'Y') return 'X';
            else if(Char == 'J') return 'W';
            else if(Char == 'C') return 'Y';
            else if(Char == 'D') return 'Z';

            #endregion

            else return Char;
        }
    }
}
