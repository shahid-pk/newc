
using System.Collections.Generic;
using NewC.Analyzer;

namespace NewC.Scanner
{
    public class Scanner
    {
        private readonly string source;
        private int start = 0;
        private int current = 0;
        private int line = 1;
        private readonly List<Token> tokens;
        private readonly IErrorReporter reporter;

        public bool HasErrors { get; set; }
        
        public Scanner(string source, IErrorReporter reporter) 
        {
            this.source = source;
            this.HasErrors = false;
            this.tokens = new List<Token>();
            this.reporter = reporter;
        }

        public List<Token> ScanTokens()
        {
            while(!IsAtEnd()) {
                start = current;
                ScanToken();
            }
            tokens.Add(new Token(TokenType.EOF, "", null, line));
            return tokens;
        }

        private void ScanToken()
        {
            char c = Advance();                          
            switch (c) {                                 
                case '(': AddToken(TokenType.LEFT_PAREN); break;     
                case ')': AddToken(TokenType.RIGHT_PAREN); break;    
                case '{': AddToken(TokenType.LEFT_BRACE); break;     
                case '}': AddToken(TokenType.RIGHT_BRACE); break;    
                case ',': AddToken(TokenType.COMMA); break;          
                case '.': AddToken(TokenType.DOT); break;            
                case '-': AddToken(TokenType.MINUS); break;          
                case '+': AddToken(TokenType.PLUS); break;           
                case ';': AddToken(TokenType.SEMICOLON); break;      
                case '*': AddToken(TokenType.STAR); break;
                case '!': AddToken(Match('=') ? TokenType.BANG_EQUAL : TokenType.BANG); break;      
                case '=': AddToken(Match('=') ? TokenType.EQUAL_EQUAL : TokenType.EQUAL); break;    
                case '<': AddToken(Match('=') ? TokenType.LESS_EQUAL : TokenType.LESS); break;      
                case '>': AddToken(Match('=') ? TokenType.GREATER_EQUAL : TokenType.GREATER); break;
                case '/':
                    if(Match('/')) {
                        // its a comment keep igonring 
                        // until the end of line
                        while (Peek() != '\n' && !IsAtEnd()) Advance();
                    } else {
                        AddToken(TokenType.SLASH);
                    }
                break;
                case ' ':
                case '\r':
                case '\t':
                break;
                case '\n': line++; break;
                case '"': String(); break;
                default :
                    if(IsDigit(c)) {
                        Number();
                    } else if(IsAlpha(c)) {
                        Identifier();
                    } else {
                        reporter.Error(line, "Unexpected character.");
                        HasErrors = true;
                    }
                break;
            }            
        }

        private void String() {
            while(Peek() != '"' && !IsAtEnd()) {
                if(Peek() == '\n') line++;
                Advance();
            }

            // unterminated string
            if(IsAtEnd()) {
                reporter.Error(line, "Unterminated String");
                HasErrors = true;
            }

            // close " (string closing)
            Advance();

            // trim quotes and add string lexeme
            AddToken(TokenType.STRING, source.Substring(start + 1, current - start - 2));
        }

        private void Number() {
            while(IsDigit(Peek())) Advance();

            // look for fractional part
            if(Peek() == '.' && IsDigit(PeekNext())) {
                // consume .
                Advance();
                while(IsDigit(Peek())) Advance();
            }

            AddToken(TokenType.NUMBER, double.Parse(source.Substring(start, current - start)));
        }

        private void Identifier()
        {
            while(IsAlphaNumeric(Peek())) Advance();

            // see if the identifer is reserved word
            var text = source.Substring(start, current - start);
            var type = KeyWord(text);
            if(type == 0) type = TokenType.IDENTIFIER;

            AddToken(type);
        }

        
        private TokenType KeyWord(string identifier) 
        {
            switch(identifier) {
                case "and":     return TokenType.AND;            
                case "class":   return TokenType.CLASS; 
                case "else":    return TokenType.ELSE;
                case "false":   return TokenType.FALSE;
                case "for":     return TokenType.FOR;
                case "fun":     return TokenType.FUN;                      
                case "if":      return TokenType.IF; 
                case "nil":     return TokenType.NIL;   
                case "or":      return TokenType.OR; 
                case "print":   return TokenType.PRINT;  
                case "return":  return TokenType.RETURN;
                case "super":   return TokenType.SUPER;
                case "this":    return TokenType.THIS;
                case "true":    return TokenType.TRUE;
                case "var":     return TokenType.VAR;
                case "while":   return TokenType.WHILE;
                case "match":   return TokenType.MATCH;
                default:
                    return 0;
            }    
        }

        // look forward one character
        private char Peek() {
            if (IsAtEnd()) return '\0';
            return source[current];
        }

        // look forward two character
        private char PeekNext() {
            if (current + 1 > source.Length) return '\0';
            return source[current + 1];
        }

        // consume forward one character
        // for a match
        private bool Match(char expected) {
            if (IsAtEnd()) return false;
            if(source[current] != expected) return false;
            current++;
            return true;
        }

        // consume forward one character
        // in any case
        private char Advance()
        {
            current++;
            return source[current - 1];
        }

         private void AddToken(TokenType type, object literal = null) {
            var text = source.Substring(start, current - start);
            tokens.Add(new Token(type, text, literal, line));
        }

        private bool IsAlpha(char c) {
            return  (c >= 'a' && c <= 'z') || 
                    (c >= 'A' && c <= 'Z') ||
                    (c == '_');

        }

        private bool IsAlphaNumeric(char c) {
            return IsAlpha(c) || IsDigit(c);
        }

        private bool IsDigit(char c) {
            return c >= '0' && c <= '9';
        }

        private bool IsAtEnd()
        {
            return current >= source.Length;
        }
    }
}