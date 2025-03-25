namespace ProgrammingLang.AST;

public enum NodeTypes
{
	//STATEMENTS
	Program,
	VariableDeclaration,
	
	//EXPRESSIONS
	NumericalLiteral,
	Identifier,
	BinaryExpression,
}

public interface IStatement
{
	NodeTypes Type { get; }
}

public class Program(List<IStatement> statements) : IStatement
{
	public NodeTypes Type => NodeTypes.Program;
	public List<IStatement> Statements { get; set; } = statements;
}

public class VariableDeclaration(String identifier, IExpression? value = null) : IStatement
{
	public NodeTypes Type => NodeTypes.VariableDeclaration;
	public string Identifier = identifier;
	public IExpression? Value = value;
}

public interface IExpression : IStatement {}

public class BinaryExpression(IExpression left, IExpression right, string op) : IExpression
{
	public NodeTypes Type => NodeTypes.BinaryExpression;
	public IExpression Left { get; set; } = left;
	public IExpression Right { get; set; } = right;
	public string Operator { get; set; } = op;
}

public class Identifier(string symbol) : IExpression
{
	public NodeTypes Type => NodeTypes.Identifier;
	public string Symbol { get; set; } = symbol;
}

public class NumericalLiteral(float value) : IExpression
{
	public NodeTypes Type => NodeTypes.NumericalLiteral;
	public float Value { get; set; } = value;
}