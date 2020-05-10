using Interpreter.Value;
using System;

namespace Interpreter
{

    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Go");
            var context = new Context();

            while (true)
            {
                var text = Console.ReadLine();

                Run(context, text);
            }
        }

        public static void Run(Context context, string text)
        {
            var lexer = new Lexer();
            lexer.Text = text;
            var tokens = lexer.CreateTokens();

            var parser = new Parser();
            parser.Tokens = tokens;
            var ast = parser.Parse();
            var interpreter = new Interpreter();
            var result = interpreter.TraverseTree(ast, context) as BooleanValue;
            
            Console.WriteLine(result.Value);
        }

    }
}
