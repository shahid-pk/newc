using static System.Console;

namespace NewC
{
    class Program
    {
        static int Main(string[] args)
        {
            var cli = new Cli();

            if(args.Length > 0) {
                WriteLine("Usage: newc [script]");
                return 64;
            } else if (args.Length == 1) {
                cli.RunFile(args[0]);
            } else {
                cli.RunPrompt();
            }
            return 0;
        }
    }
}
