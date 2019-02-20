using System;
using System.Collections.Generic;

namespace NewC.Runtime.GlobalFunctions
{
    public class Clock : ICallable
    {
        public int Arity()
        {
            return 0;
        }

        public object Call(Interpreter interpreter, List<object> arguments)
        {
            return (double)DateTime.Now.Millisecond / 1000.0;
        }

        public override string ToString()
        {
            return "<native fn>";
        }
    }
}
