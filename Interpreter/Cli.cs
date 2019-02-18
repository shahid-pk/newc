using NewC.Analyzer;
using System.IO;
using static System.Console;

namespace NewC
{
    public class Cli
    {
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
            var expression = parser.Parse();

            if (parser.HadError) return 65;

            var interpreter = new Interpreter(expression, reporter);
            var value = interpreter.Interpret();

            if (interpreter.HadRuntimeError) return 70;

            if(!isFile)
            {
                WriteLine($"{value}");
            }

            return 0;
        }
    }
}
