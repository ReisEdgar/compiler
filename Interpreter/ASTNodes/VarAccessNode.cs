using System;
using System.Collections.Generic;
using System.Text;

namespace Interpreter.ASTNodes
{
    public class VarAccessNode : INode
    {
        public string VarName { get; set; }

    }
}
