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

	public class Function : Stmt
	{
		public Token Name { get;private set; }
		public System.Collections.Generic.List<Token> Parameters { get;private set; }
		public System.Collections.Generic.List<Stmt> Body { get;private set; }

		public Function(Token name,System.Collections.Generic.List<Token> parameters,System.Collections.Generic.List<Stmt> body)
		{
			this.Name = name;
			this.Parameters = parameters;
			this.Body = body;
		}

		public override T Accept<T>(IVisitor<T> visitor)
		{
			return visitor.VisitFunctionStmt(this);
		}
	}

	public class If : Stmt
	{
		public Expr Condition { get;private set; }
		public Stmt Thenbranch { get;private set; }
		public Stmt Elsebranch { get;private set; }

		public If(Expr condition,Stmt thenBranch,Stmt elseBranch)
		{
			this.Condition = condition;
			this.Thenbranch = thenBranch;
			this.Elsebranch = elseBranch;
		}

		public override T Accept<T>(IVisitor<T> visitor)
		{
			return visitor.VisitIfStmt(this);
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

	public class While : Stmt
	{
		public Expr Condition { get;private set; }
		public Stmt Body { get;private set; }

		public While(Expr condition,Stmt body)
		{
			this.Condition = condition;
			this.Body = body;
		}

		public override T Accept<T>(IVisitor<T> visitor)
		{
			return visitor.VisitWhileStmt(this);
		}
	}

}
