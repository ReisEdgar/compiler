using Interpreter.Value;
using System;
using System.Collections.Generic;
using System.Text;

namespace Interpreter
{
    public class SymbolTable
    {
        public Dictionary<string, IValue> Symbols { get; set; } = new Dictionary<string, IValue>();
        public SymbolTable Parent { get; set; }

        public IValue GetVariable(string variableName)
        {
            IValue value = null;
            if(Symbols.TryGetValue(variableName, out value))
            {
                return value;
            }
            else if (Parent != null)
            {
                return Parent.GetVariable(variableName);
            }
            throw new Exception("Variable '{0}' doesn't exist in current context");
        }
        public void SetVariable(string name, IValue value)
        {
            Symbols.Add(name, value);
        }
        public void RemoveVariable(string name)
        {
            Symbols.Remove(name);
        }
    }
}
