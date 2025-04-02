namespace ProgrammingLang.Runtime;

public static class Types
{
	public static Dictionary<string, Type> TypesDict = new();
	
	static Types()
	{
		TypesDict.Add("Float", typeof(NumberValue));
	}
}

public interface IRuntimeType
{
	public string Name { get; }
	public bool CanCast(IRuntimeValue value);
	public IRuntimeValue Instantiate(IRuntimeValue[] args);
	public Environment? StaticEnv { get; set; }
}

public class RuntimeFloatType : IRuntimeType
{
	public string Name => "Float";
	public Environment? StaticEnv { get; set; } = null;
	public bool CanCast(IRuntimeValue value)
	{
		return value.GetType() == typeof(NumberValue);
	}
	
	public IRuntimeValue Instantiate(IRuntimeValue[] args)
	{
		throw new Exception("Can't create instance of primitive type(Float), use float literal instead");
	}
}

public class RuntimeNullType : IRuntimeType
{
	public string Name => "Null";
	public Environment? StaticEnv { get; set; } = null;
	public bool CanCast(IRuntimeValue value)
	{
		return value.GetType() == typeof(NullValue);
	}
	
	public IRuntimeValue Instantiate(IRuntimeValue[] args)
	{
		throw new Exception("Can't create instance of primitive Type(Null), use null literal instead");
	}
}

public class RuntimeBoolType : IRuntimeType
{
	public string Name => "Bool";
	public Environment? StaticEnv { get; set; } = null;
	public bool CanCast(IRuntimeValue value)
	{
		return value.GetType() == typeof(BoolValue);
	}
	
	public IRuntimeValue Instantiate(IRuntimeValue[] args)
	{
		throw new Exception("Can't create instance of primitive type(Bool), use bool literal instead");
	}
}

public class RuntimeClasType(string name, string? superClass, Environment env, Environment staticEnv) : IRuntimeType
{
	public string Name => name;
	public Environment? StaticEnv { get; set; } = null;

	public bool CanCast(IRuntimeValue value)
	{
		return true;
	}
	
	public IRuntimeValue Instantiate(IRuntimeValue[] args)
	{
		return new ClassValue(name, superClass, env, this);
	}
}
