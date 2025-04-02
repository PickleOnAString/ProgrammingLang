namespace ProgrammingLang.Lexer;

public class Token(string value, TokenTypes type)
{
	public string Value { get; } = value;
	public TokenTypes Type { get; } = type;

}

public enum TokenTypes
{
	Null,
	Number,
	Identifier,
	Equals,
	
	Let,
	Class,
	New,
	
	OpenBracket,
	CloseBracket,
	Semicolon,
	Comma,
	Colon,
	Dot,
	OpenCurlyBracket,
	CloseCurlyBracket,
	
	BinaryOperator,
	
	Private,
	Public,
	Protected,
	
	Static,
	
	EOF
}

public static class Keywords
{
	public static IDictionary<string, TokenTypes> KeywordsDic;
	static Keywords()
	{
		KeywordsDic = new Dictionary<string, TokenTypes>()
		{
			{"let", TokenTypes.Let},
			{"class", TokenTypes.Class},
			{"new", TokenTypes.New},
			{"private", TokenTypes.Private},
			{"public", TokenTypes.Public},
			{"protected", TokenTypes.Protected},
			{"static", TokenTypes.Static}
		};
	}
}