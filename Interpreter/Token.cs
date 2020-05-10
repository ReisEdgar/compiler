using System;
using System.Collections.Generic;
using System.Text;

namespace Interpreter
{
    public enum TokenType
    {
        INT = 1,
        FLOAT = 2,
        STRING = 3,
        CHAR = 4,
        PLUS = 5,
        MINUS = 6,
        MULT = 7,
        DIV = 8,
        LPAREN = 9,
        RPAREN = 10,
        IDENTIF = 11,
        KEYW = 12,
        EQ = 13,
        NEQ = 14,
        NOT = 15,
        GT = 16,
        LT = 17,
        GTEQ = 18,
        LTEQ = 19,
        EE = 20,
    }
    public static class TokenTypeGroups
    {
        public static List<TokenType> Numeric { get; set; } = new List<TokenType>()
        {
            TokenType.LT,
            TokenType.GT,
            TokenType.LTEQ,
            TokenType.GTEQ,
            TokenType.MULT,
            TokenType.DIV,
            TokenType.PLUS,
            TokenType.MINUS,
        };
    }
    public class Token
    {
        public TokenType TokenType { get; set; }
        public object Value { get; set; }
        public Token() { }
        public Token(TokenType type, object value = null)
        {
            TokenType = type;
            if (value != null)
                Value = value;
        }

        public bool Matches(TokenType type, object value)
        {
            return type == TokenType && value == Value;
        }
    }
}
