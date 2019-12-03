using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sudoku.Game
{
    /// <summary>
    /// Class to record a finding (remove an impossibility or mark a cell)
    /// </summary>
    internal class Finding
    {
        public readonly Position Position;
        public readonly uint Value;
        public readonly uint Round;
        public readonly EMarkType Type;

        protected Finding(Position position, uint value, uint round, EMarkType type)
        {
            Position = position;
            Value = value;
            Round = round;
            Type = type;
        }
    }

    internal class FoundValue : Finding
    {
        public FoundValue(Position position, uint value, uint round, EMarkType type)
            : base(position, value, round, type)
        { }

        public override string ToString()
        {
            return $"Found value {Value} at position {Position} of type {Type}";
        }
    }

    internal class FoundImpossibility : Finding
    {
        public FoundImpossibility(Position position, uint value, uint round, EMarkType type)
            : base(position, value, round, type)
        { }

        public override string ToString()
        {
            return $"Found impossible value {Value} at position {Position} because of {Type}";
        }
    }
}
