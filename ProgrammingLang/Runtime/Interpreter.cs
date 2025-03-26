using ProgrammingLang.Frontend.AST;
namespace ProgrammingLang.Runtime;

public static class Interpreter
{
	public static IRuntimeValue Evaluate(IStatement astNode, Environment env)
	{
		return astNode.Evaluate(env);
	}
}
