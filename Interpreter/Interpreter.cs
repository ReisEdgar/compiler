using Interpreter.ASTNodes;
using Interpreter.Value;
using System;
using System.Collections.Generic;
using System.Text;

namespace Interpreter
{
    public class Interpreter
    {
        public NumberValue VisitNumberNode(NumberNode node, Context context)
        {
            return new NumberValue() { Value = (int)node.NumberToken.Value };
        }
        public IValue VisitUnaryNode(UnaryOpNode node, Context context)
        {
            var value = TraverseTree(node.Node, context);
            var operation = node.Operation;

            if (operation.TokenType == TokenType.PLUS)
            {
                return value;
            }
            else if (operation.TokenType == TokenType.MINUS)
            {
                var number = ((float)value.Value) * (-1);
                return new NumberValue { Value = number};
            }
            throw new Exception();
        }
        public IValue VisitVarAssignNode(VarAssignNode node, Context context)
        {
            var name = node.Name;
            var value = TraverseTree(node.ValueNode, context);
            context.Symbols.SetVariable(name, value);
            return value;
        }
        public IValue VisitVarAccessNode(VarAccessNode node, Context context)
        {
            return context.Symbols.GetVariable(node.VarName);
        }
        public IValue VisitBinaryOpNode(BinaryOpNode node, Context context)
        {
            var valueRight = (TraverseTree(node.Left, context) as IValue).Value;
            var valueLeft = (TraverseTree(node.Right, context) as IValue).Value;
            var operation = (node.Operation as Token).TokenType;

            if (TokenTypeGroups.Numeric.Contains(operation))
            {
                var right = (float)valueRight;
                var left = (float)valueLeft;
                if (operation == TokenType.PLUS)
                {
                    return new NumberValue { Value = right + left };
                }
                else if (operation == TokenType.MINUS)
                {
                    return new NumberValue { Value = right - left };
                }
                else if (operation == TokenType.DIV)
                {
                    return new NumberValue { Value = right / left };
                }
                else if (operation == TokenType.MULT)
                {
                    return new NumberValue { Value = right * left };
                }
                else if (operation == TokenType.LT)
                {
                    return new BooleanValue { Value = right < left };
                }
                else if (operation == TokenType.GT)
                {
                    return new BooleanValue { Value = right > left };
                }
                else if (operation == TokenType.LTEQ)
                {
                    return new BooleanValue { Value = right <= left };
                }
                else if (operation == TokenType.GTEQ)
                {
                    return new BooleanValue { Value = right >= left };
                }
            }
            else 
            {
                if (operation == TokenType.EE)
                {
                    return new BooleanValue { Value = valueRight.Equals(valueLeft) };
                }
                else if (operation == TokenType.NEQ)
                {
                    return new BooleanValue { Value = !valueRight.Equals(valueLeft) };
                } else if (operation == TokenType.KEYW)
                {
                    var operationValue = (node.Operation as Token).Value;
                    var right = (bool)valueRight;
                    var left = (bool)valueLeft;
                    if (operationValue.Equals("and"))
                    {
                        return new BooleanValue { Value = right && left };
                    } else if (operationValue.Equals("or"))
                    {
                        return new BooleanValue { Value = right || left };
                    }
                }
            }
            throw new Exception();
        }

        public IValue TraverseTree(INode node, Context context)
        {
            if (node is NumberNode)
            {
                return VisitNumberNode(node as NumberNode, context);
            }
            else if (node is BinaryOpNode)
            {
                return VisitBinaryOpNode(node as BinaryOpNode, context);
            }           
            else if (node is UnaryOpNode)
            {
                return VisitUnaryNode(node as UnaryOpNode, context);
            }
            else if(node is VarAccessNode)
            {
                return VisitVarAccessNode(node as VarAccessNode, context);
            }
            else if(node is VarAssignNode)
            {
                return VisitVarAssignNode(node as VarAssignNode, context);
            }
           
            throw new Exception();
        }

    }
}
