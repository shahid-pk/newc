
// grammar rules for newc

//program        → declaration* EOF;
//declaration    → varDecl
//               | statement ;
//varDecl        → "var" IDENTIFIER ( "=" expression )? ";" ;
//statement      → exprStmt
//               | printStmt ;
//exprStmt       → expression ";" ;
//printStmt      → "print" expression ";" ;
//expression     → assignment ;
//assignment     → IDENTIFIER "=" assignment
//               | equality ;
//equality       → comparison(( "!=" | "==" ) comparison )* ;
//comparison     → addition(( ">" | ">=" | "<" | "<=" ) addition )* ;
//addition       → multiplication(( "-" | "+" ) multiplication )* ;
//multiplication → unary(( "/" | "*" ) unary )* ;
//unary          → ( "!" | "-" ) unary
//               | primary ;
//primary        → NUMBER | STRING | "false" | "true" | "nil"
//               | "(" expression ")" 
//               | IDENTIFIER;

using System;
using System.Collections.Generic;
using NewC.Analyzer;
using NewC.Scanner;

namespace NewC.Parser
{
    internal class ParseErrorException : Exception
    {}
      

    public class Parser
    {
        private readonly List<Token> tokens;
        private readonly IErrorReporter reporter;
        private int current = 0;
        public bool HadError { get; private set; }

        public Parser(List<Token> tokens, IErrorReporter reporter)
        {
            this.tokens = tokens;
            this.reporter = reporter;
            HadError = false;
        }

        public List<Stmt> Parse()
        {
            var statements = new List<Stmt>();
            while (!IsAtEnd())
            {
                statements.Add(Declaration());
            }
            return statements;
        }

        private Stmt Declaration()
        {
            try
            {
                if (Match(TokenType.VAR)) return VarDeclaration();
                return Statement();
            }
            catch (ParseErrorException)
            {
                HadError = true;
                Synchoronize();
                return null;
            }
        }

        private Stmt VarDeclaration()
        {
            var name = Consume(TokenType.IDENTIFIER, "Expect variable name.");

            Expr initializer = null;
            if(Match(TokenType.EQUAL))
            {
                initializer = Expression();
            }

            Consume(TokenType.SEMICOLON, "Expect ';' after variable declaration.");

            return new Var(name, initializer);
        }

        private Stmt Statement()
        {
            if (Match(TokenType.PRINT)) return PrintStatement();
            return ExpresionStatement();
        }

        private Stmt PrintStatement()
        {
            var value = Expression();
            Consume(TokenType.SEMICOLON, "Expect ';' after value.");
            return new Print(value);
        }

        private Stmt ExpresionStatement()
        {
            var expr = Expression();
            Consume(TokenType.SEMICOLON, "Expect ';' after Expression.");
            return new Expression(expr);
        }

        private Expr Expression()
        {
            return Assignment();
        }

        private Expr Assignment()
        {
            var expr = Equality();

            if(Match(TokenType.EQUAL))
            {
                Token equals = Previous();
                Expr value = Assignment();
                
                if(expr is Variable)
                {
                    Token name = ((Variable)expr).Name;
                    return new Assign(name, value);
                }

                Error(equals, "Invalid assignment target");
                HadError = true;
            }

            return expr;
        }

        private Expr Equality()
        {
            var expr = Comparison();

            while(Match(TokenType.BANG_EQUAL, TokenType.EQUAL_EQUAL))
            {
                Token op = Previous();
                Expr right = Comparison();
                expr = new Binary(expr, op, right);
            }

            return expr;
        }

        private Expr Comparison()
        {
            var expr = Addition();

            while(Match(TokenType.GREATER, TokenType.GREATER_EQUAL, TokenType.LESS, TokenType.LESS_EQUAL))
            {
                Token op = Previous();
                Expr right = Addition();
                expr = new Binary(expr, op, right);
            }

            return expr;
        }

        private Expr Addition()
        {
            var expr = Multiplication();

            while(Match(TokenType.PLUS, TokenType.MINUS))
            {
                Token op = Previous();
                Expr right = Multiplication();
                expr = new Binary(expr, op, right);
            }

            return expr;
        }

        private Expr Multiplication()
        {
            var expr = Unary();

            while(Match(TokenType.SLASH, TokenType.STAR))
            {
                Token op = Previous();
                Expr right = Unary();
                expr = new Binary(expr, op, right);
            }

            return expr;
        }

        private Expr Unary()
        {
            if(Match(TokenType.BANG, TokenType.MINUS))
            {
                Token op = Previous();
                Expr right = Unary();
                return new Unary(op, right);
            }

            return Primary();
        }

        private Expr Primary()
        {
            if (Match(TokenType.FALSE)) return new Literal(false);
            if (Match(TokenType.TRUE)) return new Literal(true);
            if (Match(TokenType.NIL)) return new Literal(null);

            if (Match(TokenType.NUMBER, TokenType.STRING)) return new Literal(Previous().Literal);

            if(Match(TokenType.LEFT_PAREN))
            {
                var expr = Expression();
                Consume(TokenType.RIGHT_PAREN, "Expect ')' after expression.");
                return new Grouping(expr);
            }

            if(Match(TokenType.IDENTIFIER))
            {
                return new Variable(Previous());
            }

            throw Error(Peek(), "Expected Expression");
        }

        private Token Consume(TokenType type, string message)
        {
            if (Check(type)) return Advance();
            throw Error(Peek(), message);
        }

        private ParseErrorException Error(Token token, string message)
        {
            reporter.Error(token, message);
            return new ParseErrorException();
        }

        private void Synchoronize()
        {
            Advance();

            while(!IsAtEnd())
            {
                if (Previous().Type == TokenType.SEMICOLON) return;

                switch(Peek().Type)
                {
                    case TokenType.CLASS:
                    case TokenType.FUN:
                    case TokenType.IF:
                    case TokenType.VAR:
                    case TokenType.WHILE:
                    case TokenType.FOR:
                    case TokenType.RETURN:
                    case TokenType.PRINT:
                        return;
                }
                Addition();
            }
        }

        private Token Advance()
        {
            if (!IsAtEnd()) current++;
            return Previous();
        }

        private bool Match(params TokenType[] types)
        {
            foreach(var type in types)
            {
                if(Check(type))
                {
                    Advance();
                    return true;
                }
            }

            return false;
        }

        private bool Check(TokenType type)
        {
            if (IsAtEnd()) return false;
            return Peek().Type == type;
        }

        private bool IsAtEnd()
        {
            return Peek().Type == TokenType.EOF;
        }

        private Token Peek()
        {
            return tokens[current];
        }

        private Token Previous()
        {
            return tokens[current - 1];
        }
    }
}
