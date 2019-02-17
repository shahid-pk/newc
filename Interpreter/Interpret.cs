using NewC.Analyzer;
using System.IO;
using static System.Console;

namespace NewC
{
    public class Interpret 
    {
        public void RunFile(string path)
        {
            var source = File.ReadAllText(path);
            Run(source);
        }

        public void RunPrompt()
        {
            while(true) {
                WriteLine(">");
                Run(ReadLine());
            }
        }

        private void Run(string source)
        {
            var reporter = new ErrorReporter();

            var scanner = new Scanner.Scanner(source, reporter);
            var tokens = scanner.ScanTokens();
            var parser = new Parser.Parser(tokens, reporter);
            var expression = parser.Parse();

            if (parser.HadError) return;

            WriteLine(new AstPrinter().Print(expression));
            // print tokens
            //if(!scanner.HasErrors) {
            //    foreach(var token in tokens) {
            //        WriteLine($"{token}");
            //    }
            //}
        }
    }
}