﻿using static System.Console;

namespace NewC.Helpers
{
    public class ReportError
    {
        public void Error(int line, string message)
        {
            Report(line, "", message);
        }

        private void Report(int line , string where, string message)
        {
            WriteLine($"[{line}] Error {where} : {message}");
        }

        static void error(Token token, String message)
        {
            if (token.type == TokenType.EOF)
            {
                report(token.line, " at end", message);
            }
            else
            {
                report(token.line, " at '" + token.lexeme + "'", message);
            }
        }
    }
}
