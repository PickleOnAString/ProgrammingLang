namespace ProgrammingLang.Runtime;

public class Values
{
	
}

public interface IRuntimeValue
{
	public IRuntimeType Type { get; set; }
}

public class NullValue : IRuntimeValue
{
	public string Value { get; set; } = "null";
	public IRuntimeType Type { get; set; } = new RuntimeNullType();
}

public class NumberValue(float value) : IRuntimeValue
{
	public float Value { get; set; } = value;
	public IRuntimeType Type { get; set; } = new RuntimeFloatType();
}

public class BoolValue(bool value) : IRuntimeValue
{
	public bool Value { get; set; } = value;
	public IRuntimeType Type { get; set; } = new RuntimeBoolType();
}

public class ClassValue(string name, string? superClass, Environment env, RuntimeClasType type) : IRuntimeValue
{
	public IRuntimeType Type { get; set; } = type;
	public Environment Env { get; set; } = env;
}

public enum ValueType
{
	Null,
	Number,
	Bool
}
