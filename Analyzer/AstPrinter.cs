using System;
using System.Text;
using NewC.Parser;

namespace NewC.Analyzer
{
    public class AstPrinter : IVisitor<string>
    {
        public string Print(Expr expr)
        {
            return expr.Accept(this);
        }

        public string VisitAssignExpr(Assign expr)
        {
            throw new NotImplementedException();
        }

        public string VisitBinaryExpr(Binary expr)
        {
            return Parenthesize(expr.Op.Lexeme, expr.Left, expr.Right);
        }

        public string VisitBlockStmt(Block stmt)
        {
            throw new NotImplementedException();
        }

        public string VisitExpressionStmt(Expression stmt)
        {
            throw new NotImplementedException();
        }

        public string VisitGroupingExpr(Grouping expr)
        {
            return Parenthesize("group", expr.Expression);
        }

        public string VisitIfStmt(If stmt)
        {
            throw new NotImplementedException();
        }

        public string VisitLiteralExpr(Literal expr)
        {
            if (expr.Value == null) return "nil";
            return expr.Value.ToString();
        }

        public string VisitLogicalExpr(Logical expr)
        {
            throw new NotImplementedException();
        }

        public string VisitPrintStmt(Print stmt)
        {
            throw new NotImplementedException();
        }

        public string VisitUnaryExpr(Unary expr)
        {
            return Parenthesize(expr.Op.Lexeme, expr.Right);
        }

        public string VisitVariableExpr(Variable expr)
        {
            throw new NotImplementedException();
        }

        public string VisitVarStmt(Var stmt)
        {
            throw new NotImplementedException();
        }

        public string VisitWhileStmt(While stmt)
        {
            throw new NotImplementedException();
        }

        private string Parenthesize(string name, params Expr[] exprs)
        {
            StringBuilder builder = new StringBuilder();

            builder.Append("(").Append(name);
            foreach (var expr in exprs)
            {
                builder.Append(" ");
                builder.Append(expr.Accept(this));
            }
            builder.Append(")");

            return builder.ToString();
        }
    }
}
