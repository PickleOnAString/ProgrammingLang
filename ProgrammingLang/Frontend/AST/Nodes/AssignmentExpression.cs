using ProgrammingLang.Runtime;
using System.Linq.Expressions;
using Environment = ProgrammingLang.Runtime.Environment;
namespace ProgrammingLang.Frontend.AST.Nodes;

public class AssignmentExpression(IExpression left, IExpression right) : IExpression
{
	public IExpression Left { get; set; } = left;
	public IExpression Right { get; set; } = right;
	
	public IRuntimeValue Evaluate(Environment env)
	{
		if (Left.GetType() != typeof(IdentifierNode))
		{
			throw new Exception("Left hand side of assignment must be an identifier");
		}
		
		string symbol = ((Left as IdentifierNode)!).Symbol;
		env.AssignVariable(symbol, Interpreter.Evaluate(Right, env));
		return new NullValue();
	}
}
