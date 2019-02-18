using NewC.Scanner;
using System.Collections.Generic;

namespace NewC
{
    public class Environment
    {
        private readonly Dictionary<string, object> values;

        public Environment()
        {
            this.values = new Dictionary<string, object>();
        }

        public void DefineVar(Token name, object value)
        {
            if(values.ContainsKey(name.Lexeme))
            {
                throw new RuntimeException(name, $"Variable {name.Lexeme} already defined.");
            }
            values[name.Lexeme] = value;
        }

        public object GetVar(Token name)
        {
            if(values.ContainsKey(name.Lexeme))
            {
                return values[name.Lexeme];
            }

            throw new RuntimeException(name, $"Undefined variable {name.Lexeme}.");
        }

        public void AssignVar(Token name, object value)
        {
            if (values.ContainsKey(name.Lexeme))
            {
                values[name.Lexeme] = value;
            }

            throw new RuntimeException(name, $"Undefined variable {name.Lexeme}.");
        }
    }
}
