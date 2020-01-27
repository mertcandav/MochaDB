namespace MochaDB.Encryptors {
    /// <summary>
    /// Char encryptor.
    /// </summary>
    internal class Mocha_ACE {
        /// <summary>
        /// Encrypt.
        /// </summary>
        public static string Encrypt(string Text) {
            Text = Text.TrimStart().TrimEnd();
            string EncryptText = "";

            for(int i = 0; i < Text.Length; i++)
                EncryptText += TranslateCharToCode(Text[i]);

            return EncryptText;
        }

        /// <summary>
        /// Decrypt.
        /// </summary>
        public static string Decrypt(string Text) {
            Text = Text.TrimStart().TrimEnd();
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

            if(Char == '1') return 'A';
            else if(Char == '2') return '*';
            else if(Char == '3') return 'ª';
            else if(Char == '4') return 'S';
            else if(Char == '5') return '.';
            else if(Char == '6') return '=';
            else if(Char == '7') return '§';
            else if(Char == '8') return '1';
            else if(Char == '9') return '%';
            else if(Char == '0') return 'b';

            #endregion

            #region Lowercase Chars.

            else if(Char == 'a') return ':';
            else if(Char == 'b') return '&';
            else if(Char == 'c') return '$';
            else if(Char == 'ç') return '~';
            else if(Char == 'd') return '3';
            else if(Char == 'e') return '_';
            else if(Char == 'f') return '0';
            else if(Char == 'g') return 'Y';
            else if(Char == 'ğ') return 'a';
            else if(Char == 'h') return 'd';
            else if(Char == 'ı') return '/';
            else if(Char == 'i') return 'g';
            else if(Char == 'j') return 'ö';
            else if(Char == 'k') return 'V';
            else if(Char == 'l') return 'x';
            else if(Char == 'm') return '#';
            else if(Char == 'n') return ';';
            else if(Char == 'o') return '2';
            else if(Char == 'ö') return 'w';
            else if(Char == 'p') return '{';
            else if(Char == 'q') return 'z';
            else if(Char == 'r') return '!';
            else if(Char == 's') return 'f';
            else if(Char == 'ş') return 's';
            else if(Char == 't') return '?';
            else if(Char == 'u') return 'ü';
            else if(Char == 'ü') return 'o';
            else if(Char == 'v') return 'y';
            else if(Char == 'x') return 'h';
            else if(Char == 'w') return 'Ş';
            else if(Char == 'y') return 'j';
            else if(Char == 'z') return ',';

            #endregion

            #region Uppercase Chars.

            else if(Char == 'A') return '£';
            else if(Char == 'B') return 'p';
            else if(Char == 'C') return 'u';
            else if(Char == 'Ç') return '9';
            else if(Char == 'D') return 'é';
            else if(Char == 'E') return 'ß';
            else if(Char == 'F') return 'G';
            else if(Char == 'G') return 'y';
            else if(Char == 'Ğ') return 'F';
            else if(Char == 'H') return 'D';
            else if(Char == 'I') return '>';
            else if(Char == 'İ') return '4';
            else if(Char == 'J') return 'c';
            else if(Char == 'K') return 'ş';
            else if(Char == 'L') return 'B';
            else if(Char == 'M') return 'v';
            else if(Char == 'N') return 't';
            else if(Char == 'O') return '[';
            else if(Char == 'Ö') return 'ğ';
            else if(Char == 'P') return '¥';
            else if(Char == 'Q') return '<';
            else if(Char == 'R') return 'k';
            else if(Char == 'S') return '^';
            else if(Char == 'Ş') return 'e';
            else if(Char == 'T') return ']';
            else if(Char == 'U') return '}';
            else if(Char == 'Ü') return 'ş';
            else if(Char == 'V') return '₺';
            else if(Char == 'X') return 'r';
            else if(Char == 'W') return '6';
            else if(Char == 'Y') return '+';
            else if(Char == 'Z') return 'ç';

            #endregion

            #region Special Chars.

            else if(Char == '<') return 'l';
            else if(Char == '>') return 'C';
            else if(Char == '@') return 'Ğ';
            else if(Char == ':') return '5';
            else if(Char == '¸') return '\\';
            else if(Char == '*') return 'J';
            else if(Char == '\\') return 'M';
            else if(Char == '/') return '¡';
            else if(Char == '-') return '(';
            else if(Char == '_') return 'n';
            else if(Char == '=') return '·';
            else if(Char == ']') return 'E';
            else if(Char == '[') return 'L';
            else if(Char == '{') return 'ı';
            else if(Char == '}') return 'O';
            else if(Char == '$') return 'P';
            else if(Char == '+') return 'Q';
            else if(Char == '!') return 'K';
            else if(Char == '\"') return 'R';
            else if(Char == '.') return 'T';
            else if(Char == 'ß') return 'S';
            else if(Char == '₺') return 'Ö';
            else if(Char == '?') return 'm';
            else if(Char == ',') return '7';
            else if(Char == ')') return '-';
            else if(Char == '(') return 'I';
            else if(Char == '&') return 'N';
            else if(Char == '%') return ')';
            else if(Char == '#') return 'Ç';
            else if(Char == '~') return 'İ';
            else if(Char == '^') return '@';
            else if(Char == '£') return 'i';
            else if(Char == 'é') return '8';
            else if(Char == '¥') return 'g';
            else if(Char == '|') return 'q';
            else if(Char == 'æ') return 'U';
            else if(Char == '`') return 'X';
            else if(Char == '¢') return 'W';
            else if(Char == '¶') return 'Z';
            else if(Char == '¯') return 'Y';
            else if(Char == '´') return 'Ü';

            #endregion

            return Char;
        }

        /// <summary>
        /// Converts encryption to its real value.
        /// </summary>
        public static char TranslateCodeToChar(char Char) {
            #region Numbers.

            if(Char == 'A') return '1';
            else if(Char == '*') return '2';
            else if(Char == 'ª') return '3';
            else if(Char == 'S') return '4';
            else if(Char == '.') return '5';
            else if(Char == '=') return '6';
            else if(Char == '§') return '7';
            else if(Char == '1') return '8';
            else if(Char == '%') return '9';
            else if(Char == 'b') return '0';

            #endregion

            #region Lowercase Chars.

            else if(Char == ':') return 'a';
            else if(Char == '&') return 'b';
            else if(Char == '$') return 'c';
            else if(Char == '~') return 'ç';
            else if(Char == '3') return 'd';
            else if(Char == '_') return 'e';
            else if(Char == '0') return 'f';
            else if(Char == 'Y') return 'g';
            else if(Char == 'a') return 'ğ';
            else if(Char == 'd') return 'h';
            else if(Char == '/') return 'ı';
            else if(Char == 'g') return 'i';
            else if(Char == 'ö') return 'j';
            else if(Char == 'V') return 'k';
            else if(Char == 'x') return 'l';
            else if(Char == '#') return 'm';
            else if(Char == ';') return 'n';
            else if(Char == '2') return 'o';
            else if(Char == 'w') return 'ö';
            else if(Char == '{') return 'p';
            else if(Char == 'z') return 'q';
            else if(Char == '!') return 'r';
            else if(Char == 'f') return 's';
            else if(Char == 's') return 'ş';
            else if(Char == '?') return 't';
            else if(Char == 'ü') return 'u';
            else if(Char == 'o') return 'ü';
            else if(Char == 'y') return 'v';
            else if(Char == 'h') return 'x';
            else if(Char == 'Ş') return 'w';
            else if(Char == 'j') return 'y';
            else if(Char == ',') return 'z';

            #endregion

            #region Uppercase Chars.

            else if(Char == '£') return 'A';
            else if(Char == 'p') return 'B';
            else if(Char == 'u') return 'C';
            else if(Char == '9') return 'Ç';
            else if(Char == 'é') return 'D';
            else if(Char == 'ß') return 'E';
            else if(Char == 'G') return 'F';
            else if(Char == 'y') return 'G';
            else if(Char == 'F') return 'Ğ';
            else if(Char == 'D') return 'H';
            else if(Char == '>') return 'I';
            else if(Char == '4') return 'İ';
            else if(Char == 'c') return 'J';
            else if(Char == 'ş') return 'K';
            else if(Char == 'B') return 'L';
            else if(Char == 'v') return 'M';
            else if(Char == 't') return 'N';
            else if(Char == '[') return 'O';
            else if(Char == 'ğ') return 'Ö';
            else if(Char == '¥') return 'P';
            else if(Char == '<') return 'Q';
            else if(Char == 'k') return 'R';
            else if(Char == '^') return 'S';
            else if(Char == 'e') return 'Ş';
            else if(Char == ']') return 'T';
            else if(Char == '}') return 'U';
            else if(Char == 'ş') return 'Ü';
            else if(Char == '₺') return 'V';
            else if(Char == 'r') return 'X';
            else if(Char == '6') return 'W';
            else if(Char == '+') return 'Y';
            else if(Char == 'ç') return 'Z';

            #endregion

            #region Special Chars.

            else if(Char == 'l') return '<';
            else if(Char == 'C') return '>';
            else if(Char == 'Ğ') return '@';
            else if(Char == '5') return ':';
            else if(Char == '\\') return '¸';
            else if(Char == 'J') return '*';
            else if(Char == 'M') return '\\';
            else if(Char == '¡') return '/';
            else if(Char == '(') return '-';
            else if(Char == 'n') return '_';
            else if(Char == '·') return '=';
            else if(Char == 'E') return ']';
            else if(Char == 'L') return '[';
            else if(Char == 'ı') return '}';
            else if(Char == 'O') return '}';
            else if(Char == 'P') return '$';
            else if(Char == 'Q') return '+';
            else if(Char == 'K') return '!';
            else if(Char == 'R') return '\"';
            else if(Char == 'T') return '.';
            else if(Char == 'S') return 'ß';
            else if(Char == 'Ö') return '₺';
            else if(Char == 'm') return '?';
            else if(Char == '7') return ',';
            else if(Char == '-') return ')';
            else if(Char == 'I') return '(';
            else if(Char == 'N') return '&';
            else if(Char == ')') return '%';
            else if(Char == 'Ç') return '#';
            else if(Char == 'İ') return '~';
            else if(Char == '@') return '^';
            else if(Char == 'i') return '£';
            else if(Char == '8') return 'é';
            else if(Char == 'g') return '¥';
            else if(Char == 'q') return '|';
            else if(Char == 'U') return 'æ';
            else if(Char == 'X') return '`';
            else if(Char == 'W') return '¢';
            else if(Char == 'Z') return '¶';
            else if(Char == 'Y') return '¯';
            else if(Char == 'Ü') return '´';

            #endregion

            return Char;
        }
    }
}
