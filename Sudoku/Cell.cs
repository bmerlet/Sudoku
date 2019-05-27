using System;
using System.Collections.Generic;
using System.Text;

namespace Sudoku
{
    /// <summary>
    /// Cell of sudoku puzzle
    /// </summary>
    public class Cell
    {
        // Value of the cell (0-9), 0 means unmarked
        public uint Value { get; private set; }

        // If the cell is marked
        public bool IsMarked => Value != 0;

        // Round at which the cell got marked, 0 if none
        public uint Round { get; private set; }

        // Type of mark
        public EMarkType Type { get; private set; }

        // Possible values, indexed by  Value-1. 0 means possible, round that made the value impossible otherwise 
        private readonly uint[] possible = new uint[Position.ROW_COL_SEC_SIZE];

        public Cell()
        {
        }

        public void Mark(uint value, uint round, EMarkType type)
        {
            Value = value;
            Round = round;
            Type = type;

            for(int v = 0; v < possible.Length; v++)
            {
                if (possible[v] == 0)
                {
                    possible[v] = round;
                }
            }
        }

        public void Unmark(uint round)
        {
            if (Round == round || round == 0)
            {
                Value = 0;
                Round = 0;
            }

            for (int v = 0; v < possible.Length; v++)
            {
                if (possible[v] == round)
                {
                    possible[v] = 0;
                }
            }
        }

            public bool IsPossible(uint value)
        {
            if (value == 0)
            {
                throw new InvalidOperationException("Called IsPossible with zero value");
            }

            return possible[value - 1] == 0;
        }

        public bool SetImpossible(uint value, uint round)
        {
            if (value == 0)
            {
                throw new InvalidOperationException("Called SetImpossible with zero value");
            }

            if (possible[value - 1] == 0)
            {
                possible[value - 1] = round;
                return true;
            }

            return false;
        }

        public void SetPossible(uint value)
        {
            if (value == 0)
            {
                throw new InvalidOperationException("Called SetPossible with zero value");
            }

            possible[value - 1] = 0;
        }

        public uint CountPossibilities()
        {
            uint count = 0;

            for (int v = 0; v < possible.Length; v++)
            {
                if (possible[v] == 0)
                {
                    count += 1;
                }
            }

            return count;
        }

        public bool SamePossibilities(Cell otherCell)
        {
            foreach (var cv in Cell.AllValidCellValues)
            {
                if (IsPossible(cv) != otherCell.IsPossible(cv))
                {
                    return false;
                }
            }

            return true;
        }


        public void Reset()
        {
            Value = 0;
            Round = 0;
            possible.Initialize();
        }

        public override string ToString()
        {
            string result = "";

            if (IsMarked)
            {
                result = $"Marked val {Value}, Round {Round}, Type {Type}";
            }
            else
            {
                result = $"Unmarked 1={possible[0]} 2={possible[1]} 3={possible[2]} 4={possible[3]} 5={possible[4]} 6={possible[5]} 7={possible[6]} 8={possible[7]} 9={possible[8]}";
            }

            return result;
        }

        public void Clone(Cell src)
        {
            Value = src.Value;
            Round = src.Round;
            Type = src.Type;
            for (int i = 0; i < possible.Length; i++)
            {
                possible[i] = src.possible[i];
            }
        }


        static public IEnumerable<uint> AllValidCellValues
        {
            get
            {
                for (uint i = 0; i < Position.ROW_COL_SEC_SIZE; i++)
                {
                    yield return i + 1;
                }
            }
        }
    }
}
