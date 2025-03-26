using ProgrammingLang.Frontend.AST;
using ProgrammingLang.Frontend.AST.Nodes;
using ProgrammingLang.Lexer;
using ProgrammingLang.Runtime;
namespace ProgrammingLang.Frontend.Parser;

public class Parser
{
	List<Token> _tokens = [];

	public ProgramNode ProduceAst(string source)
	{
		_tokens = Lexer.Lexer.Tokenize(source).ToList();
		ProgramNode programNode = new ProgramNode([]);
		
		while (!IsAtEnd())
		{
			programNode.Statements.Add(ParseStatement());
		}
		
		return programNode;
	}

	private IStatement ParseStatement()
	{
		switch (_tokens[0].Type)
		{
			case TokenTypes.Type:
				return ParseVariableDeclarationStatement();
			default:
				return ParseExpression();
		}
	}

	private IStatement ParseVariableDeclarationStatement()
	{
		String type = Advance().Value;
		string identifier = AdvanceExpect(TokenTypes.Identifier, "Expected identifier following 'let' keyword.").Value;
		if (_tokens[0].Type == TokenTypes.Semicolon)
		{
			Advance();
			return new VariableDeclarationNode(identifier, Types.TypesDict[type]);
		}

		AdvanceExpect(TokenTypes.Equals, "Expected '=' or ';' after variable declaration.");
		VariableDeclarationNode variableDeclarationNode = new VariableDeclarationNode(identifier, Types.TypesDict[type], ParseExpression());
		
		AdvanceExpect(TokenTypes.Semicolon, "Expected ';' after variable declaration.");
		return variableDeclarationNode;
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
			left = new BinaryExpressionNode(left, right, op);
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
			left = new BinaryExpressionNode(left, right, op);
		}
		
		return left;
	}

	private IExpression ParsePrimaryExpression()
	{
		Token tk = _tokens[0];

		switch (tk.Type)
		{
			case TokenTypes.Identifier:
				return new IdentifierNode(Advance().Value);
			case TokenTypes.Number:
				return new NumericalLiteralNode(float.Parse(Advance().Value));
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
