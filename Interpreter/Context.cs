using System;
using System.Collections.Generic;
using System.Text;

namespace Interpreter
{
    public class Context
    {
        public Context Parent { get; set; }
        public SymbolTable Symbols { get; set; } = new SymbolTable();
    }
}
