namespace ProgrammingLang.Runtime;

public class Environment(Environment? parent)
{
	Environment? _parent = parent;
	public Dictionary<string, IRuntimeValue> Variables = new Dictionary<string, IRuntimeValue>();
	
	public IRuntimeValue DeclareVariable(string name, IRuntimeValue value)
	{
		if (Variables.ContainsKey(name))
			throw new Exception($"Cannot declare variable {name}, as it is already declared in scope");
		Variables.Add(name, value);
		return value;
	}

	public IRuntimeValue AssignVariable(string name, IRuntimeValue value)
	{
		Environment env = Resolve(name);
		env.Variables.Add(name, value);
		return value;
	}

	public Environment Resolve(string name)
	{
		if (Variables.ContainsKey(name))
			return this;
		if (_parent != null)
			return _parent.Resolve(name);
		throw new Exception($"Variable {name} not declared in scope");
	}

	public IRuntimeValue LookupVariable(string name)
	{
		Environment env = Resolve(name);
		return env.Variables[name];
	}
}
