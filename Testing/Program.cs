// See https://aka.ms/new-console-template for more information
using ProgrammingLang.Frontend.AST.Nodes;
using ProgrammingLang.Frontend.Parser;
using ProgrammingLang.Runtime;
using System.Text.Json;
using Environment = ProgrammingLang.Runtime.Environment;
Console.WriteLine("Hello World!");

Parser parser = new Parser();
ProgramNode programNode = parser.ProduceAst("Float bleh = 15; bleh * 2");

Environment env = new Environment(null);
env.DeclareVariable("x", new NumberValue(10));
env.DeclareVariable("true", new BoolValue(true));
env.DeclareVariable("false", new BoolValue(false));
env.DeclareVariable("null", new NullValue());

Console.WriteLine(JsonSerializer.Serialize(((NumberValue)Interpreter.Evaluate(programNode, env)).Value));
	
