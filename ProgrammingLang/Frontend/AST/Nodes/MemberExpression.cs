using ProgrammingLang.Runtime;
using Environment = ProgrammingLang.Runtime.Environment;
namespace ProgrammingLang.Frontend.AST.Nodes;

public class MemberExpression(IExpression member, IExpression obj) : IExpression
{
	public IExpression Member { get; set; } = member;
	public IExpression Object { get; set; } = obj;
	
	public IRuntimeValue Evaluate(Environment env)
	{
		IRuntimeValue leftSideValue = Object.Evaluate(env);

		if (leftSideValue is ClassValue)
			if (Member is IdentifierNode)
				return Lookup(((IdentifierNode)Member).Symbol, ((ClassValue)leftSideValue).Env, env);
			else
				return Member.Evaluate(((ClassValue)leftSideValue).Env);
		if (leftSideValue is RuntimeClasType)
			if (Member is IdentifierNode)
				return Lookup(((IdentifierNode)Member).Symbol, ((RuntimeClasType)leftSideValue).StaticEnv, env);
			else
				return Member.Evaluate(((RuntimeClasType)leftSideValue).StaticEnv);
		
		throw new Exception("MemberExpression: Object is not a class or instance of class");
	}

	private IRuntimeValue Lookup(string symbol, Environment env, Environment callerEnv)
	{
		try
		{
			return env.LookupVariable(symbol, callerEnv);
		}
		catch (Exception e)
		{
			try
			{
				return env.LookupType(symbol, callerEnv);
			}
			catch (Exception e2)
			{
				throw new Exception($"Identifier: {symbol} not found");
			}
		}
	}
}
