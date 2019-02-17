
namespace NewC.Scanner
{
    public class Token
    {
        private readonly TokenType type;
        private readonly string lexeme;
        private readonly object literal;
        private readonly int line;

        public Token(TokenType type, string lexeme, object literal, int line)
        {
            this.type = type;
            this.lexeme = lexeme;
            this.literal = literal;
            this.line = line;
        }

        public string Lexeme => lexeme;
        public int Line => line;
        public object Literal => literal;
        public TokenType Type => type;

        public override string ToString()
        {
            return $"{this.type} {this.lexeme} {this.literal}";
        }
    }
}