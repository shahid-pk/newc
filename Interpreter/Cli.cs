using NewC.Analyzer;
using System.IO;
using static System.Console;

namespace NewC
{
    public class Cli
    {
        private Interpreter interpreter;

        public int RunFile(string path)
        {
            var source = File.ReadAllText(path);
            return Run(source, true);
        }

        public int RunPrompt()
        {
            while (true)
            {
                WriteLine(">");
                Run(ReadLine());
            }
        }

        private int Run(string source, bool isFile = false)
        {
            var reporter = new ErrorReporter();

            var scanner = new Scanner.Scanner(source, reporter);
            var tokens = scanner.ScanTokens();

            var parser = new Parser.Parser(tokens, reporter);
            var statements = parser.Parse();

            if (parser.HadError) return 65;

            if(interpreter == null)
            {
                interpreter = new Interpreter(reporter);
            }

            interpreter.Interpret(statements);

            if (interpreter.HadRuntimeError) return 70;

            return 0;
        }
    }
}
