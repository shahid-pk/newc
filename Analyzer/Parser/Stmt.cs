//This file is auto generated, do not edit by hand.
//This file includes AST types for newc

using NewC.Scanner;

namespace NewC.Parser
{
	public abstract class Stmt
	{
		public abstract T Accept<T>(IVisitor<T> visitor);
	}

	public class Block : Stmt
	{
		public System.Collections.Generic.List<Stmt> Statements { get;private set; }

		public Block(System.Collections.Generic.List<Stmt> statements)
		{
			this.Statements = statements;
		}

		public override T Accept<T>(IVisitor<T> visitor)
		{
			return visitor.VisitBlockStmt(this);
		}
	}

	public class Expression : Stmt
	{
		public Expr Expr { get;private set; }

		public Expression(Expr Expr)
		{
			this.Expr = Expr;
		}

		public override T Accept<T>(IVisitor<T> visitor)
		{
			return visitor.VisitExpressionStmt(this);
		}
	}

	public class Print : Stmt
	{
		public Expr Expr { get;private set; }

		public Print(Expr Expr)
		{
			this.Expr = Expr;
		}

		public override T Accept<T>(IVisitor<T> visitor)
		{
			return visitor.VisitPrintStmt(this);
		}
	}

	public class Var : Stmt
	{
		public Token Name { get;private set; }
		public Expr Initializer { get;private set; }

		public Var(Token name,Expr initializer)
		{
			this.Name = name;
			this.Initializer = initializer;
		}

		public override T Accept<T>(IVisitor<T> visitor)
		{
			return visitor.VisitVarStmt(this);
		}
	}

}
