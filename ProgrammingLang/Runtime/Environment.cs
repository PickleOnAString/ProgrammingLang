namespace ProgrammingLang.Runtime;

public enum AccessModifier
{
    Public,
    Private,
    Protected
}

public class Environment
{
    Environment? _parent;
    public Environment? StaticEnv;
    public Dictionary<string, (IRuntimeValue Value, AccessModifier Modifier)> Variables =
        new Dictionary<string, (IRuntimeValue, AccessModifier)>();
    public Dictionary<string, (IRuntimeType Type, AccessModifier Modifier)> Types =
        new Dictionary<string, (IRuntimeType, AccessModifier)>();
    public ScopeType ScopeType { get; set; }
    
    public Environment(ScopeType scopeType, Environment? parent = null)
    {
        _parent = parent;
        if (scopeType == ScopeType.Global)
            SetupScope(this);
        ScopeType = scopeType;
    }

    public static void SetupScope(Environment env)
    {
        Console.WriteLine("Setting up global scope");
        env.DeclareVariable("true", new BoolValue(true), AccessModifier.Public);
        env.DeclareVariable("false", new BoolValue(false), AccessModifier.Public);
        env.DeclareVariable("null", new NullValue(), AccessModifier.Public);
        
        env.DeclareType("Float", new RuntimeFloatType(), AccessModifier.Public);
    }
    
    public IRuntimeValue DeclareVariable(string name, IRuntimeValue value, AccessModifier modifier)
    {
        if (Variables.ContainsKey(name))
            throw new Exception($"Cannot declare variable {name}, as it is already declared in scope");
        Variables.Add(name, (value, modifier));
        return value;
    }
    
    public IRuntimeType DeclareType(string name, IRuntimeType type, AccessModifier modifier)
    {
        if (Types.ContainsKey(name))
            throw new Exception($"Cannot declare type {name}, as it is already declared in scope");
        Types.Add(name, (type, modifier));
        return type;
    }

    public IRuntimeValue AssignVariable(string name, IRuntimeValue value)
    {
        Environment env = Resolve(name);
        if (env.Variables[name].Modifier == AccessModifier.Private && env != this)
            throw new Exception($"Cannot assign to private variable '{name}' outside its defining scope.");
        env.Variables[name] = (value, env.Variables[name].Modifier);
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

    public Environment ResolveType(string name)
    {
        if (Types.ContainsKey(name))
            return this;
        if (_parent != null)
            return _parent.ResolveType(name);
        throw new Exception($"Type {name} not declared in scope");
    }

    public IRuntimeValue LookupVariable(string name, Environment srcEnv)
    {
        Environment env = Resolve(name);
        var (value, modifier) = env.Variables[name];

        if (modifier == AccessModifier.Private && srcEnv != this)
            throw new Exception($"Cannot access private variable '{name}' outside its defining scope.");
        if (modifier == AccessModifier.Protected && (_parent == null || _parent.Resolve(name) != srcEnv))
            throw new Exception($"Cannot access protected variable '{name}' outside its class hierarchy.");
        
        return value;
    }
    
    public IRuntimeType LookupType(string name, Environment srcEnv)
    {
        Environment env = ResolveType(name);
        var (type, modifier) = env.Types[name];

        if (modifier == AccessModifier.Private && srcEnv != this)
            throw new Exception($"Cannot access private type '{name}' outside its defining scope.");
        if (modifier == AccessModifier.Protected && (_parent == null || _parent.ResolveType(name) != srcEnv))
            throw new Exception($"Cannot access protected type '{name}' outside its class hierarchy.");

        return type;
    }
}


public enum ScopeType
{
	Global,
	Class,
	Function,
	Static
}
