using NewC.Analyzer;
using NewC.Parser;
using NewC.Scanner;

namespace NewC
{
    public class Interpreter : IVisitor<object>
    {
        private readonly Expr expression;
        private readonly IErrorReporter reporter;
        public bool HadRuntimeError { get; private set; }

        public Interpreter(Expr expression, IErrorReporter reporter)
        {
            this.expression = expression;
            this.reporter = reporter;
            this.HadRuntimeError = false;
        }

        public string Interpret()
        {
            try
            {
                var value = Evaluate(expression);
                return Stringify(value);
            }
            catch(RuntimeException ex)
            {
                reporter.Error(ex.Token, ex.Message);
                this.HadRuntimeError = true;
                // to make the compiler happy
                return "\n";
            }
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