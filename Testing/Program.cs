// See https://aka.ms/new-console-template for more information
using ProgrammingLang.AST;
using ProgrammingLang.Lexer;
using ProgrammingLang.Parser;
using ProgrammingLang.Runtime;
using System.Text.Json;
using Environment = ProgrammingLang.Runtime.Environment;
Console.WriteLine("Hello World!");

Parser parser = new Parser();
ProgrammingLang.AST.Program program = parser.ProduceAst("let bleh = 15;");

Environment env = new Environment(null);
env.DeclareVariable("x", new NumberValue(10));
env.DeclareVariable("true", new BoolValue(true));
env.DeclareVariable("false", new BoolValue(false));
env.DeclareVariable("null", new NullValue());

Console.WriteLine(JsonSerializer.Serialize(((NumberValue)Interpreter.Evaluate(program, env)).Value));
	
