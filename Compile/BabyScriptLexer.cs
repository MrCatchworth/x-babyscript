//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     ANTLR Version: 4.7.1
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

// Generated from c:\Users\Johnny\Documents\programming\XRebirthBabyScript.Core\BabyScriptLexer.g4 by ANTLR 4.7.1

// Unreachable code detected
#pragma warning disable 0162
// The variable '...' is assigned but its value is never used
#pragma warning disable 0219
// Missing XML comment for publicly visible type or member '...'
#pragma warning disable 1591
// Ambiguous reference in cref attribute
#pragma warning disable 419

namespace XBabyScript.Compile {
using System;
using System.IO;
using System.Text;
using Antlr4.Runtime;
using Antlr4.Runtime.Atn;
using Antlr4.Runtime.Misc;
using DFA = Antlr4.Runtime.Dfa.DFA;

[System.CodeDom.Compiler.GeneratedCode("ANTLR", "4.7.1")]
[System.CLSCompliant(false)]
public partial class BabyScriptLexer : Lexer {
	protected static DFA[] decisionToDFA;
	protected static PredictionContextCache sharedContextCache = new PredictionContextCache();
	public const int
		OPPAREN=1, CLPAREN=2, OPBRACKET=3, CLBRACKET=4, OPBRACE=5, CLBRACE=6, 
		BLOCK_COMMENT=7, SLASH_COMMENT=8, LINE_BREAK=9, WS=10, PLUSPLUS=11, MINUSMINUS=12, 
		PLUSEQUALS=13, MINUSEQUALS=14, EQUALS=15, SEMICOLON=16, PLUS=17, MINUS=18, 
		TIMES=19, DIVIDE=20, MODULO=21, LT=22, LT2=23, LE=24, LE2=25, GT=26, GT2=27, 
		GE=28, GE2=29, EQUAL=30, NEQUAL=31, AND=32, OR=33, COLON=34, COMMA=35, 
		DOT=36, QUESTIONMARK=37, AT=38, HAT=39, NUMBER=40, UNITCAST=41, NOT=42, 
		TYPEOF=43, SIN=44, COS=45, SQRT=46, IF=47, THEN=48, ELSE=49, TABLE=50, 
		ID=51, DOUBLE_QUOTE_STRING=52, SINGLE_QUOTE_STRING=53;
	public const int
		WHITESPACE=2;
	public static string[] channelNames = {
		"DEFAULT_TOKEN_CHANNEL", "HIDDEN", "WHITESPACE"
	};

	public static string[] modeNames = {
		"DEFAULT_MODE"
	};

	public static readonly string[] ruleNames = {
		"OPPAREN", "CLPAREN", "OPBRACKET", "CLBRACKET", "OPBRACE", "CLBRACE", 
		"BLOCK_COMMENT", "SLASH_COMMENT", "LINE_BREAK", "WS", "PLUSPLUS", "MINUSMINUS", 
		"PLUSEQUALS", "MINUSEQUALS", "EQUALS", "SEMICOLON", "PLUS", "MINUS", "TIMES", 
		"DIVIDE", "MODULO", "LT", "LT2", "LE", "LE2", "GT", "GT2", "GE", "GE2", 
		"EQUAL", "NEQUAL", "AND", "OR", "COLON", "COMMA", "DOT", "QUESTIONMARK", 
		"AT", "HAT", "UNIT", "NUMBER", "UNITCAST", "NOT", "TYPEOF", "SIN", "COS", 
		"SQRT", "IF", "THEN", "ELSE", "TABLE", "ID", "DOUBLE_QUOTE_STRING", "SINGLE_QUOTE_STRING"
	};


	public BabyScriptLexer(ICharStream input)
	: this(input, Console.Out, Console.Error) { }

	public BabyScriptLexer(ICharStream input, TextWriter output, TextWriter errorOutput)
	: base(input, output, errorOutput)
	{
		Interpreter = new LexerATNSimulator(this, _ATN, decisionToDFA, sharedContextCache);
	}

	private static readonly string[] _LiteralNames = {
		null, "'('", "')'", "'['", "']'", "'{'", "'}'", null, null, null, null, 
		"'++'", "'--'", "'+='", "'-='", "'='", "';'", "'+'", "'-'", "'*'", "'/'", 
		"'%'", "'<'", "'lt'", "'<='", "'le'", "'>'", "'gt'", "'>='", "'ge'", "'=='", 
		"'!='", "'and'", "'or'", "':'", "','", "'.'", "'?'", "'@'", "'^'", null, 
		null, "'not'", "'typeof'", "'sin'", "'cos'", "'sqrt'", "'if'", "'then'", 
		"'else'", "'table'"
	};
	private static readonly string[] _SymbolicNames = {
		null, "OPPAREN", "CLPAREN", "OPBRACKET", "CLBRACKET", "OPBRACE", "CLBRACE", 
		"BLOCK_COMMENT", "SLASH_COMMENT", "LINE_BREAK", "WS", "PLUSPLUS", "MINUSMINUS", 
		"PLUSEQUALS", "MINUSEQUALS", "EQUALS", "SEMICOLON", "PLUS", "MINUS", "TIMES", 
		"DIVIDE", "MODULO", "LT", "LT2", "LE", "LE2", "GT", "GT2", "GE", "GE2", 
		"EQUAL", "NEQUAL", "AND", "OR", "COLON", "COMMA", "DOT", "QUESTIONMARK", 
		"AT", "HAT", "NUMBER", "UNITCAST", "NOT", "TYPEOF", "SIN", "COS", "SQRT", 
		"IF", "THEN", "ELSE", "TABLE", "ID", "DOUBLE_QUOTE_STRING", "SINGLE_QUOTE_STRING"
	};
	public static readonly IVocabulary DefaultVocabulary = new Vocabulary(_LiteralNames, _SymbolicNames);

	[NotNull]
	public override IVocabulary Vocabulary
	{
		get
		{
			return DefaultVocabulary;
		}
	}

	public override string GrammarFileName { get { return "BabyScriptLexer.g4"; } }

