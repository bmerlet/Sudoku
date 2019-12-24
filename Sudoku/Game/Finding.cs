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
            return $"Found value {Value} at {Position.ToReadableString()} of type {Type}";
        }
    }

    internal class FoundGuess : Finding
    {
        public FoundGuess(uint round)
            : base(null, 0, round, EMarkType.GUESS)
        { }

        public override string ToString()
        {
            return "Take a guess!";
        }
    }

    internal class FoundImpossibility : Finding
    {
        public FoundImpossibility(Position position, uint value, uint round, EMarkType type)
            : base(position, value, round, type)
        { }

        public override string ToString()
        {
            return $"Found impossible value {Value} at {Position.ToReadableString()} because of {Type}";
        }
    }
}
