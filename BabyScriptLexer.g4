lexer grammar BabyScriptLexer;

channels {
    WHITESPACE
}

@members {
    private int parenDepth = 0;
}

OPPAREN : '(' { parenDepth++; };
CLPAREN : ')' { parenDepth--; };

OPBRACKET : '[' ;
CLBRACKET : ']' ;

OPBRACE : '{' ;
CLBRACE : '}' ;

BLOCK_COMMENT : '/*' .*? '*/' ;
SLASH_COMMENT : '//' ~('\r' | '\n')* ;
LINE_BREAK : '\r'? '\n' -> channel(WHITESPACE); 
WS : [ \t\r\n]+ -> channel(WHITESPACE) ;

PLUSPLUS : '++';
MINUSMINUS : '--';
PLUSEQUALS : '+=';
MINUSEQUALS : '-=';

EQUALS : '=' ;
SEMICOLON : ';' ;

PLUS : '+';
MINUS : '-';
TIMES : '*';
DIVIDE : '/';
MODULO : '%';
LT : '<';
LT2 : 'lt';
LE : '<=';
LE2 : 'le';
GT : '>';
GT2 : 'gt';
GE : '>=';
GE2 : 'ge';
EQUAL : '==';
NEQUAL : '!=';
AND : 'and';
OR : 'or';
COLON : ':';
COMMA : ',' ;
DOT : '.' ;
QUESTIONMARK : '?' ;
AT : '@' ;
HAT : '^' ;

fragment UNIT
    : 'i'
    | 'L'
    | 'f'
    | 'LF'
    | 'ct'
    | 'Cr'
    | 'm'
    | 'km'
    | 'deg'
    | 'rad'
    | 'hp'
    | 'ms'
    | 's'
    | 'min'
    | 'h';

NUMBER : '-'? ('0'..'9')+ ('.' ('0' .. '9')*)? ('e' ('0' .. '9')+)? UNIT?;
UNITCAST : UNIT;

NOT :           'not';
TYPEOF :        'typeof';

SIN :           'sin';
COS :           'cos';
SQRT :          'sqrt';

IF :            'if';
THEN :          'then';
ELSE :          'else';

TABLE :         'table';
DELETE :        'delete';

ID : '$'?[A-Za-z] [A-Za-z0-9_]* ;

DOUBLE_QUOTE_STRING : '"' (~('\\'|'"') | '\\'. )* '"' ;
SINGLE_QUOTE_STRING : '\'' (~('\\'|'\'') | '\\'. )* '\'' ;