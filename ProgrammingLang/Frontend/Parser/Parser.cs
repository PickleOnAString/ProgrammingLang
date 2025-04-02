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
	
	public List<IStatement> ProduceAst(List<Token> tokens)
	{
		_tokens = tokens;
		List<IStatement> statements = new List<IStatement>();
		
		while (!IsAtEnd())
		{
			statements.Add(ParseAccessModifier());
		}
		
		return statements;
	}

	private IStatement ParseAccessModifier()
	{
		switch (_tokens[0].Type)
		{
			case TokenTypes.Private:
				Advance();
				return ParseStatement(AccessModifier.Private);
			case TokenTypes.Public:
				Advance();
				return ParseStatement(AccessModifier.Public);
			case TokenTypes.Protected:
				Advance();
				return ParseStatement(AccessModifier.Protected);
			default:
				return ParseStatement();
		}
	}

	private IStatement ParseStatement(AccessModifier? accessModifier = null)
	{
		switch (_tokens[0].Type)
		{
			case TokenTypes.Identifier:
				return ParseVariableDeclarationStatement(accessModifier);
			case TokenTypes.Class:
				return ParseClassDeclarationStatement(accessModifier);
			default:
				return ParseExpression();
		}
	}

	private IStatement ParseClassDeclarationStatement(AccessModifier? accessModifier = null)
	{
		// Consume 'class' token
		Advance();
		
		string name = AdvanceExpect(TokenTypes.Identifier, "Expected class name after 'class' keyword.").Value;
    
		string? superClass = null;
		if (_tokens[0].Type == TokenTypes.Colon)
		{
			Advance();
			superClass = AdvanceExpect(TokenTypes.Identifier, "Expected superclass name after ':'").Value;
		}
		
		AdvanceExpect(TokenTypes.OpenCurlyBracket, "Expected open curly bracket after class name.");

		List<Token> classBody = new List<Token>();
		int bracketDepth = 1;

		while (bracketDepth > 0 && _tokens[0].Type != TokenTypes.EOF)
		{
			if (_tokens[0].Type == TokenTypes.OpenCurlyBracket)
			{
				bracketDepth++;
			}
			else if (_tokens[0].Type == TokenTypes.CloseCurlyBracket)
			{
				bracketDepth--;
				if (bracketDepth == 0) break;
			}

			classBody.Add(Advance());
		}

		// Ensure the final closing bracket is consumed
		AdvanceExpect(TokenTypes.CloseCurlyBracket, "Expected closing curly bracket after class body.");
    
		classBody.Add(new Token("EndOfFile", TokenTypes.EOF));

		// Recursively parse class body
		Parser parser = new Parser();
		List<IStatement> classStatements = parser.ProduceAst(classBody);

		return new ClassDeclarationNode(name, classStatements, accessModifier, superClass);
	}

	private IStatement ParseVariableDeclarationStatement(AccessModifier? accessModifier = null)
	{
		if (_tokens[1].Type == TokenTypes.Identifier)
		{
			String type = Advance().Value;
			string identifier = AdvanceExpect(TokenTypes.Identifier, "Expected identifier following 'let' keyword.").Value;
			if (_tokens[0].Type == TokenTypes.Semicolon)
			{
				Advance();
				return new VariableDeclarationNode(identifier, type, accessModifier);
			}

			AdvanceExpect(TokenTypes.Equals, "Expected '=' or ';' after variable declaration.");
			VariableDeclarationNode variableDeclarationNode = new VariableDeclarationNode(identifier, type, accessModifier, ParseExpression());

			AdvanceExpect(TokenTypes.Semicolon, "Expected ';' after variable declaration.");
			return variableDeclarationNode;
		}
		return ParseExpression();
	}

	private IExpression ParseExpression()
	{
		return ParseAssignmentExpression();
	}

	private IExpression ParseAssignmentExpression()
	{
		IExpression left = ParseAdditionExpression();

		if (_tokens[0].Type == TokenTypes.Equals)
		{
			Advance();
			IExpression right = ParseAssignmentExpression();
			AdvanceExpect(TokenTypes.Semicolon, "Expected ';' after variable assignment.");
			return new AssignmentExpression(left, right);
		}
		
		return left;
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
		IExpression left = ParseMemberExpression();

		while (_tokens[0].Value is "/" or "*" or "%")
		{
			string op = Advance().Value;
			IExpression right = ParseMemberExpression();
			left = new BinaryExpressionNode(left, right, op);
		}
		
		return left;
	}

	private IExpression ParseMemberExpression()
	{
		IExpression expr;

		// Handle `new Class()`
		if (_tokens[0].Type == TokenTypes.New)
		{
			expr = ParseNewExpression();
		}
		else
		{
			expr = ParsePrimaryExpression();
		}

		// Handle member access (`.`)
		while (_tokens[0].Type == TokenTypes.Dot)
		{
			Advance();
			string ident = AdvanceExpect(TokenTypes.Identifier, "Expected identifier after '.'").Value;
			expr = new MemberExpression(ident, expr);
		}

		return expr;
	}
	
	private IExpression ParseNewExpression()
	{
		Advance(); // Consume 'new'
    
		// Expect a class name (identifier)
		string className = AdvanceExpect(TokenTypes.Identifier, "Expected class name after 'new'.").Value;

		// Expect '(' for constructor arguments
		AdvanceExpect(TokenTypes.OpenBracket, "Expected '(' after class name.");

		List<IExpression> args = new List<IExpression>();

		// Parse arguments inside the parentheses
		if (_tokens[0].Type != TokenTypes.CloseBracket)
		{
			do
			{
				args.Add(ParseExpression());
			}
			while (_tokens[0].Type == TokenTypes.Comma && Advance().Type != TokenTypes.EOF);
		}

		// Expect ')' to close the constructor call
		AdvanceExpect(TokenTypes.CloseBracket, "Expected ')' after constructor arguments.");

		return new NewExpressionNode(className, args.ToArray());
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
