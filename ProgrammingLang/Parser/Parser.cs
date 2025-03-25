using ProgrammingLang.AST;
using ProgrammingLang.Lexer;
namespace ProgrammingLang.Parser;

public class Parser
{
	List<Token> _tokens = [];

	public Program ProduceAst(string source)
	{
		_tokens = Lexer.Lexer.Tokenize(source).ToList();
		Program program = new Program([]);
		
		while (!IsAtEnd())
		{
			program.Statements.Add(ParseStatement());
		}
		
		return program;
	}

	private IStatement ParseStatement()
	{
		switch (_tokens[0].Type)
		{
			case TokenTypes.Let:
				return ParseVariableDeclarationStatement();
			default:
				return ParseExpression();
		}
	}

	private IStatement ParseVariableDeclarationStatement()
	{
		Advance();
		string identifier = AdvanceExpect(TokenTypes.Identifier, "Expected identifier following 'let' keyword.").Value;
		if (_tokens[0].Type == TokenTypes.Semicolon)
		{
			Advance();
			return new VariableDeclaration(identifier);
		}

		AdvanceExpect(TokenTypes.Equals, "Expected '=' or ';' after variable declaration.");
		VariableDeclaration variableDeclaration = new VariableDeclaration(identifier, ParseExpression());
		
		AdvanceExpect(TokenTypes.Semicolon, "Expected ';' after variable declaration.");
		return variableDeclaration;
	}

	private IExpression ParseExpression()
	{
		return ParseAdditionExpression();
	}

	private IExpression ParseAdditionExpression()
	{
		IExpression left = ParseMultiplicationExpression();

		while (_tokens[0].Value is "+" or "-")
		{
			string op = Advance().Value;
			IExpression right = ParseMultiplicationExpression();
			left = new BinaryExpression(left, right, op);
		}
		
		return left;
	}
	
	private IExpression ParseMultiplicationExpression()
	{
		IExpression left = ParsePrimaryExpression();

		while (_tokens[0].Value is "/" or "*" or "%")
		{
			string op = Advance().Value;
			IExpression right = ParsePrimaryExpression();
			left = new BinaryExpression(left, right, op);
		}
		
		return left;
	}

	private IExpression ParsePrimaryExpression()
	{
		Token tk = _tokens[0];

		switch (tk.Type)
		{
			case TokenTypes.Identifier:
				return new Identifier(Advance().Value);
			case TokenTypes.Number:
				return new NumericalLiteral(float.Parse(Advance().Value));
			case TokenTypes.OpenBracket:
				Advance();
				IExpression expr = ParseExpression();
				AdvanceExpect(TokenTypes.CloseBracket, "Unexpected token found: Expected closing bracket.");
				return expr;
			default:
				throw new Exception($"Unexpected token {tk.Type}");
		}
	}

	private Token Advance()
	{
		Token tk = _tokens[0];
		_tokens.RemoveAt(0);
		return tk;
	}
	
	private Token AdvanceExpect(TokenTypes type, string message)
	{
		Token tk = _tokens[0];
		_tokens.RemoveAt(0);
		if (tk.Type != type)
		{
			throw new Exception(message+$" Expected {type}, found {tk.Type}");
		}
		return tk;
	}
	
	private bool IsAtEnd()
	{
		return _tokens[0].Type == TokenTypes.EOF;
	}
}
