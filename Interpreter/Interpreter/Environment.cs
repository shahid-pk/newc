using NewC.Scanner;
using System;
using System.Collections.Generic;

namespace NewC
{
    public class Environment
    {
        private readonly Dictionary<string, object> values;
        private Environment enclosing;

        public Environment()
        {
            this.values = new Dictionary<string, object>();
            this.enclosing = null;
        }

        public Environment(Environment enclosing)
        {
            this.values = new Dictionary<string, object>();
            this.enclosing = enclosing;
        }

        public void DefineVar(Token name, object value)
        {
            if(values.ContainsKey(name.Lexeme))
            {
                throw new RuntimeException(name, $"Variable {name.Lexeme} already defined.");
            }
            values[name.Lexeme] = value;
        }

        internal void DefineVar(string name, object value)
        {
            // this will be used by the interpreter
            // so throwing c# exception is allowed
            // to catch language bug
            if (values.ContainsKey(name))
            {
                throw new Exception($"Global {name} already defined.");
            }
            values[name] = value;
        }

        public object GetVar(Token name)
        {
            if(values.ContainsKey(name.Lexeme))
            {
                return values[name.Lexeme];
            }

            if(enclosing != null)
            {
                return enclosing.GetVar(name);
            }

            throw new RuntimeException(name, $"Undefined variable {name.Lexeme}.");
        }

        public object AssignVar(Token name, object value)
        {
            if (values.ContainsKey(name.Lexeme))
            {
                return values[name.Lexeme] = value;
            }

            if(enclosing != null)
            {
                return enclosing.AssignVar(name, value);
            }

            throw new RuntimeException(name, $"Undefined variable {name.Lexeme}.");
        }
    }
}
