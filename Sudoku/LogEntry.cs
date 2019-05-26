using System;
using System.Collections.Generic;
using System.Text;

using Toolbox.Attributes;

namespace Sudoku
{
    internal class LogEntry
    {
        public readonly uint Round;
        public readonly EMarkType Type;
        public readonly Position Position;
        public readonly uint Value;

        public LogEntry(uint round, EMarkType type, Position position = null, uint value = uint.MaxValue)
        {
            Round = round;
            Type = type;
            Position = position;
            Value = value;
        }

        public override string ToString()
        {
            var str = $"Round: {Round} - {EnumDescriptionAttribute.GetDescription(Type)}";

            if (Position != null || Value != uint.MaxValue)
            {
                str += $" (";
            }

            if (Position != null)
            {
                str += $"Row {Position.Row}, Column {Position.Column}";
            }

            if (Position != null && Value != uint.MaxValue)
            {
                str += ", ";
            }

            if (Value != uint.MaxValue)
            {
                str += $"Value = {Value}";
            }

            if (Position != null || Value != uint.MaxValue)
            {
                str += $")";
            }

            return str;
        }
    }
}
