using NewC.Analyzer;
using NewC.Parser;
using NewC.Scanner;
using System;
using System.Collections.Generic;

namespace NewC
{
    public class Interpreter : IVisitor<object>
    {
        private readonly IErrorReporter reporter;
        private Environment environment;
        public bool HadRuntimeError { get; private set; }

        public Interpreter(IErrorReporter reporter)
        {
            this.reporter = reporter;
            this.environment = new Environment();
            this.HadRuntimeError = false;
        }

        public void Interpret(List<Stmt> statements)
        {
            try
            {
                foreach(var statement in statements)
                {
                    Execute(statement);
                }
            }
            catch(RuntimeException ex)
            {
                reporter.Error(ex.Token, ex.Message);
                this.HadRuntimeError = true;
            }
        }

        public object VisitVarStmt(Var stmt)
        {
            object value = null;
            if(stmt.Initializer != null)
            {
                value = Evaluate(stmt.Initializer);
            }
            environment.DefineVar(stmt.Name, value);
            return null;
        }

        public object VisitIfStmt(If stmt)
        {
            var condition = Evaluate(stmt.Condition);
            if(!(condition is bool))
            {
                condition = IsTruthy(condition);
            }

            if((bool)condition)
            {
                Execute(stmt.Thenbranch);
            }
            else if (stmt.Elsebranch != null)
            {
                Execute(stmt.Elsebranch);
            }
            return null;
        }

        public object VisitVariableExpr(Variable expr)
        {
            return environment.GetVar(expr.Name);
        }

        public object VisitAssignExpr(Assign expr)
        {
            object value = Evaluate(expr.Value);
            environment.AssignVar(expr.Name, value);
            return value;
        }

        public object VisitExpressionStmt(Expression stmt)
        {
            Evaluate(stmt.Expr);
            return null;
        }

        public object VisitPrintStmt(Print stmt)
        {
            var value = Evaluate(stmt.Expr);
            Console.WriteLine(Stringify(value));
            return null;
        }

        public object VisitBlockStmt(Block stmt)
        {
            ExecuteBlock(stmt.Statements, new Environment(environment));
            return null;
        }

        public object VisitBinaryExpr(Binary expr)
        {
            var left = Evaluate(expr.Left);
            var right = Evaluate(expr.Right);

            switch(expr.Op.Type)
            {
                case TokenType.GREATER:
                    CheckNumberOperand(expr.Op, left, right);
                    return (double)left > (double)right;
                case TokenType.GREATER_EQUAL:
                    CheckNumberOperand(expr.Op, left, right);
                    return (double)left >= (double)right;
                case TokenType.LESS:
                    CheckNumberOperand(expr.Op, left, right);
                    return (double)left < (double)right;
                case TokenType.LESS_EQUAL:
                    CheckNumberOperand(expr.Op, left, right);
                    return (double)left <= (double)right;
                case TokenType.BANG_EQUAL:
                    return !IsEqual(left, right);
                case TokenType.EQUAL_EQUAL:
                    return IsEqual(left, right);
                case TokenType.MINUS:
                    CheckNumberOperand(expr.Op, left, right);
                    return (double)left - (double)right;
                case TokenType.PLUS:
                    if(left is double && right is double)
                    {
                        return (double)left + (double)right;
                    }

                    if(left is string && right is string)
                    {
                        return (string)left + (string)right;
                    }
                    throw new RuntimeException(expr.Op, "Operands must be two numbers or two strings.");
                case TokenType.SLASH:
                    CheckNumberOperand(expr.Op, left, right);
                    return (double)left / (double)right;
                case TokenType.STAR:
                    CheckNumberOperand(expr.Op, left, right);
                    return (double)left * (double)right;
            }

            // unreachable code
            return null;
        }

        public object VisitGroupingExpr(Grouping expr)
        {
            return Evaluate(expr.Expression);
        }

        public object VisitLiteralExpr(Literal expr)
        {
            return expr.Value;
        }

        public object VisitUnaryExpr(Unary expr)
        {
            var right = Evaluate(expr.Right);

            switch(expr.Op.Type)
            {
                case TokenType.BANG:
                    return !IsTruthy(right);
                case TokenType.MINUS:
                    CheckNumberOperand(expr.Op, right);
                    return -(double)right;
            }

            // unreachable code
            return null;
        }

        private void ExecuteBlock(List<Stmt> statements, Environment environment)
        {
            var previous = this.environment;
            try
            {
                this.environment = environment;
                foreach(var statement in statements)
                {
                    Execute(statement);
                }
            }
            finally
            {
                this.environment = previous;
            }
        }

        private string Stringify(object obj)
        {
            if (obj == null) return "nil";

            if(obj is double)
            {
                var text = obj.ToString();
                if(text.EndsWith(".0"))
                {
                    text = text.Substring(text.Length - 2, 2);
                }
                return text;
            }

            return obj.ToString();
        }

        private void CheckNumberOperand(Token op, object operand)
        {
            if (operand is double) return;
            throw new RuntimeException(op, "Operand must be a number.");
        }

        private void CheckNumberOperand(Token op, object left, object right)
        {
            if (left is double && right is double) return;
            throw new RuntimeException(op, "Operands must be numbers.");
        }

        private object Evaluate(Expr expr)
        {
            return expr.Accept(this);
        }

        private object Execute(Stmt statement)
        {
            return statement.Accept(this);
        }

        private bool IsTruthy(object val)
        {
            if (val == null) return false;
            if (val is bool) return false;

            return true;
        }

        private bool IsEqual(object left, object right)
        {
            if (left == null && right == null) return true;
            if (left == null) return false;

            return left.Equals(right);
        }
    }
}