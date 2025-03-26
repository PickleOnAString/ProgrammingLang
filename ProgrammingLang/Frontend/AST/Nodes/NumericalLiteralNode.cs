using ProgrammingLang.Runtime;
using Environment = ProgrammingLang.Runtime.Environment;
namespace ProgrammingLang.Frontend.AST.Nodes;

public class NumericalLiteralNode(float value) : IExpression
{
	public float Value { get; set; } = value;
	
	public IRuntimeValue Evaluate(Environment env)
	{
		return new NumberValue(Value);
	}
}
