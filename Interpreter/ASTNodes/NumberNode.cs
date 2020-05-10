using System;
using System.Collections.Generic;
using System.Text;

namespace Interpreter.ASTNodes
{
    public class NumberNode : INode
    {
        public Token NumberToken { get; set; }
        
    }
}