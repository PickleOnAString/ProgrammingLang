using System.Collections;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.RegularExpressions;
namespace ProgrammingLang.Lexer;

public class Lexer
{
	public static Token[] Tokenize(string source)
	{
		List<Token> tokens = new List<Token>();
		List<string> sourceSplit = Regex.Split(source, string.Empty).ToList();
		sourceSplit.RemoveAll(IsNull);
		Console.WriteLine(sourceSplit.ToArray().Length + " tokens");
		
		while (sourceSplit.Count > 0)
		{
			Console.WriteLine(JsonSerializer.Serialize(sourceSplit));
			switch (sourceSplit[0])
			{
				case "(":
					tokens.Add(new Token(sourceSplit[0], TokenTypes.OpenBracket));
					sourceSplit.RemoveAt(0);
					break;
				case ")":
					tokens.Add(new Token(sourceSplit[0], TokenTypes.CloseBracket));
					sourceSplit.RemoveAt(0);
					break;
				case ";":
					tokens.Add(new Token(sourceSplit[0], TokenTypes.Semicolon));
					sourceSplit.RemoveAt(0);
					break;
				case "+":
				case "-":
				case "*":
				case "/":
				case "%":	
					tokens.Add(new Token(sourceSplit[0], TokenTypes.BinaryOperator));
					sourceSplit.RemoveAt(0);
					break;
				case "=":
					tokens.Add(new Token(sourceSplit[0], TokenTypes.Equals));
					sourceSplit.RemoveAt(0);
					break;
				default:
				{
					//Multi char tokens
					if (IsNumeric(sourceSplit[0]))
					{
						String number = "";
						while (sourceSplit.Count > 0 && IsNumeric(sourceSplit[0]))
						{
							number += sourceSplit[0];
							sourceSplit.RemoveAt(0);
						}

						tokens.Add(new Token(number, TokenTypes.Number));
					}
					else if (IsAlphabetic(sourceSplit[0]))
					{
						String identifier = "";
						while (sourceSplit.Count > 0 && IsAlphabetic(sourceSplit[0]))
						{
							identifier += sourceSplit[0];
							sourceSplit.RemoveAt(0);
						}

						//check keywords
						bool isKeyword = Keywords.KeywordsDic.TryGetValue(identifier, out TokenTypes type);
						
						if (isKeyword || type == TokenTypes.Number)
							tokens.Add(new Token(identifier, type));
						else
							tokens.Add(new Token(identifier, TokenTypes.Identifier));
					} else if (IsSkippable(sourceSplit[0])) {
						sourceSplit.RemoveAt(0);
					}
					else
					{
						throw new Exception("Unknown token: "+"\""+sourceSplit[0]+"\"");
					}
					break;
				}
			}
		}
		
		tokens.Add(new Token("EndOfFile", TokenTypes.EOF));
		
		return tokens.ToArray();
	}
	
	private static bool IsAlphabetic(string src)
	{
		return !src.Equals(src.ToUpper());
	}
	
	private static bool IsNumeric(string src)
	{
		char charToCheck = src[0];
		return charToCheck >= '0' && charToCheck <= '9';
	}
	
	private static bool IsSkippable(string src)
	{
		return src.Equals(" ") || src.Equals("\n") || src.Equals("\t") || src.Equals("");
	}
	
	private static bool IsNull(string src)
	{
		return src.Equals("");
	}
}
