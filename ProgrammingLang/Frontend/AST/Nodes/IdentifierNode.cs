using ProgrammingLang.Runtime;
using Environment = ProgrammingLang.Runtime.Environment;
namespace ProgrammingLang.Frontend.AST.Nodes;

public class IdentifierNode(string symbol) : IExpression
{
	public string Symbol { get; set; } = symbol;
	
	public IRuntimeValue Evaluate(Environment env)
	{
		try
		{
			return env.LookupVariable(Symbol, env);
		}
		catch (Exception e)
		{
			try
			{
				return env.LookupType(Symbol, env);
			}
			catch (Exception e2)
			{
				throw new Exception($"Identifier: {Symbol} not found");
			}
		}
	}
}
