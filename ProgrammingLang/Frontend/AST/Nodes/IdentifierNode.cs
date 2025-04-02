using ProgrammingLang.Runtime;
using Environment = ProgrammingLang.Runtime.Environment;
namespace ProgrammingLang.Frontend.AST.Nodes;

public class IdentifierNode(string symbol) : IExpression
{
	public string Symbol { get; set; } = symbol;
	
	public IRuntimeValue Evaluate(Environment env)
	{
		return env.LookupVariable(Symbol, env);
	}
}
