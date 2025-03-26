using ProgrammingLang.Runtime;
using Environment = ProgrammingLang.Runtime.Environment;
namespace ProgrammingLang.Frontend.AST.Nodes;

public class ProgramNode(List<IStatement> statements) : IStatement
{
	public List<IStatement> Statements { get; set; } = statements;
	
	public IRuntimeValue Evaluate(Environment env)
	{
		IRuntimeValue lastEvaluate = new NullValue();
		foreach (IStatement statement in Statements)
			lastEvaluate = Interpreter.Evaluate(statement, env);
		return lastEvaluate;
	}
}
