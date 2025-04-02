// See https://aka.ms/new-console-template for more information
using ProgrammingLang.Frontend.AST.Nodes;
using ProgrammingLang.Frontend.Parser;
using ProgrammingLang.Runtime;
using System.Reflection;
using System.Text.Json;
using Environment = ProgrammingLang.Runtime.Environment;
Console.WriteLine("Hello World!");

Assembly assembly = Assembly.GetExecutingAssembly();
string resourceName = "Testing.Main.sl";

using (Stream stream = assembly.GetManifestResourceStream(resourceName))
using (StreamReader reader = new StreamReader(stream))
{
	string result = reader.ReadToEnd();
	Parser parser = new Parser();
	Environment env = new Environment(ScopeType.Global);
	ProgramNode programNode = parser.ProduceAst(result);
	Console.WriteLine(JsonSerializer.Serialize(((NumberValue)Interpreter.Evaluate(programNode, env)).Value));
}
	
