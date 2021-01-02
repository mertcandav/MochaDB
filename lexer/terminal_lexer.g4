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

lexer grammar terminal_lexer;

// Script Keywords
WORK : 'work' ;
WORKFLOW : 'workflow' ;

// Modules
CHANGE_DIRECTORY : 'cd' ;
LIST_DIRECTORY : 'ls' ;
VERSION : 'ver' ;
ENGINE : 'eng' ;
CREATE_NEW_DATABASE : 'make' ;
CONNECT_TO_DATABASE : 'connect' ;
CLEAR_SCREEN : 'clear' ;
HELP : 'help' ;
EXIT_TERMINAL : 'exit' ;

// -------------------------------------------------------

// Symbols
ALL    : '*' ;
PARAM_DELIMITER : ',' ;
FUNCPARAM_DELIMITER : ',' ;
SUBCALL_DELIMITER : '.' ;
CACHE_OBJECT : '$' ;
HASTAG : '#' ;
LPAR : '(' ;
RPAR : ')' ;
COLON : ':' ;
COMMA : ',' ;
SEMICOLON : ';' ;
PLUS : '+' ;
MINUS : '-' ;
STAR : '*' ;
SLASH : '/' ;
VBAR : '|' ;
AMPER : '&' ;
LESS : '<' ;
GREATER : '>' ;
EQUAL : '=' ;
DOT : '.' ;
PERCENT : '%' ;
LBRACE : '{' ;
RBRACE : '}' ;
EQEQUAL : '==' ;
NOTEQUAL : '!=' ;
LESSEQUAL : '<=' ;
GREATEREQUAL : '>=' ;
TILDE : '~' ;
CIRCUMFLEX : '^' ;
LEFTSHIFT : '<<' ;
RIGHTSHIFT : '>>' ;
DOUBLESTAR : '**' ;
PLUSEQUAL : '+=' ;
MINEQUAL : '-=' ;
SLASHEQUAL : '/=' ;
PERCENTEQUAL : '%=' ;
AMPEREQUAL : '&=' ;
VBAREQUAL : '|=' ;
CIRCUMFLEXEQUAL : '^=' ;
LEFTSHIFTEQUAL : '<<=' ;
RIGHTSHIFTEQUAL : '>>=' ;
DOUBLESTAREQUAL : '**=' ;
DOUBLESLASHEQUAL : '//=' ;
AT : '@' ;
ATEQUAL : '@=' ;
LARROW : '<-' ;
RARROW : '->' ;
ELLIPSIS : '...' ;
COLONEQUAL : ':=' ;
DOLLAR : '$' ;
DOLLAREQUAL : '$=' ;

// Condition Operators
OP_EQUALS_TO : '=' ;
OP_NOT_EQUALS_TO : '<>' ;
OP_BIGGER_THAN_OR_EQUALS : '>=' ;
OP_BIGGER_THAN : '>' ;
OP_LOWER_THAN_OR_EQUALS : '<=' ;
OP_LOWER_THAN : '<' ;

// Conditions Types
NONE : '' ;
EQUALS_TO : 'EQUAL' ;
NOT_EQUALS_TO : 'NOTEQUAL' ;
BIGGER_THAN : 'BIGGER' ;
LOWER_THAN : 'LOWER' ;
BIGGER_THAN_OR_EQUALS : 'BIGGEREQ' ;
LOWER_THAN_OR_EQUALS : 'LOWEREQ' ;

// Char Escape Sequences
SEQ_DOUBLE_QUOTE : '\\\\\"' ;
SEQ_QUOTE : '\\\\\'' ;
SEQ_NEW_LINE : '\\\\n' ;
SEQ_CARRIAGE_RETURN : '\\\\r' ;
SEQ_HORIZONTAL_TAB : '\\\\t' ;
SEQ_BACKSPACE : '\\\\b' ;
SEQ_FORM_FEED : '\\\\f' ;
SEQ_BELL_ALERT : '\\\\a' ;
SEQ_VERTICAL_TAB : '\\\\v' ;
