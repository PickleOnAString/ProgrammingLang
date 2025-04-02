using ProgrammingLang.Runtime;
using Environment = ProgrammingLang.Runtime.Environment;
namespace ProgrammingLang.Frontend.AST.Nodes;

public class ClassDeclarationNode(string name, List<IStatement> statements, AccessModifier? accessModifier, string? superClass = null) : IStatement
{
	public string Name { get; set; } = name;
	public string? SuperClass { get; set; } = superClass;
	public List<IStatement> Statements { get; set; } = statements;
	public AccessModifier? AcMod { get; set; } = accessModifier;
	
	public IRuntimeValue Evaluate(Environment env)
	{
		Environment classEnv = new Environment(ScopeType.Class, env);
		Environment staticEnvironment = new Environment(ScopeType.Static);
		foreach (IStatement statement in Statements)
			Interpreter.Evaluate(statement, classEnv);
		AcMod ??= AccessModifier.Private;
		env.DeclareType(Name, new RuntimeClasType(Name, SuperClass, classEnv, staticEnvironment), (AccessModifier)AcMod);
		return new NullValue();
	}
}
