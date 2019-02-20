//This file is auto generated, do not edit by hand.
//This file includes AST types for newc

using NewC.Scanner;

namespace NewC.Parser
{
	public interface IVisitor<T>
	{
		T VisitAssignExpr(Assign expr);
		T VisitBinaryExpr(Binary expr);
		T VisitCallExpr(Call expr);
		T VisitGroupingExpr(Grouping expr);
		T VisitLiteralExpr(Literal expr);
		T VisitLogicalExpr(Logical expr);
		T VisitUnaryExpr(Unary expr);
		T VisitVariableExpr(Variable expr);
        T VisitBlockStmt(Block stmt);
        T VisitExpressionStmt(Expression stmt);
        T VisitIfStmt(If stmt);
        T VisitPrintStmt(Print stmt);
        T VisitVarStmt(Var stmt);
        T VisitWhileStmt(While stmt);
    }

	public abstract class Expr
	{
		public abstract T Accept<T>(IVisitor<T> visitor);
	}

	public class Assign : Expr
	{
		public Token Name { get;private set; }
		public Expr Value { get;private set; }

		public Assign(Token name,Expr value)
		{
			this.Name = name;
			this.Value = value;
		}

		public override T Accept<T>(IVisitor<T> visitor)
		{
			return visitor.VisitAssignExpr(this);
		}
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

	public class Call : Expr
	{
		public Expr Callee { get;private set; }
		public Token Paren { get;private set; }
		public System.Collections.Generic.List<Expr> Arguments { get;private set; }

		public Call(Expr callee,Token paren, System.Collections.Generic.List<Expr> arguments)
		{
			this.Callee = callee;
			this.Paren = paren;
			this.Arguments = arguments;
		}

		public override T Accept<T>(IVisitor<T> visitor)
		{
			return visitor.VisitCallExpr(this);
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

	public class Logical : Expr
	{
		public Expr Left { get;private set; }
		public Token Op { get;private set; }
		public Expr Right { get;private set; }

		public Logical(Expr left,Token op,Expr right)
		{
			this.Left = left;
			this.Op = op;
			this.Right = right;
		}

		public override T Accept<T>(IVisitor<T> visitor)
		{
			return visitor.VisitLogicalExpr(this);
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

	public class Variable : Expr
	{
		public Token Name { get;private set; }

		public Variable(Token name)
		{
			this.Name = name;
		}

		public override T Accept<T>(IVisitor<T> visitor)
		{
			return visitor.VisitVariableExpr(this);
		}
	}

}
