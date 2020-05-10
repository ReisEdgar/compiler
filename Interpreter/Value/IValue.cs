using System;
using System.Collections.Generic;
using System.Text;

namespace Interpreter.Value
{
    public interface IValue
    {
        object Value { get; set; }
    }
}
