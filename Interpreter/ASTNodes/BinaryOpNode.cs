using System;
using System.Collections.Generic;
using System.Text;

namespace Interpreter.ASTNodes
{
    public class BinaryOpNode: INode
    {
        public INode Left { get; set; }
        public INode Right { get; set; }
        public Token Operation { get; set; }
    }
}
