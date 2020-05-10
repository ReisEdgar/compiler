using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;

namespace Interpreter
{
    public class Lexer
    {
        public string CurrentChar { get; set; }
        public Position CurrentPosition { get; set; } = new Position(0,-1);
        public string Text { get; set; }
        public bool OnSpecialChar { get; set; }
        public bool Finished { get; set; }
        public List<string> KeyWords = new List<string>() { "var", "and", "or", "not", "if", "for", "while", "func" };


        public void Next()
        {
            CurrentPosition.Index++;

            if (Text.Length <= CurrentPosition.Index)
            {
                CurrentChar = null;
                Finished = true;
                return;
            }
            CurrentChar = Text[CurrentPosition.Index].ToString();
            if (CurrentChar == "\n")
            {
                CurrentPosition.Line++;
            }
        }
        public void Previous()
        {
            if (CurrentChar == "\n")
            {
                CurrentPosition.Line--;
            }
            CurrentPosition.Index--;
            Finished = false;
            CurrentChar = Text[CurrentPosition.Index].ToString();

        }
        public bool IsCurrentCharADigit()
        {
            return Regex.IsMatch(CurrentChar, @"^[0-9]+$");
        }
        public bool IsCurrentCharAWhiteSpaceOrEndOfTheLine()
        {
            return CurrentChar == " " || CurrentChar == "\t" || CurrentChar == ";";
        }
        public bool IsCurrentCharALetter()
        {
            return Regex.IsMatch(CurrentChar, @"^[a-zA-Z]+$");
        }
        public List<Token> CreateTokens()
        {
            var tokens = new List<Token>();
            Next();

            while (!Finished)
            {
                if (IsCurrentCharALetter())
                {
                    tokens.Add(CreateIdentifier());
                }
                else if (CurrentChar == "\"")
                {
                    tokens.Add(CreateString());
                }
                else if (IsCurrentCharADigit())
                {
                    tokens.Add(CreateNumber());
                }
                else if (CurrentChar == "+")
                {
                    tokens.Add(new Token(TokenType.PLUS));
                }
                else if (CurrentChar == "-")
                {
                    tokens.Add(new Token(TokenType.MINUS));
                }
                else if (CurrentChar == "*")
                {
                    tokens.Add(new Token(TokenType.MULT));
                }
                else if (CurrentChar == "/")
                {
                    tokens.Add(new Token(TokenType.DIV));
                }
                else if (CurrentChar == "(")
                {
                    tokens.Add(new Token(TokenType.LPAREN));
                }
                else if (CurrentChar == ")")
                {
                    tokens.Add(new Token(TokenType.RPAREN));
                }
                else if (CurrentChar == "=")
                {
                    tokens.Add(CreateEquals());
                }
                else if (CurrentChar == "!=")
                {
                    tokens.Add(CreateNotEquals());
                }
                else if (CurrentChar == ">")
                {
                    tokens.Add(CreateGreaterThan());
                }
                else if (CurrentChar == "<")
                {
                    tokens.Add(CreateLessThan());
                }
                Next();
            }
         //   tokens.Add(new Token(TokenType.EOF));
            return tokens;
        }

        public Token CreateEquals()
        {
            Next();
            if(CurrentChar == "=")
            {
                return new Token(TokenType.EE);
            }
            else
            {
                Previous();
                return new Token(TokenType.EQ);
            }
        }
        public Token CreateNotEquals()
        {
            Next();
            if (CurrentChar == "=")
            {
                return new Token(TokenType.NEQ);
            }
            else
            {
                Previous();
                return new Token(TokenType.NOT);
            }
        }
        public Token CreateGreaterThan()
        {
            Next();
            if (CurrentChar == "=")
            {
                return new Token(TokenType.GTEQ);
            }
            else
            {
                Previous();
                return new Token(TokenType.GT);
            }
        }
        public Token CreateLessThan()
        {
            Next();
            if (CurrentChar == "=")
            {
                return new Token(TokenType.LTEQ);
            }
            else
            {
                Previous();
                return new Token(TokenType.LT);
            }
        }
        public Token CreateNumber()
        {
            var numString = "";
            var dotFound = false;

            
            do {
                if (CurrentChar == ".")
                {
                    if (dotFound || numString == "")
                    {
                        throw new Exception();
                    }
                    dotFound = true;
                }
                numString += CurrentChar;
                Next();
            } while (!Finished && CurrentChar != null && (IsCurrentCharADigit() || CurrentChar == ".")) ;

                Previous();
                if (dotFound)
            {
                float floatVal = float.Parse(numString, CultureInfo.InvariantCulture.NumberFormat);

                return new Token(TokenType.FLOAT, floatVal);
            }
            return new Token(TokenType.INT, int.Parse(numString));
        }
        public Token CreateString()
        {
            var str = "";
            Next();

            do
            {
                if(!(CurrentChar is string))
                {
                    throw new Exception("Syntax error");
                }
                if (CurrentChar == "\n")
                {
                    throw new Exception("Multiline strings are not supported");
                }
                if (CurrentChar == "\"")
                {
                    return new Token(TokenType.STRING, str);
                }
                str += CurrentChar;
                Next();
            } while (!Finished || CurrentChar != null);
            throw new Exception("Unexpected end of file");
        }
        public Token CreateIdentifier()
        {
            var identifier = "";

            while (!Finished && CurrentChar != null) 
            {

                if (IsCurrentCharAWhiteSpaceOrEndOfTheLine())
                {
                    if (KeyWords.Contains(identifier))
                    {
                        return new Token(TokenType.KEYW, identifier);
                    }
                    else
                    {
                        return new Token(TokenType.IDENTIF, identifier);
                    }
                }
                if (!IsCurrentCharALetter())
                {
                    throw new Exception("Identifiers can contain only letters");
                }
                identifier += CurrentChar;
                Next();
            }
            throw new Exception("Unexpected end of file");
        }
    
        public void CreateComment()
        {
            while(CurrentChar != "\n")
            {
                Next();
            }
        }
 
    }
}

