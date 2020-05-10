using Interpreter.Value;
using System;
using System.Collections.Generic;
using System.Text;

namespace Interpreter.ASTNodes
{
    public class VarAssignNode: INode
    {
        public string Name { get; set; }
        public INode ValueNode { get; set; }
    }
}
