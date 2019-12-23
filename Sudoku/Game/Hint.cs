using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sudoku.Game
{
    public class Hint
    {
        public readonly Position Position;
        public readonly uint Value;
        public readonly string Explanation;

        public Hint(Position position, uint value, string explanation)
        {
            Position = position;
            Value = value;
            Explanation = explanation;
        }
    }
}
