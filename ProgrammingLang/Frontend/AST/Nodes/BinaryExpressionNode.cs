using ProgrammingLang.Runtime;
using Environment = ProgrammingLang.Runtime.Environment;
using ValueType = ProgrammingLang.Runtime.ValueType;
namespace ProgrammingLang.Frontend.AST.Nodes;

public class BinaryExpressionNode(IExpression left, IExpression right, string op) : IExpression
{
	public IExpression Left { get; set; } = left;
	public IExpression Right { get; set; } = right;
	public string Operator { get; set; } = op;

	public IRuntimeValue Evaluate(Environment env)
	{
		IRuntimeValue left = Interpreter.Evaluate(Left, env);
		IRuntimeValue right = Interpreter.Evaluate(Right, env);
		if (left.Type.Name == "Float" && right.Type.Name == "Float")
		{
			return EvaluateNumericBinaryExpression((NumberValue)left, (NumberValue)right, Operator);
		}
		
		throw new Exception("Not a valid binary expression between value types: "+left.Type+" and "+right.Type);
	}
	
	public static NumberValue EvaluateNumericBinaryExpression(NumberValue left, NumberValue right, String operatorType)
	{
		float result = 0;
		switch (operatorType)
		{
		case "+":
			result = left.Value + right.Value;
			break;
		case "-":
			result = left.Value - right.Value;
			break;
		case "*":
			result = left.Value * right.Value;
			break;
		case "/":
			// TODO: Handle div by 0
			result = left.Value / right.Value;
			break;
		}
		return new NumberValue(result);
	}
}
