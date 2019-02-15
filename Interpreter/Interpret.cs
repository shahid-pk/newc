using System;
using System.IO;
using System.Collections.Generic;
using static System.Console;
using NewC.Parser;

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
            var scanner = new Scanner(source);
            var tokens = scanner.ScanTokens();

            // scanner has already reported the error
            // we don't need to do anything
            if(!scanner.HasErrors) {
                foreach(var token in tokens) {
                    WriteLine($"{token}");
                }
            }
        }
    }
}