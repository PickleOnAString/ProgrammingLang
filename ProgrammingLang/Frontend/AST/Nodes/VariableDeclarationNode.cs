using ProgrammingLang.Runtime;
using Environment = ProgrammingLang.Runtime.Environment;
namespace ProgrammingLang.Frontend.AST.Nodes;

public class VariableDeclarationNode(String identifier, string type, AccessModifier? accessModifier, IExpression? value = null) : IStatement
{
	public string Identifier = identifier;
	public IExpression? Value = value;
	public string Type = type;
	public AccessModifier? AcMod { get; set; } = accessModifier;
	
	public IRuntimeValue Evaluate(Environment env)
	{
		IRuntimeValue value = Value != null ? Interpreter.Evaluate(Value, env) : new NullValue();
		IRuntimeType type = env.LookupType(this.Type, env);
		if (!type.CanCast(value))
			throw new Exception($"Type mismatch: {value.GetType()} is not {Type}");
		AcMod ??= AccessModifier.Private;
		return env.DeclareVariable(Identifier, value, (AccessModifier)AcMod);
	}
}
