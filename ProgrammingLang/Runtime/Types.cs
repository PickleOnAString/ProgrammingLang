namespace ProgrammingLang.Runtime;

public static class Types
{
	public static Dictionary<string, Type> TypesDict = new();
	
	static Types()
	{
		TypesDict.Add("Float", typeof(NumberValue));
	}
}
