using NewC.Scanner;
using System;

namespace NewC.Analyzer
{
    public interface IErrorReporter
    {
        void Error(int line, string message);
        void Error(Token token, String message);
    }
}
