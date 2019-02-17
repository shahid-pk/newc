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
	}

	public abstract class Expr
	{
		public abstract T Accept<T>(IVisitor<T> visitor);
	}

	public class Binary : Expr
	{
		private Expr left;
		private Token op;
		private Expr right;

		public Expr Left => left;
		public Token Op => op;
		public Expr Right => right;

		public Binary(Expr left,Token op,Expr right)
		{
			this.left = left;
			this.op = op;
			this.right = right;
		}

		public override T Accept<T>(IVisitor<T> visitor)
		{
			return visitor.VisitBinaryExpr(this);
		}
	}

	public class Grouping : Expr
	{
		private Expr expression;

		public Expr Expression => expression;

		public Grouping(Expr expression)
		{
			this.expression = expression;
		}

		public override T Accept<T>(IVisitor<T> visitor)
		{
			return visitor.VisitGroupingExpr(this);
		}
	}

	public class Literal : Expr
	{
		private object value;

		public object Value => value;

		public Literal(object value)
		{
			this.value = value;
		}

		public override T Accept<T>(IVisitor<T> visitor)
		{
			return visitor.VisitLiteralExpr(this);
		}
	}

	public class Unary : Expr
	{
		private Token op;
		private Expr right;

		public Token Op => op;
		public Expr Right => right;

		public Unary(Token op,Expr right)
		{
			this.op = op;
			this.right = right;
		}

		public override T Accept<T>(IVisitor<T> visitor)
		{
			return visitor.VisitUnaryExpr(this);
		}
	}

}
