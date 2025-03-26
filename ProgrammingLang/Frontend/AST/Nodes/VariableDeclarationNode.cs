using ProgrammingLang.Runtime;
using Environment = ProgrammingLang.Runtime.Environment;
namespace ProgrammingLang.Frontend.AST.Nodes;

public class VariableDeclarationNode(String identifier, Type type, IExpression? value = null) : IStatement
{
	public string Identifier = identifier;
	public IExpression? Value = value;
	public Type Type = type;
	
	public IRuntimeValue Evaluate(Environment env)
	{
		IRuntimeValue value = Value != null ? Interpreter.Evaluate(Value, env) : new NullValue();
		if (!value.GetType().IsSubclassOf(Type) && value.GetType() != Type)
			throw new Exception($"Type mismatch: {value.GetType()} is not {Type}");
		return env.DeclareVariable(Identifier, value);
	}
}
