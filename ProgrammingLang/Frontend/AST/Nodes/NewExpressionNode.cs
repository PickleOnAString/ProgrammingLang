using ProgrammingLang.Runtime;
using Environment = ProgrammingLang.Runtime.Environment;
namespace ProgrammingLang.Frontend.AST.Nodes;

public class NewExpressionNode(IExpression className, IExpression[] args) : IExpression
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
		IRuntimeValue evaluatedClass = className.Evaluate(env);
		if (evaluatedClass is not IRuntimeType type)
			throw new Exception("NewExpression: can't instantiate a variable. expected a type, found " + evaluatedClass.GetType().Name);
		return type.Instantiate(runtimeArgs);
	}
}
