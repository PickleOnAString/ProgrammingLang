namespace ProgrammingLang.Runtime;

public class Values
{
	
}

public interface IRuntimeValue
{
	public ValueType Type { get;}
}

public class NullValue : IRuntimeValue
{
	public ValueType Type => ValueType.Null;
	public string Value { get; set; } = "null";
}

public class NumberValue(float value) : IRuntimeValue
{
	public ValueType Type { get; set; } = ValueType.Number;
	public float Value { get; set; } = value;

}

public class BoolValue(bool value) : IRuntimeValue
{
	public ValueType Type { get; set; } = ValueType.Bool;
	public bool Value { get; set; } = value;
}

public enum ValueType
{
	Null,
	Number,
	Bool
}
