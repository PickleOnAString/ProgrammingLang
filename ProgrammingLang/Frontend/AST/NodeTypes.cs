using ProgrammingLang.Runtime;
using Environment = ProgrammingLang.Runtime.Environment;
namespace ProgrammingLang.Frontend.AST;

public interface IStatement
{
	public IRuntimeValue Evaluate(Environment env);
}
public interface IExpression : IStatement { }