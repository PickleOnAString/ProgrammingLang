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
	
	OpenBracket,
	CloseBracket,
	Semicolon,
	
	BinaryOperator,
	
	EOF
}

public static class Keywords
{
	public static IDictionary<string, TokenTypes> KeywordsDic;
	static Keywords()
	{
		KeywordsDic = new Dictionary<string, TokenTypes>()
		{
			{"let", TokenTypes.Let}
		};
	}
}