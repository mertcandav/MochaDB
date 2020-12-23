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

lexer grammar mhql_lexer;

// Type names
ANY : 'any' ;
BOOL : 'boolean' ;
INDEX : 'integer32' ;
CHAR : 'char';
STRING : 'string';
ARITHMETIC : 'arithmetic' ;

// Main Keywords
USE : 'USE' ;
ORDER_BY: 'ORDERBY' ;
MUST : 'MUST' ;
GROUP_BY : 'GROUPBY' ;
SUB_ROW : 'SUBROW' ;
SUB_COL : 'SUBCOL' ;
DELETE_ROW : 'DELROW' ;
ADD_ROW : 'ADDROW' ;
COLUMN_ORDER_BY : 'CORDERBY' ;
 
// Secondary Keywords
AND : 'AND' ;
ASCENDING : 'ASC' | '' ;
DESCENDING : 'DESC' ;
FROM : 'FROM' ;
AS : 'AS' ;
IN : 'IN' ;
IN_EQUALS : 'INEQ' ;

// Function of USE keyword
// For GROUPBY
COUNT : 'COUNT' ;
SUM : 'SUM' ;
MAXIMUM : 'MAX' ;
MINIMUM : 'MIN' ;
AVERAGE : 'AVG' ;

// Functions of MUST keyword
BETWEEN : 'BETWEEN' ;
BIGGER : 'BIGGER' ;
LOWER : 'LOWER' ;
EQUALS : 'EQUAL' ;
NOT_EQUALS : 'NOTEQUAL' ;
STARTS_WITH : 'STARTW' ;
ENDS_WITH : 'ENDW' ;
CONTAINS : 'CONTAINS' ;
NOT_CONTAINS : 'NOTCONTAINS' ;
NOT_STARTS_WITH : 'NOTSTARTW' ;
NOT_ENDS_WITH : 'NOTENDW' ;

// Literals
BoolLiteral : 'TRUE' | 'FALSE' ;
IntLiteral : [0-9]+ ;
NullLiteral : 'NULL';
StringLiteral : '"' StringCharacters? '"' ;
fragment StringCharacters : StringCharacter+ ;
fragment StringCharacter : ~['\\'] | EscapeSequence ;
fragment EscapeSequence : '\\' . ;

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
OP_EQUALS_TO : '==' ;
OP_NOT_EQUALS_TO : '!=' ;
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

// Identifiers
Iden : PLetter PLetterOrDigit* ;
fragment PLetter : [a-zA-Z_] ;
fragment PLetterOrDigit : [a-zA-Z0-9_] ;

// Non-code regions
Whitespace : [ \t\r\n\f]+ -> skip ;
BlockComment : '/*' .*? '*/' -> channel(HIDDEN) ;
LineComment : '//' ~[\r\n]* -> channel(HIDDEN) ;
