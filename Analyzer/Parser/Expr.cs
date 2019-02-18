//This file is auto generated, do not edit by hand.
//This file includes AST types for newc

using NewC.Scanner;

namespace NewC.Parser
{
	public interface IVisitor<T>
	{
		T VisitBinaryExpr(Binary expr);
		T VisitGroupingExpr(Grouping expr);
		T VisitLiteralExpr(Literal expr);
		T VisitUnaryExpr(Unary expr);
        T VisitExpressionStmt(Expression stmt);
        T VisitPrintStmt(Print stmt);
    }

	public abstract class Expr
	{
		public abstract T Accept<T>(IVisitor<T> visitor);
	}

	public class Binary : Expr
	{
		public Expr Left { get;private set; }
		public Token Op { get;private set; }
		public Expr Right { get;private set; }

		public Binary(Expr left,Token op,Expr right)
		{
			this.Left = left;
			this.Op = op;
			this.Right = right;
		}

		public override T Accept<T>(IVisitor<T> visitor)
		{
			return visitor.VisitBinaryExpr(this);
		}
	}

	public class Grouping : Expr
	{
		public Expr Expression { get;private set; }

		public Grouping(Expr expression)
		{
			this.Expression = expression;
		}

		public override T Accept<T>(IVisitor<T> visitor)
		{
			return visitor.VisitGroupingExpr(this);
		}
	}

	public class Literal : Expr
	{
		public object Value { get;private set; }

		public Literal(object value)
		{
			this.Value = value;
		}

		public override T Accept<T>(IVisitor<T> visitor)
		{
			return visitor.VisitLiteralExpr(this);
		}
	}

	public class Unary : Expr
	{
		public Token Op { get;private set; }
		public Expr Right { get;private set; }

		public Unary(Token op,Expr right)
		{
			this.Op = op;
			this.Right = right;
		}

		public override T Accept<T>(IVisitor<T> visitor)
		{
			return visitor.VisitUnaryExpr(this);
		}
	}

}
