//Рогачевский Илья @IlyaRog
using Antlr4.Runtime;
using System;
using System.IO;

class Program
{
    static void Main(string[] args)
    {
        //Для того чтобы файл с кодом был обнаружен, он должен иметь имя example.pas,
        //а также находиться в папке с исполняемым файлом .exe(для windows)
        var input = File.ReadAllText("example.pas");

        Console.Title = "Интерпретатор";

        var inputStream = new AntlrInputStream(input);
        var lexer = new pascalLexer(inputStream);
        var tokens = new CommonTokenStream(lexer);
        var parser = new pascalParser(tokens);

        var tree = parser.program(); // начальное правило

        var visitor = new EvalVisitor();
        visitor.Visit(tree);
    }
}
