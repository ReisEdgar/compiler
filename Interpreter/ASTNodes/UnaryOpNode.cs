using System;
using System.Collections.Generic;
using System.Text;

namespace Interpreter.ASTNodes
{
    public class UnaryOpNode : INode
    {
        public INode Node { get; set; }
        public Token Operation { get; set; }
    }
}
