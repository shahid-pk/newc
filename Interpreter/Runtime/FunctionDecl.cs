using NewC.Parser;
using System.Collections.Generic;

namespace NewC.Runtime
{
    public class FunctionDecl : ICallable
    {
        private readonly Function declaration;

        public FunctionDecl(Function declaration)
        {
            this.declaration = declaration;
        }

        public int Arity()
        {
            return declaration.Parameters.Count;
        }

        public object Call(Interpreter interpreter, List<object> arguments)
        {
            var environment = new Environment(interpreter.globals);
            for(var i = 0; i < declaration.Parameters.Count; i++)
            {
                environment.DefineVar(declaration.Parameters[i], arguments[i]);
            }
            interpreter.ExecuteBlock(declaration.Body, environment);
            return null;
        }

        public override string ToString()
        {
            return $"<fn {declaration.Name.Lexeme}>";
        }
    }
}
