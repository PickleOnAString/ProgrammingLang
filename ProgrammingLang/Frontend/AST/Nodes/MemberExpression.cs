using ProgrammingLang.Runtime;
using Environment = ProgrammingLang.Runtime.Environment;
namespace ProgrammingLang.Frontend.AST.Nodes;

public class MemberExpression(string member, IExpression obj) : IExpression
{
	public string Member { get; set; } = member;
	public IExpression Object { get; set; } = obj;
	
	public IRuntimeValue Evaluate(Environment env)
	{
		IRuntimeValue leftSideValue = Object.Evaluate(env);
		if (leftSideValue.Type.GetType() != typeof(RuntimeClasType))
			throw new Exception("MemberExpression: Object is not a class");
		return ((ClassValue)leftSideValue).Env.LookupVariable(member, env);
	}
}
