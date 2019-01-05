parser grammar BabyScriptParser;


options
{
    language=CSharp;
    tokenVocab=BabyScriptLexer;
}

@header
{
    using XBabyScript.Compile;
}

document
    : nodeList+=node+
    ;

expr
    : IF expr THEN expr (ELSE expr)?
	| PLUS expr
    | MINUS expr
    | TABLE OPBRACKET tableDef? CLBRACKET //table initialiser
    | (NOT|TYPEOF) expr //not, typeof
    | (SIN|COS|SQRT) OPPAREN expr CLPAREN //sin, cos, sqrt
    | expr TIMES expr
    | expr DIVIDE expr
    | expr MODULO expr
    | expr PLUS expr
    | expr MINUS expr
    | expr HAT expr
    | expr (LT|LT2) expr
    | expr (LE|LE2) expr
    | expr (GT|GT2) expr
    | expr (GE|GE2) expr
    | expr EQUAL expr
    | expr NEQUAL expr
    | expr AND expr
    | expr OR expr
    | expr QUESTIONMARK
    | AT? lookupChain //atom and optional lookups
    ;
    
exprEof
    : expr EOF
    ;
    
tableDef
    : tableRowDef (COMMA tableRowDef)*
    ;
    
tableRowDef
    : ( ID | OPBRACE expr CLBRACE ) EQUALS expr
    ;

squareList
    : OPBRACKET (expr (COMMA expr)*)? CLBRACKET
    ;

braceList
    : OPBRACE (expr (COMMA expr)*)? CLBRACE
    ;
    
atom
    : NUMBER
    | SINGLE_QUOTE_STRING
    | squareList
    | braceList
    | OPPAREN expr CLPAREN UNITCAST? //subexpression with optional type cast
	| {BabyScriptCompiler.IdRegex.Match(CurrentToken.Text).Success}? . //this is so messy
    ;

elementName
    : {BabyScriptCompiler.IdRegex.Match(CurrentToken.Text).Success}? .
    ;

node
    : eleName=elementName elementAttributes elementChildren # Element
    | leftHand=lookupChain EQUALS rightHand=expr SEMICOLON # Assign
	| leftHand=lookupChain PLUSPLUS SEMICOLON # Increment
	| leftHand=lookupChain MINUSMINUS SEMICOLON # Decrement
    | leftHand=lookupChain PLUSEQUALS rightHand=expr SEMICOLON # AdditionAssign
    | leftHand=lookupChain MINUSEQUALS rightHand=expr SEMICOLON # SubtractionAssign
    | textValue=DOUBLE_QUOTE_STRING SEMICOLON # Text
	| commentText=BLOCK_COMMENT # BlockComment
    | commentText=SLASH_COMMENT # SlashComment
    ;

elementAttributes
    : OPPAREN (rawList+=attribute (COMMA rawList+=attribute)*)? CLPAREN
    |
    ;

attribute
    : {BabyScriptCompiler.IdRegex.Match(CurrentToken.Text).Success}? attrName=. COLON value=attributeValue
    | value=attributeValue
    ;

attributeValue
    : exprValue=expr
    | exprLiteral=DOUBLE_QUOTE_STRING
    ;

elementChildren
    : SEMICOLON inlineComment=SLASH_COMMENT?
    | inlineComment=SLASH_COMMENT? OPBRACE rawList+=node* CLBRACE
    ;

lookupChain
    : atom (DOT atom)*
    ;