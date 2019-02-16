//This file is auto generated, do not edit by hand.
//This file includes AST types for newc

using NewC.Lexer;

namespace NewC.Scanner
{
	public abstract class Expr
	{}

	public class Binary : Expr
	{
		private Expr left;
		private Token op;
		private Expr right;

		public Binary(Expr left,Token op,Expr right)
		{
			this.left = left;
			this.op = op;
			this.right = right;
		}
	}

	public class Grouping : Expr
	{
		private Expr expression;

		public Grouping(Expr expression)
		{
			this.expression = expression;
		}
	}

	public class Literal : Expr
	{
		private object value;

		public Literal(object value)
		{
			this.value = value;
		}
	}

	public class Unary : Expr
	{
		private Token op;
		private Expr right;

		public Unary(Token op,Expr right)
		{
			this.op = op;
			this.right = right;
		}
	}

}
