using ProgrammingLang.Runtime;
using Environment = ProgrammingLang.Runtime.Environment;
namespace ProgrammingLang.Frontend.AST.Nodes;

public class NewExpressionNode(string className, IExpression[] args) : IExpression
{

	public IRuntimeValue Evaluate(Environment env)
	{
		IRuntimeValue[] runtimeArgs = new IRuntimeValue[args.Length];
		int i = 0;
		foreach (IExpression expression in args)
		{
			runtimeArgs[i] = Interpreter.Evaluate(expression, env);
			i++;
		}
		return env.LookupType(className, env).Instantiate(runtimeArgs);
	}
}
