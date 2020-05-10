using System;
using System.Collections.Generic;
using System.Text;

namespace Interpreter
{
    public class Position
    {
        public int Line { get; set; }
        public int Index { get; set; }

        public Position(int line, int index)
        {
            Line = line;
            Index = index;
        }
    }
}