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
		Environment classEnv = new Environment(ScopeType.Class, env.ScopeType == ScopeType.Class ? env.StaticEnv : env);
		Environment staticEnvironment = new Environment(ScopeType.Static, env.ScopeType == ScopeType.Class ? env.StaticEnv : env);
		classEnv.StaticEnv = staticEnvironment;
		foreach (IStatement statement in Statements)
			Interpreter.Evaluate(statement, classEnv);
		AcMod ??= AccessModifier.Private;
		if (env.ScopeType == ScopeType.Global)
			env.DeclareType(Name, new RuntimeClasType(Name, SuperClass, classEnv, staticEnvironment), (AccessModifier)AcMod);
		else if (env.ScopeType == ScopeType.Class)
			env.StaticEnv.DeclareType(Name, new RuntimeClasType(Name, SuperClass, classEnv, staticEnvironment), (AccessModifier)AcMod);
		else
			throw new Exception("Class declaration can only be used in global or class scope");
		return new NullValue();
	}
}
