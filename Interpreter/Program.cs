using static System.Console;

namespace NewC
{
    class Program
    {
        static int Main(string[] args)
        {
            var interpret = new Interpret();

            if(args.Length > 0) {
                WriteLine("Usage: newc [script]");
                return 64;
            } else if (args.Length == 1) {
                interpret.RunFile(args[0]);
            } else {
                interpret.RunPrompt();
            }
            return 0;
        }
    }
}
