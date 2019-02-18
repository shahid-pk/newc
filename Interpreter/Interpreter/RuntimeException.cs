using System;
using NewC.Scanner;

namespace NewC
{
    public class RuntimeException: Exception
    {
        public Token Token { get; private set; }

        public RuntimeException(Token token, string message) : base(message)
        {
            this.Token = token;
        }
    }
}