	public override string[] RuleNames { get { return ruleNames; } }

	public override string[] ChannelNames { get { return channelNames; } }

	public override string[] ModeNames { get { return modeNames; } }

	public override string SerializedAtn { get { return new string(_serializedATN); } }

	static BabyScriptLexer() {
		decisionToDFA = new DFA[_ATN.NumberOfDecisions];
		for (int i = 0; i < _ATN.NumberOfDecisions; i++) {
			decisionToDFA[i] = new DFA(_ATN.GetDecisionState(i), i);
		}
	}
	private static char[] _serializedATN = {
		'\x3', '\x608B', '\xA72A', '\x8133', '\xB9ED', '\x417C', '\x3BE7', '\x7786', 
		'\x5964', '\x2', '\x37', '\x16B', '\b', '\x1', '\x4', '\x2', '\t', '\x2', 
		'\x4', '\x3', '\t', '\x3', '\x4', '\x4', '\t', '\x4', '\x4', '\x5', '\t', 
		'\x5', '\x4', '\x6', '\t', '\x6', '\x4', '\a', '\t', '\a', '\x4', '\b', 
		'\t', '\b', '\x4', '\t', '\t', '\t', '\x4', '\n', '\t', '\n', '\x4', '\v', 
		'\t', '\v', '\x4', '\f', '\t', '\f', '\x4', '\r', '\t', '\r', '\x4', '\xE', 
		'\t', '\xE', '\x4', '\xF', '\t', '\xF', '\x4', '\x10', '\t', '\x10', '\x4', 
		'\x11', '\t', '\x11', '\x4', '\x12', '\t', '\x12', '\x4', '\x13', '\t', 
		'\x13', '\x4', '\x14', '\t', '\x14', '\x4', '\x15', '\t', '\x15', '\x4', 
		'\x16', '\t', '\x16', '\x4', '\x17', '\t', '\x17', '\x4', '\x18', '\t', 
		'\x18', '\x4', '\x19', '\t', '\x19', '\x4', '\x1A', '\t', '\x1A', '\x4', 
		'\x1B', '\t', '\x1B', '\x4', '\x1C', '\t', '\x1C', '\x4', '\x1D', '\t', 
		'\x1D', '\x4', '\x1E', '\t', '\x1E', '\x4', '\x1F', '\t', '\x1F', '\x4', 
		' ', '\t', ' ', '\x4', '!', '\t', '!', '\x4', '\"', '\t', '\"', '\x4', 
		'#', '\t', '#', '\x4', '$', '\t', '$', '\x4', '%', '\t', '%', '\x4', '&', 
		'\t', '&', '\x4', '\'', '\t', '\'', '\x4', '(', '\t', '(', '\x4', ')', 
		'\t', ')', '\x4', '*', '\t', '*', '\x4', '+', '\t', '+', '\x4', ',', '\t', 
		',', '\x4', '-', '\t', '-', '\x4', '.', '\t', '.', '\x4', '/', '\t', '/', 
		'\x4', '\x30', '\t', '\x30', '\x4', '\x31', '\t', '\x31', '\x4', '\x32', 
		'\t', '\x32', '\x4', '\x33', '\t', '\x33', '\x4', '\x34', '\t', '\x34', 
		'\x4', '\x35', '\t', '\x35', '\x4', '\x36', '\t', '\x36', '\x4', '\x37', 
		'\t', '\x37', '\x3', '\x2', '\x3', '\x2', '\x3', '\x3', '\x3', '\x3', 
		'\x3', '\x4', '\x3', '\x4', '\x3', '\x5', '\x3', '\x5', '\x3', '\x6', 
		'\x3', '\x6', '\x3', '\a', '\x3', '\a', '\x3', '\b', '\x3', '\b', '\x3', 
		'\b', '\x3', '\b', '\a', '\b', '\x80', '\n', '\b', '\f', '\b', '\xE', 
		'\b', '\x83', '\v', '\b', '\x3', '\b', '\x3', '\b', '\x3', '\b', '\x3', 
		'\t', '\x3', '\t', '\x3', '\t', '\x3', '\t', '\a', '\t', '\x8C', '\n', 
		'\t', '\f', '\t', '\xE', '\t', '\x8F', '\v', '\t', '\x3', '\n', '\x5', 
		'\n', '\x92', '\n', '\n', '\x3', '\n', '\x3', '\n', '\x3', '\n', '\x3', 
		'\n', '\x3', '\v', '\x6', '\v', '\x99', '\n', '\v', '\r', '\v', '\xE', 
		'\v', '\x9A', '\x3', '\v', '\x3', '\v', '\x3', '\f', '\x3', '\f', '\x3', 
		'\f', '\x3', '\r', '\x3', '\r', '\x3', '\r', '\x3', '\xE', '\x3', '\xE', 
		'\x3', '\xE', '\x3', '\xF', '\x3', '\xF', '\x3', '\xF', '\x3', '\x10', 
		'\x3', '\x10', '\x3', '\x11', '\x3', '\x11', '\x3', '\x12', '\x3', '\x12', 
		'\x3', '\x13', '\x3', '\x13', '\x3', '\x14', '\x3', '\x14', '\x3', '\x15', 
		'\x3', '\x15', '\x3', '\x16', '\x3', '\x16', '\x3', '\x17', '\x3', '\x17', 
		'\x3', '\x18', '\x3', '\x18', '\x3', '\x18', '\x3', '\x19', '\x3', '\x19', 
		'\x3', '\x19', '\x3', '\x1A', '\x3', '\x1A', '\x3', '\x1A', '\x3', '\x1B', 
		'\x3', '\x1B', '\x3', '\x1C', '\x3', '\x1C', '\x3', '\x1C', '\x3', '\x1D', 
		'\x3', '\x1D', '\x3', '\x1D', '\x3', '\x1E', '\x3', '\x1E', '\x3', '\x1E', 
		'\x3', '\x1F', '\x3', '\x1F', '\x3', '\x1F', '\x3', ' ', '\x3', ' ', '\x3', 
		' ', '\x3', '!', '\x3', '!', '\x3', '!', '\x3', '!', '\x3', '\"', '\x3', 
		'\"', '\x3', '\"', '\x3', '#', '\x3', '#', '\x3', '$', '\x3', '$', '\x3', 
		'%', '\x3', '%', '\x3', '&', '\x3', '&', '\x3', '\'', '\x3', '\'', '\x3', 
		'(', '\x3', '(', '\x3', ')', '\x3', ')', '\x3', ')', '\x3', ')', '\x3', 
		')', '\x3', ')', '\x3', ')', '\x3', ')', '\x3', ')', '\x3', ')', '\x3', 
		')', '\x3', ')', '\x3', ')', '\x3', ')', '\x3', ')', '\x3', ')', '\x3', 
		')', '\x3', ')', '\x3', ')', '\x3', ')', '\x3', ')', '\x3', ')', '\x3', 
		')', '\x3', ')', '\x3', ')', '\x5', ')', '\x101', '\n', ')', '\x3', '*', 
		'\x5', '*', '\x104', '\n', '*', '\x3', '*', '\x6', '*', '\x107', '\n', 
		'*', '\r', '*', '\xE', '*', '\x108', '\x3', '*', '\x3', '*', '\a', '*', 
		'\x10D', '\n', '*', '\f', '*', '\xE', '*', '\x110', '\v', '*', '\x5', 
		'*', '\x112', '\n', '*', '\x3', '*', '\x3', '*', '\x6', '*', '\x116', 
		'\n', '*', '\r', '*', '\xE', '*', '\x117', '\x5', '*', '\x11A', '\n', 
		'*', '\x3', '*', '\x5', '*', '\x11D', '\n', '*', '\x3', '+', '\x3', '+', 
		'\x3', ',', '\x3', ',', '\x3', ',', '\x3', ',', '\x3', '-', '\x3', '-', 
		'\x3', '-', '\x3', '-', '\x3', '-', '\x3', '-', '\x3', '-', '\x3', '.', 
		'\x3', '.', '\x3', '.', '\x3', '.', '\x3', '/', '\x3', '/', '\x3', '/', 
		'\x3', '/', '\x3', '\x30', '\x3', '\x30', '\x3', '\x30', '\x3', '\x30', 
		'\x3', '\x30', '\x3', '\x31', '\x3', '\x31', '\x3', '\x31', '\x3', '\x32', 
		'\x3', '\x32', '\x3', '\x32', '\x3', '\x32', '\x3', '\x32', '\x3', '\x33', 
		'\x3', '\x33', '\x3', '\x33', '\x3', '\x33', '\x3', '\x33', '\x3', '\x34', 
		'\x3', '\x34', '\x3', '\x34', '\x3', '\x34', '\x3', '\x34', '\x3', '\x34', 
		'\x3', '\x35', '\x5', '\x35', '\x14D', '\n', '\x35', '\x3', '\x35', '\x3', 
		'\x35', '\a', '\x35', '\x151', '\n', '\x35', '\f', '\x35', '\xE', '\x35', 
		'\x154', '\v', '\x35', '\x3', '\x36', '\x3', '\x36', '\x3', '\x36', '\x3', 
		'\x36', '\a', '\x36', '\x15A', '\n', '\x36', '\f', '\x36', '\xE', '\x36', 
		'\x15D', '\v', '\x36', '\x3', '\x36', '\x3', '\x36', '\x3', '\x37', '\x3', 
		'\x37', '\x3', '\x37', '\x3', '\x37', '\a', '\x37', '\x165', '\n', '\x37', 
		'\f', '\x37', '\xE', '\x37', '\x168', '\v', '\x37', '\x3', '\x37', '\x3', 
		'\x37', '\x3', '\x81', '\x2', '\x38', '\x3', '\x3', '\x5', '\x4', '\a', 
		'\x5', '\t', '\x6', '\v', '\a', '\r', '\b', '\xF', '\t', '\x11', '\n', 
		'\x13', '\v', '\x15', '\f', '\x17', '\r', '\x19', '\xE', '\x1B', '\xF', 
		'\x1D', '\x10', '\x1F', '\x11', '!', '\x12', '#', '\x13', '%', '\x14', 
		'\'', '\x15', ')', '\x16', '+', '\x17', '-', '\x18', '/', '\x19', '\x31', 
		'\x1A', '\x33', '\x1B', '\x35', '\x1C', '\x37', '\x1D', '\x39', '\x1E', 
		';', '\x1F', '=', ' ', '?', '!', '\x41', '\"', '\x43', '#', '\x45', '$', 
		'G', '%', 'I', '&', 'K', '\'', 'M', '(', 'O', ')', 'Q', '\x2', 'S', '*', 
		'U', '+', 'W', ',', 'Y', '-', '[', '.', ']', '/', '_', '\x30', '\x61', 
		'\x31', '\x63', '\x32', '\x65', '\x33', 'g', '\x34', 'i', '\x35', 'k', 
		'\x36', 'm', '\x37', '\x3', '\x2', '\t', '\x4', '\x2', '\f', '\f', '\xF', 
		'\xF', '\x5', '\x2', '\v', '\f', '\xF', '\xF', '\"', '\"', '\x5', '\x2', 
		'N', 'N', 'h', 'h', 'k', 'k', '\x4', '\x2', '\x43', '\\', '\x63', '|', 
		'\x6', '\x2', '\x32', ';', '\x43', '\\', '\x61', '\x61', '\x63', '|', 
		'\x4', '\x2', '$', '$', '^', '^', '\x4', '\x2', ')', ')', '^', '^', '\x2', 
		'\x186', '\x2', '\x3', '\x3', '\x2', '\x2', '\x2', '\x2', '\x5', '\x3', 
		'\x2', '\x2', '\x2', '\x2', '\a', '\x3', '\x2', '\x2', '\x2', '\x2', '\t', 
		'\x3', '\x2', '\x2', '\x2', '\x2', '\v', '\x3', '\x2', '\x2', '\x2', '\x2', 
		'\r', '\x3', '\x2', '\x2', '\x2', '\x2', '\xF', '\x3', '\x2', '\x2', '\x2', 
		'\x2', '\x11', '\x3', '\x2', '\x2', '\x2', '\x2', '\x13', '\x3', '\x2', 
		'\x2', '\x2', '\x2', '\x15', '\x3', '\x2', '\x2', '\x2', '\x2', '\x17', 
		'\x3', '\x2', '\x2', '\x2', '\x2', '\x19', '\x3', '\x2', '\x2', '\x2', 
		'\x2', '\x1B', '\x3', '\x2', '\x2', '\x2', '\x2', '\x1D', '\x3', '\x2', 
		'\x2', '\x2', '\x2', '\x1F', '\x3', '\x2', '\x2', '\x2', '\x2', '!', '\x3', 
		'\x2', '\x2', '\x2', '\x2', '#', '\x3', '\x2', '\x2', '\x2', '\x2', '%', 
		'\x3', '\x2', '\x2', '\x2', '\x2', '\'', '\x3', '\x2', '\x2', '\x2', '\x2', 
		')', '\x3', '\x2', '\x2', '\x2', '\x2', '+', '\x3', '\x2', '\x2', '\x2', 
		'\x2', '-', '\x3', '\x2', '\x2', '\x2', '\x2', '/', '\x3', '\x2', '\x2', 
		'\x2', '\x2', '\x31', '\x3', '\x2', '\x2', '\x2', '\x2', '\x33', '\x3', 
		'\x2', '\x2', '\x2', '\x2', '\x35', '\x3', '\x2', '\x2', '\x2', '\x2', 
		'\x37', '\x3', '\x2', '\x2', '\x2', '\x2', '\x39', '\x3', '\x2', '\x2', 
		'\x2', '\x2', ';', '\x3', '\x2', '\x2', '\x2', '\x2', '=', '\x3', '\x2', 
		'\x2', '\x2', '\x2', '?', '\x3', '\x2', '\x2', '\x2', '\x2', '\x41', '\x3', 
		'\x2', '\x2', '\x2', '\x2', '\x43', '\x3', '\x2', '\x2', '\x2', '\x2', 
		'\x45', '\x3', '\x2', '\x2', '\x2', '\x2', 'G', '\x3', '\x2', '\x2', '\x2', 
		'\x2', 'I', '\x3', '\x2', '\x2', '\x2', '\x2', 'K', '\x3', '\x2', '\x2', 
		'\x2', '\x2', 'M', '\x3', '\x2', '\x2', '\x2', '\x2', 'O', '\x3', '\x2', 
		'\x2', '\x2', '\x2', 'S', '\x3', '\x2', '\x2', '\x2', '\x2', 'U', '\x3', 
		'\x2', '\x2', '\x2', '\x2', 'W', '\x3', '\x2', '\x2', '\x2', '\x2', 'Y', 
		'\x3', '\x2', '\x2', '\x2', '\x2', '[', '\x3', '\x2', '\x2', '\x2', '\x2', 
		']', '\x3', '\x2', '\x2', '\x2', '\x2', '_', '\x3', '\x2', '\x2', '\x2', 
		'\x2', '\x61', '\x3', '\x2', '\x2', '\x2', '\x2', '\x63', '\x3', '\x2', 
		'\x2', '\x2', '\x2', '\x65', '\x3', '\x2', '\x2', '\x2', '\x2', 'g', '\x3', 
		'\x2', '\x2', '\x2', '\x2', 'i', '\x3', '\x2', '\x2', '\x2', '\x2', 'k', 
		'\x3', '\x2', '\x2', '\x2', '\x2', 'm', '\x3', '\x2', '\x2', '\x2', '\x3', 
		'o', '\x3', '\x2', '\x2', '\x2', '\x5', 'q', '\x3', '\x2', '\x2', '\x2', 
		'\a', 's', '\x3', '\x2', '\x2', '\x2', '\t', 'u', '\x3', '\x2', '\x2', 
		'\x2', '\v', 'w', '\x3', '\x2', '\x2', '\x2', '\r', 'y', '\x3', '\x2', 
		'\x2', '\x2', '\xF', '{', '\x3', '\x2', '\x2', '\x2', '\x11', '\x87', 
		'\x3', '\x2', '\x2', '\x2', '\x13', '\x91', '\x3', '\x2', '\x2', '\x2', 
		'\x15', '\x98', '\x3', '\x2', '\x2', '\x2', '\x17', '\x9E', '\x3', '\x2', 
		'\x2', '\x2', '\x19', '\xA1', '\x3', '\x2', '\x2', '\x2', '\x1B', '\xA4', 
		'\x3', '\x2', '\x2', '\x2', '\x1D', '\xA7', '\x3', '\x2', '\x2', '\x2', 
		'\x1F', '\xAA', '\x3', '\x2', '\x2', '\x2', '!', '\xAC', '\x3', '\x2', 
		'\x2', '\x2', '#', '\xAE', '\x3', '\x2', '\x2', '\x2', '%', '\xB0', '\x3', 
		'\x2', '\x2', '\x2', '\'', '\xB2', '\x3', '\x2', '\x2', '\x2', ')', '\xB4', 
		'\x3', '\x2', '\x2', '\x2', '+', '\xB6', '\x3', '\x2', '\x2', '\x2', '-', 
		'\xB8', '\x3', '\x2', '\x2', '\x2', '/', '\xBA', '\x3', '\x2', '\x2', 
		'\x2', '\x31', '\xBD', '\x3', '\x2', '\x2', '\x2', '\x33', '\xC0', '\x3', 
		'\x2', '\x2', '\x2', '\x35', '\xC3', '\x3', '\x2', '\x2', '\x2', '\x37', 
		'\xC5', '\x3', '\x2', '\x2', '\x2', '\x39', '\xC8', '\x3', '\x2', '\x2', 
		'\x2', ';', '\xCB', '\x3', '\x2', '\x2', '\x2', '=', '\xCE', '\x3', '\x2', 
		'\x2', '\x2', '?', '\xD1', '\x3', '\x2', '\x2', '\x2', '\x41', '\xD4', 
		'\x3', '\x2', '\x2', '\x2', '\x43', '\xD8', '\x3', '\x2', '\x2', '\x2', 
		'\x45', '\xDB', '\x3', '\x2', '\x2', '\x2', 'G', '\xDD', '\x3', '\x2', 
		'\x2', '\x2', 'I', '\xDF', '\x3', '\x2', '\x2', '\x2', 'K', '\xE1', '\x3', 
		'\x2', '\x2', '\x2', 'M', '\xE3', '\x3', '\x2', '\x2', '\x2', 'O', '\xE5', 
		'\x3', '\x2', '\x2', '\x2', 'Q', '\x100', '\x3', '\x2', '\x2', '\x2', 
		'S', '\x103', '\x3', '\x2', '\x2', '\x2', 'U', '\x11E', '\x3', '\x2', 
		'\x2', '\x2', 'W', '\x120', '\x3', '\x2', '\x2', '\x2', 'Y', '\x124', 
		'\x3', '\x2', '\x2', '\x2', '[', '\x12B', '\x3', '\x2', '\x2', '\x2', 
		']', '\x12F', '\x3', '\x2', '\x2', '\x2', '_', '\x133', '\x3', '\x2', 
		'\x2', '\x2', '\x61', '\x138', '\x3', '\x2', '\x2', '\x2', '\x63', '\x13B', 
		'\x3', '\x2', '\x2', '\x2', '\x65', '\x140', '\x3', '\x2', '\x2', '\x2', 
		'g', '\x145', '\x3', '\x2', '\x2', '\x2', 'i', '\x14C', '\x3', '\x2', 
		'\x2', '\x2', 'k', '\x155', '\x3', '\x2', '\x2', '\x2', 'm', '\x160', 
		'\x3', '\x2', '\x2', '\x2', 'o', 'p', '\a', '*', '\x2', '\x2', 'p', '\x4', 
		'\x3', '\x2', '\x2', '\x2', 'q', 'r', '\a', '+', '\x2', '\x2', 'r', '\x6', 
		'\x3', '\x2', '\x2', '\x2', 's', 't', '\a', ']', '\x2', '\x2', 't', '\b', 
		'\x3', '\x2', '\x2', '\x2', 'u', 'v', '\a', '_', '\x2', '\x2', 'v', '\n', 
		'\x3', '\x2', '\x2', '\x2', 'w', 'x', '\a', '}', '\x2', '\x2', 'x', '\f', 
		'\x3', '\x2', '\x2', '\x2', 'y', 'z', '\a', '\x7F', '\x2', '\x2', 'z', 
		'\xE', '\x3', '\x2', '\x2', '\x2', '{', '|', '\a', '\x31', '\x2', '\x2', 
		'|', '}', '\a', ',', '\x2', '\x2', '}', '\x81', '\x3', '\x2', '\x2', '\x2', 
		'~', '\x80', '\v', '\x2', '\x2', '\x2', '\x7F', '~', '\x3', '\x2', '\x2', 
		'\x2', '\x80', '\x83', '\x3', '\x2', '\x2', '\x2', '\x81', '\x82', '\x3', 
		'\x2', '\x2', '\x2', '\x81', '\x7F', '\x3', '\x2', '\x2', '\x2', '\x82', 
		'\x84', '\x3', '\x2', '\x2', '\x2', '\x83', '\x81', '\x3', '\x2', '\x2', 
		'\x2', '\x84', '\x85', '\a', ',', '\x2', '\x2', '\x85', '\x86', '\a', 
		'\x31', '\x2', '\x2', '\x86', '\x10', '\x3', '\x2', '\x2', '\x2', '\x87', 
		'\x88', '\a', '\x31', '\x2', '\x2', '\x88', '\x89', '\a', '\x31', '\x2', 
		'\x2', '\x89', '\x8D', '\x3', '\x2', '\x2', '\x2', '\x8A', '\x8C', '\n', 
		'\x2', '\x2', '\x2', '\x8B', '\x8A', '\x3', '\x2', '\x2', '\x2', '\x8C', 
		'\x8F', '\x3', '\x2', '\x2', '\x2', '\x8D', '\x8B', '\x3', '\x2', '\x2', 
		'\x2', '\x8D', '\x8E', '\x3', '\x2', '\x2', '\x2', '\x8E', '\x12', '\x3', 
		'\x2', '\x2', '\x2', '\x8F', '\x8D', '\x3', '\x2', '\x2', '\x2', '\x90', 
		'\x92', '\a', '\xF', '\x2', '\x2', '\x91', '\x90', '\x3', '\x2', '\x2', 
		'\x2', '\x91', '\x92', '\x3', '\x2', '\x2', '\x2', '\x92', '\x93', '\x3', 
		'\x2', '\x2', '\x2', '\x93', '\x94', '\a', '\f', '\x2', '\x2', '\x94', 
		'\x95', '\x3', '\x2', '\x2', '\x2', '\x95', '\x96', '\b', '\n', '\x2', 
		'\x2', '\x96', '\x14', '\x3', '\x2', '\x2', '\x2', '\x97', '\x99', '\t', 
		'\x3', '\x2', '\x2', '\x98', '\x97', '\x3', '\x2', '\x2', '\x2', '\x99', 
		'\x9A', '\x3', '\x2', '\x2', '\x2', '\x9A', '\x98', '\x3', '\x2', '\x2', 
		'\x2', '\x9A', '\x9B', '\x3', '\x2', '\x2', '\x2', '\x9B', '\x9C', '\x3', 
		'\x2', '\x2', '\x2', '\x9C', '\x9D', '\b', '\v', '\x2', '\x2', '\x9D', 
		'\x16', '\x3', '\x2', '\x2', '\x2', '\x9E', '\x9F', '\a', '-', '\x2', 
		'\x2', '\x9F', '\xA0', '\a', '-', '\x2', '\x2', '\xA0', '\x18', '\x3', 
		'\x2', '\x2', '\x2', '\xA1', '\xA2', '\a', '/', '\x2', '\x2', '\xA2', 
		'\xA3', '\a', '/', '\x2', '\x2', '\xA3', '\x1A', '\x3', '\x2', '\x2', 
		'\x2', '\xA4', '\xA5', '\a', '-', '\x2', '\x2', '\xA5', '\xA6', '\a', 
		'?', '\x2', '\x2', '\xA6', '\x1C', '\x3', '\x2', '\x2', '\x2', '\xA7', 
		'\xA8', '\a', '/', '\x2', '\x2', '\xA8', '\xA9', '\a', '?', '\x2', '\x2', 
		'\xA9', '\x1E', '\x3', '\x2', '\x2', '\x2', '\xAA', '\xAB', '\a', '?', 
		'\x2', '\x2', '\xAB', ' ', '\x3', '\x2', '\x2', '\x2', '\xAC', '\xAD', 
		'\a', '=', '\x2', '\x2', '\xAD', '\"', '\x3', '\x2', '\x2', '\x2', '\xAE', 
		'\xAF', '\a', '-', '\x2', '\x2', '\xAF', '$', '\x3', '\x2', '\x2', '\x2', 
		'\xB0', '\xB1', '\a', '/', '\x2', '\x2', '\xB1', '&', '\x3', '\x2', '\x2', 
		'\x2', '\xB2', '\xB3', '\a', ',', '\x2', '\x2', '\xB3', '(', '\x3', '\x2', 
		'\x2', '\x2', '\xB4', '\xB5', '\a', '\x31', '\x2', '\x2', '\xB5', '*', 
		'\x3', '\x2', '\x2', '\x2', '\xB6', '\xB7', '\a', '\'', '\x2', '\x2', 
		'\xB7', ',', '\x3', '\x2', '\x2', '\x2', '\xB8', '\xB9', '\a', '>', '\x2', 
		'\x2', '\xB9', '.', '\x3', '\x2', '\x2', '\x2', '\xBA', '\xBB', '\a', 
		'n', '\x2', '\x2', '\xBB', '\xBC', '\a', 'v', '\x2', '\x2', '\xBC', '\x30', 
		'\x3', '\x2', '\x2', '\x2', '\xBD', '\xBE', '\a', '>', '\x2', '\x2', '\xBE', 
		'\xBF', '\a', '?', '\x2', '\x2', '\xBF', '\x32', '\x3', '\x2', '\x2', 
		'\x2', '\xC0', '\xC1', '\a', 'n', '\x2', '\x2', '\xC1', '\xC2', '\a', 
		'g', '\x2', '\x2', '\xC2', '\x34', '\x3', '\x2', '\x2', '\x2', '\xC3', 
		'\xC4', '\a', '@', '\x2', '\x2', '\xC4', '\x36', '\x3', '\x2', '\x2', 
		'\x2', '\xC5', '\xC6', '\a', 'i', '\x2', '\x2', '\xC6', '\xC7', '\a', 
		'v', '\x2', '\x2', '\xC7', '\x38', '\x3', '\x2', '\x2', '\x2', '\xC8', 
		'\xC9', '\a', '@', '\x2', '\x2', '\xC9', '\xCA', '\a', '?', '\x2', '\x2', 
		'\xCA', ':', '\x3', '\x2', '\x2', '\x2', '\xCB', '\xCC', '\a', 'i', '\x2', 
		'\x2', '\xCC', '\xCD', '\a', 'g', '\x2', '\x2', '\xCD', '<', '\x3', '\x2', 
		'\x2', '\x2', '\xCE', '\xCF', '\a', '?', '\x2', '\x2', '\xCF', '\xD0', 
		'\a', '?', '\x2', '\x2', '\xD0', '>', '\x3', '\x2', '\x2', '\x2', '\xD1', 
		'\xD2', '\a', '#', '\x2', '\x2', '\xD2', '\xD3', '\a', '?', '\x2', '\x2', 
		'\xD3', '@', '\x3', '\x2', '\x2', '\x2', '\xD4', '\xD5', '\a', '\x63', 
		'\x2', '\x2', '\xD5', '\xD6', '\a', 'p', '\x2', '\x2', '\xD6', '\xD7', 
		'\a', '\x66', '\x2', '\x2', '\xD7', '\x42', '\x3', '\x2', '\x2', '\x2', 
		'\xD8', '\xD9', '\a', 'q', '\x2', '\x2', '\xD9', '\xDA', '\a', 't', '\x2', 
		'\x2', '\xDA', '\x44', '\x3', '\x2', '\x2', '\x2', '\xDB', '\xDC', '\a', 
		'<', '\x2', '\x2', '\xDC', '\x46', '\x3', '\x2', '\x2', '\x2', '\xDD', 
		'\xDE', '\a', '.', '\x2', '\x2', '\xDE', 'H', '\x3', '\x2', '\x2', '\x2', 
		'\xDF', '\xE0', '\a', '\x30', '\x2', '\x2', '\xE0', 'J', '\x3', '\x2', 
		'\x2', '\x2', '\xE1', '\xE2', '\a', '\x41', '\x2', '\x2', '\xE2', 'L', 
		'\x3', '\x2', '\x2', '\x2', '\xE3', '\xE4', '\a', '\x42', '\x2', '\x2', 
		'\xE4', 'N', '\x3', '\x2', '\x2', '\x2', '\xE5', '\xE6', '\a', '`', '\x2', 
		'\x2', '\xE6', 'P', '\x3', '\x2', '\x2', '\x2', '\xE7', '\x101', '\t', 
		'\x4', '\x2', '\x2', '\xE8', '\xE9', '\a', 'N', '\x2', '\x2', '\xE9', 
		'\x101', '\a', 'H', '\x2', '\x2', '\xEA', '\xEB', '\a', '\x65', '\x2', 
		'\x2', '\xEB', '\x101', '\a', 'v', '\x2', '\x2', '\xEC', '\xED', '\a', 
		'\x45', '\x2', '\x2', '\xED', '\x101', '\a', 't', '\x2', '\x2', '\xEE', 
		'\x101', '\a', 'o', '\x2', '\x2', '\xEF', '\xF0', '\a', 'm', '\x2', '\x2', 
		'\xF0', '\x101', '\a', 'o', '\x2', '\x2', '\xF1', '\xF2', '\a', '\x66', 
		'\x2', '\x2', '\xF2', '\xF3', '\a', 'g', '\x2', '\x2', '\xF3', '\x101', 
		'\a', 'i', '\x2', '\x2', '\xF4', '\xF5', '\a', 't', '\x2', '\x2', '\xF5', 
		'\xF6', '\a', '\x63', '\x2', '\x2', '\xF6', '\x101', '\a', '\x66', '\x2', 
		'\x2', '\xF7', '\xF8', '\a', 'j', '\x2', '\x2', '\xF8', '\x101', '\a', 
		'r', '\x2', '\x2', '\xF9', '\xFA', '\a', 'o', '\x2', '\x2', '\xFA', '\x101', 
		'\a', 'u', '\x2', '\x2', '\xFB', '\x101', '\a', 'u', '\x2', '\x2', '\xFC', 
		'\xFD', '\a', 'o', '\x2', '\x2', '\xFD', '\xFE', '\a', 'k', '\x2', '\x2', 
		'\xFE', '\x101', '\a', 'p', '\x2', '\x2', '\xFF', '\x101', '\a', 'j', 
		'\x2', '\x2', '\x100', '\xE7', '\x3', '\x2', '\x2', '\x2', '\x100', '\xE8', 
		'\x3', '\x2', '\x2', '\x2', '\x100', '\xEA', '\x3', '\x2', '\x2', '\x2', 
		'\x100', '\xEC', '\x3', '\x2', '\x2', '\x2', '\x100', '\xEE', '\x3', '\x2', 
		'\x2', '\x2', '\x100', '\xEF', '\x3', '\x2', '\x2', '\x2', '\x100', '\xF1', 
		'\x3', '\x2', '\x2', '\x2', '\x100', '\xF4', '\x3', '\x2', '\x2', '\x2', 
		'\x100', '\xF7', '\x3', '\x2', '\x2', '\x2', '\x100', '\xF9', '\x3', '\x2', 
		'\x2', '\x2', '\x100', '\xFB', '\x3', '\x2', '\x2', '\x2', '\x100', '\xFC', 
		'\x3', '\x2', '\x2', '\x2', '\x100', '\xFF', '\x3', '\x2', '\x2', '\x2', 
		'\x101', 'R', '\x3', '\x2', '\x2', '\x2', '\x102', '\x104', '\a', '/', 
		'\x2', '\x2', '\x103', '\x102', '\x3', '\x2', '\x2', '\x2', '\x103', '\x104', 
		'\x3', '\x2', '\x2', '\x2', '\x104', '\x106', '\x3', '\x2', '\x2', '\x2', 
		'\x105', '\x107', '\x4', '\x32', ';', '\x2', '\x106', '\x105', '\x3', 
		'\x2', '\x2', '\x2', '\x107', '\x108', '\x3', '\x2', '\x2', '\x2', '\x108', 
		'\x106', '\x3', '\x2', '\x2', '\x2', '\x108', '\x109', '\x3', '\x2', '\x2', 
		'\x2', '\x109', '\x111', '\x3', '\x2', '\x2', '\x2', '\x10A', '\x10E', 
		'\a', '\x30', '\x2', '\x2', '\x10B', '\x10D', '\x4', '\x32', ';', '\x2', 
		'\x10C', '\x10B', '\x3', '\x2', '\x2', '\x2', '\x10D', '\x110', '\x3', 
		'\x2', '\x2', '\x2', '\x10E', '\x10C', '\x3', '\x2', '\x2', '\x2', '\x10E', 
		'\x10F', '\x3', '\x2', '\x2', '\x2', '\x10F', '\x112', '\x3', '\x2', '\x2', 
		'\x2', '\x110', '\x10E', '\x3', '\x2', '\x2', '\x2', '\x111', '\x10A', 
		'\x3', '\x2', '\x2', '\x2', '\x111', '\x112', '\x3', '\x2', '\x2', '\x2', 
		'\x112', '\x119', '\x3', '\x2', '\x2', '\x2', '\x113', '\x115', '\a', 
		'g', '\x2', '\x2', '\x114', '\x116', '\x4', '\x32', ';', '\x2', '\x115', 
		'\x114', '\x3', '\x2', '\x2', '\x2', '\x116', '\x117', '\x3', '\x2', '\x2', 
		'\x2', '\x117', '\x115', '\x3', '\x2', '\x2', '\x2', '\x117', '\x118', 
		'\x3', '\x2', '\x2', '\x2', '\x118', '\x11A', '\x3', '\x2', '\x2', '\x2', 
		'\x119', '\x113', '\x3', '\x2', '\x2', '\x2', '\x119', '\x11A', '\x3', 
		'\x2', '\x2', '\x2', '\x11A', '\x11C', '\x3', '\x2', '\x2', '\x2', '\x11B', 
		'\x11D', '\x5', 'Q', ')', '\x2', '\x11C', '\x11B', '\x3', '\x2', '\x2', 
		'\x2', '\x11C', '\x11D', '\x3', '\x2', '\x2', '\x2', '\x11D', 'T', '\x3', 
		'\x2', '\x2', '\x2', '\x11E', '\x11F', '\x5', 'Q', ')', '\x2', '\x11F', 
		'V', '\x3', '\x2', '\x2', '\x2', '\x120', '\x121', '\a', 'p', '\x2', '\x2', 
		'\x121', '\x122', '\a', 'q', '\x2', '\x2', '\x122', '\x123', '\a', 'v', 
		'\x2', '\x2', '\x123', 'X', '\x3', '\x2', '\x2', '\x2', '\x124', '\x125', 
		'\a', 'v', '\x2', '\x2', '\x125', '\x126', '\a', '{', '\x2', '\x2', '\x126', 
		'\x127', '\a', 'r', '\x2', '\x2', '\x127', '\x128', '\a', 'g', '\x2', 
		'\x2', '\x128', '\x129', '\a', 'q', '\x2', '\x2', '\x129', '\x12A', '\a', 
		'h', '\x2', '\x2', '\x12A', 'Z', '\x3', '\x2', '\x2', '\x2', '\x12B', 
		'\x12C', '\a', 'u', '\x2', '\x2', '\x12C', '\x12D', '\a', 'k', '\x2', 
		'\x2', '\x12D', '\x12E', '\a', 'p', '\x2', '\x2', '\x12E', '\\', '\x3', 
		'\x2', '\x2', '\x2', '\x12F', '\x130', '\a', '\x65', '\x2', '\x2', '\x130', 
		'\x131', '\a', 'q', '\x2', '\x2', '\x131', '\x132', '\a', 'u', '\x2', 
		'\x2', '\x132', '^', '\x3', '\x2', '\x2', '\x2', '\x133', '\x134', '\a', 
		'u', '\x2', '\x2', '\x134', '\x135', '\a', 's', '\x2', '\x2', '\x135', 
		'\x136', '\a', 't', '\x2', '\x2', '\x136', '\x137', '\a', 'v', '\x2', 
		'\x2', '\x137', '`', '\x3', '\x2', '\x2', '\x2', '\x138', '\x139', '\a', 
		'k', '\x2', '\x2', '\x139', '\x13A', '\a', 'h', '\x2', '\x2', '\x13A', 
		'\x62', '\x3', '\x2', '\x2', '\x2', '\x13B', '\x13C', '\a', 'v', '\x2', 
		'\x2', '\x13C', '\x13D', '\a', 'j', '\x2', '\x2', '\x13D', '\x13E', '\a', 
		'g', '\x2', '\x2', '\x13E', '\x13F', '\a', 'p', '\x2', '\x2', '\x13F', 
		'\x64', '\x3', '\x2', '\x2', '\x2', '\x140', '\x141', '\a', 'g', '\x2', 
		'\x2', '\x141', '\x142', '\a', 'n', '\x2', '\x2', '\x142', '\x143', '\a', 
		'u', '\x2', '\x2', '\x143', '\x144', '\a', 'g', '\x2', '\x2', '\x144', 
		'\x66', '\x3', '\x2', '\x2', '\x2', '\x145', '\x146', '\a', 'v', '\x2', 
		'\x2', '\x146', '\x147', '\a', '\x63', '\x2', '\x2', '\x147', '\x148', 
		'\a', '\x64', '\x2', '\x2', '\x148', '\x149', '\a', 'n', '\x2', '\x2', 
		'\x149', '\x14A', '\a', 'g', '\x2', '\x2', '\x14A', 'h', '\x3', '\x2', 
		'\x2', '\x2', '\x14B', '\x14D', '\a', '&', '\x2', '\x2', '\x14C', '\x14B', 
		'\x3', '\x2', '\x2', '\x2', '\x14C', '\x14D', '\x3', '\x2', '\x2', '\x2', 
		'\x14D', '\x14E', '\x3', '\x2', '\x2', '\x2', '\x14E', '\x152', '\t', 
		'\x5', '\x2', '\x2', '\x14F', '\x151', '\t', '\x6', '\x2', '\x2', '\x150', 
		'\x14F', '\x3', '\x2', '\x2', '\x2', '\x151', '\x154', '\x3', '\x2', '\x2', 
		'\x2', '\x152', '\x150', '\x3', '\x2', '\x2', '\x2', '\x152', '\x153', 
		'\x3', '\x2', '\x2', '\x2', '\x153', 'j', '\x3', '\x2', '\x2', '\x2', 
		'\x154', '\x152', '\x3', '\x2', '\x2', '\x2', '\x155', '\x15B', '\a', 
		'$', '\x2', '\x2', '\x156', '\x15A', '\n', '\a', '\x2', '\x2', '\x157', 
		'\x158', '\a', '^', '\x2', '\x2', '\x158', '\x15A', '\v', '\x2', '\x2', 
		'\x2', '\x159', '\x156', '\x3', '\x2', '\x2', '\x2', '\x159', '\x157', 
		'\x3', '\x2', '\x2', '\x2', '\x15A', '\x15D', '\x3', '\x2', '\x2', '\x2', 
		'\x15B', '\x159', '\x3', '\x2', '\x2', '\x2', '\x15B', '\x15C', '\x3', 
		'\x2', '\x2', '\x2', '\x15C', '\x15E', '\x3', '\x2', '\x2', '\x2', '\x15D', 
		'\x15B', '\x3', '\x2', '\x2', '\x2', '\x15E', '\x15F', '\a', '$', '\x2', 
		'\x2', '\x15F', 'l', '\x3', '\x2', '\x2', '\x2', '\x160', '\x166', '\a', 
		')', '\x2', '\x2', '\x161', '\x165', '\n', '\b', '\x2', '\x2', '\x162', 
		'\x163', '\a', '^', '\x2', '\x2', '\x163', '\x165', '\v', '\x2', '\x2', 
		'\x2', '\x164', '\x161', '\x3', '\x2', '\x2', '\x2', '\x164', '\x162', 
		'\x3', '\x2', '\x2', '\x2', '\x165', '\x168', '\x3', '\x2', '\x2', '\x2', 
		'\x166', '\x164', '\x3', '\x2', '\x2', '\x2', '\x166', '\x167', '\x3', 
		'\x2', '\x2', '\x2', '\x167', '\x169', '\x3', '\x2', '\x2', '\x2', '\x168', 
		'\x166', '\x3', '\x2', '\x2', '\x2', '\x169', '\x16A', '\a', ')', '\x2', 
		'\x2', '\x16A', 'n', '\x3', '\x2', '\x2', '\x2', '\x15', '\x2', '\x81', 
		'\x8D', '\x91', '\x9A', '\x100', '\x103', '\x108', '\x10E', '\x111', '\x117', 
		'\x119', '\x11C', '\x14C', '\x152', '\x159', '\x15B', '\x164', '\x166', 
		'\x3', '\x2', '\x4', '\x2',
	};

	public static readonly ATN _ATN =
		new ATNDeserializer().Deserialize(_serializedATN);


}
} // namespace XBabyScript.Compile
