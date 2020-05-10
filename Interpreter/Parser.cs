using Interpreter.ASTNodes;
using Interpreter.Value;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Interpreter
{

    public class Parser
    {
        public List<Token> Tokens { get; set; }
        public int Index { get; set; } = -1;
        public Token CurrentToken { get; set; }

        public void Next()
        {
            Index++;
            if (Tokens.Count > Index)
            {
                CurrentToken = Tokens[Index];
            }
            else if (Tokens.Count < Index)
            {
                throw new Exception();
            }
        }

        public void Previous()
        {
            Index--;
            CurrentToken = Tokens[Index];
        }
        public INode Parse()
        {
            Next();
            return Expression();
        }

        public INode Atom()
        {
            var token = CurrentToken;
            if (CurrentToken.TokenType == TokenType.INT || CurrentToken.TokenType == TokenType.FLOAT)
            {
                //   Next();
                return new NumberNode() { NumberToken = token };
            }
            else if (CurrentToken.TokenType == TokenType.IDENTIF)
            {
                return new VarAccessNode() { VarName = token.Value.ToString() };
            }
            else if (CurrentToken.TokenType == TokenType.LPAREN)
            {
                Next();
                var expression = Expression();
                Next();
                if (CurrentToken.TokenType == TokenType.RPAREN)
                {
                    return expression;
                }
                else
                {
                    throw new Exception("Expected ')'");
                }
            }
            throw new Exception("Invalid syntax");
        }

        public INode Factor()
        {
            var token = CurrentToken;
            if (CurrentToken.TokenType == TokenType.PLUS || CurrentToken.TokenType == TokenType.MINUS)
            {
                Next();
                var factor = Factor();

                return new UnaryOpNode() { Operation = token, Node = factor };
            }
            return Atom();
        }
        public INode ArithmeticExpression()
        {
            return ArithmeticBinaryOperation(Term, new List<TokenType>() { TokenType.PLUS, TokenType.MINUS });
        }
        public INode ComparisonExpression()
        {
            if (CurrentToken.Matches(TokenType.KEYW, "not"))
            {
                var operationToken = CurrentToken;
                Next();
                var node = ComparisonExpression();
                return new UnaryOpNode { Node = node, Operation = operationToken };
            }
            return ArithmeticBinaryOperation(ArithmeticExpression, new List<TokenType>() { TokenType.EE, TokenType.NEQ, TokenType.LT, TokenType.GT, TokenType.LTEQ, TokenType.GTEQ });
        }
        public INode Term()
        {
            return ArithmeticBinaryOperation(Factor, new List<TokenType>() { TokenType.MULT, TokenType.DIV });
        }
        public INode Expression()
        {
            if (CurrentToken.TokenType == TokenType.KEYW)
            {
                Next();
                if (CurrentToken.TokenType == TokenType.IDENTIF)
                {
                    var varName = (string)CurrentToken.Value;
                    Next();
                    if (CurrentToken.TokenType == TokenType.EQ)
                    {
                        Next();
                        var expression = Expression();
                        return new VarAssignNode() { Name = varName, ValueNode = expression };
                    }
                }
            }
            return ComparisonBinaryOperation(ComparisonExpression, new List<Token>()
                {
                    new Token() { TokenType = TokenType.KEYW, Value = "and" },
                    new Token() { TokenType = TokenType.KEYW, Value = "or" }
                });
        }/// <summary>
        /// /
        /// </summary>
        /// <param name="functionA"></param>
        /// <param name="operations"></param>
        /// <param name="functionB"></param>
        /// <returns></returns>
        public INode ArithmeticBinaryOperation(Func<INode> functionA, List<TokenType> operations = null, Func<INode> functionB = null)
        {
            if (functionB == null)
            {
                functionB = functionA;
            }
            var left = functionA();
            Next();
            while (operations.Contains(CurrentToken.TokenType))
            {
                var operationToken = CurrentToken;
                Next();
                var right = functionB();
                left = new BinaryOpNode { Left = left, Right = right, Operation = operationToken };
                Next();
            }
            Previous();
            return left;
        }
        public INode ComparisonBinaryOperation(Func<INode> functionA, List<Token> keywords, Func<INode> functionB = null)
        {
            if (functionB == null)
            {
                functionB = functionA;
            }
            var left = functionA();
            Next();

            var ff = keywords[0].TokenType == CurrentToken.TokenType;

            var ffd = keywords[0].Value.Equals(CurrentToken.Value);

            while (keywords.Any(x => x.TokenType == CurrentToken.TokenType && x.Value.Equals(CurrentToken.Value)))
            {
                var operationToken = CurrentToken;
                Next();
                var right = functionB();
                left = new BinaryOpNode { Left = left, Right = right, Operation = operationToken };
                Next();
            }
            Previous();
            return left;
        }
    }
}
