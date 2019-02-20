using System.Collections.Generic;

namespace NewC.Runtime
{
    public interface ICallable
    {
        int Arity();
        object Call(Interpreter interpreter, List<object> arguments);
    }
}
