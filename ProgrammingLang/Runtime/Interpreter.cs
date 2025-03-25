using ProgrammingLang.AST;
namespace ProgrammingLang.Runtime;

public class Interpreter
{
	public static IRuntimeValue EvaluateProgram(Program astNode, Environment env)
	{
		IRuntimeValue lastEvaluate = new NullValue();
		foreach (IStatement statement in astNode.Statements)
			lastEvaluate = Evaluate(statement, env);
		return lastEvaluate;
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
	
	public static IRuntimeValue EvaluateBinaryExpression(BinaryExpression astNode, Environment env)
	{
		IRuntimeValue left = Evaluate(astNode.Left, env);
		IRuntimeValue right = Evaluate(astNode.Right, env);
		if (left.Type == ValueType.Number && right.Type == ValueType.Number)
		{
			return EvaluateNumericBinaryExpression((NumberValue)left, (NumberValue)right, astNode.Operator);
		}
		
		throw new Exception("Not a valid binary expression between value types: "+left.Type+" and "+right.Type);
	}

	public static IRuntimeValue EvaluateIdentifier(Identifier astNode, Environment env)
	{
		return env.LookupVariable(astNode.Symbol);
	}

	public static IRuntimeValue EvaluateVariableDeclaration(VariableDeclaration astNode, Environment env)
	{
		IRuntimeValue? value = astNode.Value != null ? Evaluate(astNode.Value, env) : new NullValue();
		return env.DeclareVariable(astNode.Identifier, value);
	}
	
	public static IRuntimeValue Evaluate(IStatement astNode, Environment env)
	{
		switch (astNode.Type)
		{
			case NodeTypes.NumericalLiteral:
				return new NumberValue(((NumericalLiteral)astNode).Value);
			case NodeTypes.BinaryExpression:
				return EvaluateBinaryExpression((BinaryExpression)astNode, env);
			case NodeTypes.Identifier:
				return EvaluateIdentifier((Identifier)astNode, env);
			case NodeTypes.Program:
				return EvaluateProgram((Program)astNode, env);
			case NodeTypes.VariableDeclaration:
				return EvaluateVariableDeclaration((VariableDeclaration)astNode, env);
			default:
				throw new Exception("Unknown node type in evaluation: "+astNode.Type);
		}
	}
}
