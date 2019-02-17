using System;
using static System.Console;
using NewC.Scanner;

namespace NewC.Analyzer
{
    public class ErrorReporter : IErrorReporter
    {
        public void Error(int line, string message)
        {
            Report(line, "", message);
        }

        private void Report(int line , string where, string message)
        {
            WriteLine($"[{line}] Error {where} : {message}");
        }

        public void Error(Token token, String message)
        {
            if (token.Type == TokenType.EOF)
            {
                Report(token.Line, " at end", message);
            }
            else
            {
                Report(token.Line, " at '" + token.Lexeme + "'", message);
            }
        }
    }
}
