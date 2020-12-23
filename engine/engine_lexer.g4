//
// MIT License
//
// Copyright (c) 2020 Mertcan Davulcu
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
// IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
// DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR
// OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE
// OR OTHER DEALINGS IN THE SOFTWARE.

lexer grammar engine_lexer;

EXTENSION : '.mhdb' ;

// Data Types
DT_STRING : 'String' ;
DT_16BIT_INTEGER : 'Int16' ;
DT_32BIT_INTEGER : 'Int32' ;
DT_64BIT_INTEGER : 'Int64' ;
DT_16BIT_UNSIGNED_INTEGER : 'UInt16' ;
DT_32BIT_UNSIGNED_INTEGER : 'UInt32' ;
DT_64BIT_UNSIGNED_INTEGER : 'UInt64' ;
DT_DOUBLE : 'Double' ;
DT_FLOAT : 'Float' ;
DT_DECIMAL : 'Decimal' ;
DT_BYTE : 'Byte' ;
DT_DATE_TIME : 'DateTime' ;
DT_SIGNED_BYTE : 'SByte' ;
DT_BOOLEAN : 'Boolean' ;
DT_CHAR : 'Char' ;
DT_AUTO_INTEGER : 'AutoInt' ;
DT_UNIQUE : 'Unique' ;
DT_BINARY_DIGIT : 'Bit' ;
